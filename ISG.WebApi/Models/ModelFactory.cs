using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using ISG.WebApi.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ISG.WebApi.Models
{
    public class ModelFactory
    {

        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Level = appUser.Level,
                JoinDate = appUser.JoinDate,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result,
                Meslek = appUser.Meslek,
                Gorevi = appUser.Gorevi,
                Resim = appUser.Resim,
                Tel = appUser.Tel,
                Tel1 = appUser.Tel1,
                MedullaPassw = appUser.MedullaPassw,
                TcNo = appUser.TcNo,
                doktorBransKodu = appUser.doktorBransKodu,
                doktorSertifikaKodu = appUser.doktorSertifikaKodu,
                doktorTesisKodu = appUser.doktorTesisKodu,
                key=appUser.key,
                LockoutEnabled = appUser.LockoutEnabled
            };

        }

        public RoleReturnModel Create(IdentityRole appRole)
        {

            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };

        }
    }

    public class UserReturnModel
    {

        public string Url { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Level { get; set; }

        public bool LockoutEnabled { get; set; }
        public DateTime JoinDate { get; set; }
        public IList<string> Roles { get; set; }

        public string Gorevi { get; set; }

        public string Meslek { get; set; }

        public string  Resim { get; set; }

        public string Tel { get; set; }
        public string Tel1 { get; set; }
        public string MedullaPassw { get; set; }
        public string key { get; set; }
        public string TcNo { get; set; }
        public string doktorBransKodu { get; set; }
        public string doktorSertifikaKodu { get; set; }
        public string doktorTesisKodu { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }

    }

    public class RoleReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}