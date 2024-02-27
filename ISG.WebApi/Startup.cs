using ISG.WebApi.App_Start;
using ISG.WebApi.Infrastructure;
using ISG.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using System.Data.Entity.Migrations;
using System.Web.Http.Cors;
using Microsoft.Owin.Cors;
using System.Web.Cors;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;

namespace ISG.WebApi
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {            
            HttpConfiguration httpConfig = new HttpConfiguration();
            httpConfig.EnableCors();

            ConfigureOAuthTokenGeneration(app);

            ConfigureOAuthTokenConsumption(app);

            httpConfig.MapHttpAttributeRoutes();

            
            httpConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(httpConfig);

            GlobalConfiguration.Configuration.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
            var json = httpConfig.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            httpConfig.Formatters.Remove(httpConfig.Formatters.XmlFormatter);
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            // GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            app.UseNinjectMiddleware(() => NinjectConfig.CreateKernel.Value);//depency dosyası

            app.UseNinjectWebApi(httpConfig);
            //migrasyon yapsn diye
            //var configuration = new Migrations.Configuration();
            //var migrator = new DbMigrator(configuration);
            //migrator.Update();
            var corsPolicy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                ExposedHeaders = { "Location" },
                SupportsCredentials = true
            };

            //// Try and load allowed origins from web.config
            //// If none are specified we'll allow all origins

            corsPolicy.Origins.Add("*");

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(corsPolicy)
                }
            };
            //app.UseStaticFiles(); https://stackoverflow.com/questions/36294188/using-a-wwwroot-folder-asp-net-core-style-in-asp-net-4-5-project
           
            app.UseCors(corsOptions);

        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(10),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(ConfigurationManager.AppSettings["link"])
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["link"];
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

    }

  
}



//https://www.red-gate.com/simple-talk/blogs/speeding-up-your-application-with-the-iis-auto-start-feature/
//  iis başlangıç performansını artırmak için https://docs.microsoft.com/en-us/biztalk/technical-guides/optimizing-iis-performance
