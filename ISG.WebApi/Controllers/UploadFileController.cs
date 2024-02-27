using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ISG.Entities.ComplexType;
using ISG.WebApi.Infrastructure;
using System.Net.Http.Headers;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using System.Globalization;
using System.Dynamic;

namespace ISG.WebApi.Controllers
{
    [RoutePrefix("api/Img")]
   // [Authorize]
    public class UploadFileController : ApiController
    {
        private IImageUploadService _imageUploadService;
        private  ISirketUploadService _sirketUploadService;

        public UploadFileController(IImageUploadService imageUploadService,
            ISirketUploadService sirketUploadService)
        {
            _imageUploadService = imageUploadService;
            _sirketUploadService = sirketUploadService;
        }

        #region Bu alanda personel uploadı için yazıldı

        [HttpDelete]
        [Route("{genericName}/{mime}")]
        public async Task<int> Delete(string genericName, string mime)
        {
            try
            {
                imageUploadFilter asd = new imageUploadFilter() { GenericName = mime != "x" ? genericName + "." + mime : genericName };
                imageUpload imu = await _imageUploadService.FindAsync(asd);
                int iReturnValue = await _imageUploadService.DeleteAsync(imu.id);
                DeleteFtpFile(imu.GenericName);
                if (iReturnValue > 0)
                {
                    //UploadFileFtp(genericName, Pathx, WebRequestMethods.Ftp.DeleteFile);

                    var Pathx = Path.Combine(HttpContext.Current.Server.MapPath("~/uploads"), genericName = mime != "x" ? genericName + "." + mime : genericName);
                    System.IO.File.Delete(Pathx);
                    return iReturnValue;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                return 1;
            }
        }

        [HttpGet]
        [Route("{IdGuid}/{Folder}/{idx?}")]
        public async Task<List<FileUploadResult>> GetAll(string IdGuid, string Folder, int idx = 0)
        {
            imageUploadFilter asd = new imageUploadFilter();
            var FUR = new List<FileUploadResult>();
            asd.IdGuid = IdGuid;
            asd.Folder = Folder;
            asd.Protokol = idx;
            IEnumerable<imageUpload> qw = idx > 0 ? await _imageUploadService.FindAllFileAsync(asd) :
                await _imageUploadService.FindAllAsync(asd);
            qw.ToList().ForEach(x => FUR.Add(new FileUploadResult()
            {
                FileLength = x.FileLenght,
                FileName = x.FileName,
                GenericName = x.GenericName,
                LocalFilePath = ConfigurationManager.AppSettings["storageLink"] + "uploads/" + x.GenericName
            }));
            return FUR;
        }

        [HttpPost]
        [Route("{IdGuid}/{Folder}/{idx?}")]
        public async Task<IEnumerable<FileUploadResult>> Post(string IdGuid, string Folder, int idx = 0)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }

            string fullPath = HttpContext.Current.Server.MapPath("~/uploads");//tüm dosyalarn yükleneceği yolu belirliyoruz.

            var streamProvider = new CustomMultipartFormDataStreamProvider(fullPath);
            //tüm dosyalar belirtilen klasöre yükleniyor. MultipartFormDataStreamProvider
            //MultipartFormDataStreamProvider CustomMultipartFormDataStreamProvider ile ezerek istediğimiz adla upload ediyoruz.
            try
            {//mime dosyalarını okuyoruz
                await Request.Content.ReadAsMultipartAsync(streamProvider);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, ex.Message.ToString()+ fullPath 
                      ));}//mime hatası klasör izninden kaynaklı geçiş yapmıyorsa stream üzerinde ftp yapılabili kod örneği aşağıda.....                                                                          

            //upload ettiğimiz dosyaları okuyoruz
            var fileInfo = streamProvider.FileData.Select(i =>
            {
                var uploadFiles = new imageUpload();
                var info = new FileInfo(i.LocalFileName);
                uploadFiles.IdGuid = IdGuid;
                uploadFiles.Protokol = idx;
                uploadFiles.Folder = Folder;
                uploadFiles.FileLenght = Convert.ToInt32(info.Length);
                var sdff = i.Headers.ContentDisposition.FileName;
                uploadFiles.FileName = sdff.Substring(1, sdff.Length - 1).Substring(0, sdff.Substring(1, sdff.Length - 1).Length - 1);
                //                       son karakter silindi                     başındaki karakter silindi.
                uploadFiles.GenericName = info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1];
                //split array neslesine dönüşüp son elemanı aldım.
                uploadFiles.MimeType = i.Headers.ContentType.MediaType;
                uploadFiles.UserId = User.Identity.GetUserId();
                var result = _imageUploadService.AddAsync(uploadFiles);
                UploadFileFtp(info.Name, info.FullName, WebRequestMethods.Ftp.UploadFile);
                uploadFiles = null;
                return new FileUploadResult()
                {
                    FileLength = Convert.ToInt32(info.Length),
                    FileName = sdff.Substring(1, sdff.Length - 1).Substring(0, sdff.Substring(1, sdff.Length - 1).Length - 1),
                    LocalFilePath = ConfigurationManager.AppSettings["storageLink"] + "uploads/" + info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1],
                    GenericName = info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1]
                };
            });
            return fileInfo;
        }

        //http://localhost:1943/api/Img/down/cc81e63a-d184-4f61-a066-4e09ac78a3d4
        [AllowAnonymous]
        [HttpGet]
        [Route("down/{genericName}")]
        public async Task<HttpResponseMessage> Download(string genericName)
        {
            string fullPath = HttpContext.Current.Server.MapPath("~/uploads");
            string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
            string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
            string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                imageUploadFilter asd = new imageUploadFilter() { GenericName = genericName };
                imageUpload imu = await _imageUploadService.FindAsync(asd);
                if (ftp_Work == "true")
                {
                    var ftpRequest = (FtpWebRequest)WebRequest.Create(ftp_Link + genericName);
                    ftpRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    var ftpStream = ftpResponse.GetResponseStream();
                    result.Content = new StreamContent(ftpStream);
                }
                else
                    result.Content = new StreamContent(new FileStream(fullPath + "\\" + genericName, FileMode.Open, FileAccess.Read));

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = imu.FileName;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(imu.MimeType);
                result.Content.Headers.ContentLength = imu.FileLenght;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, await Task.Run(() => ex.Message.ToString() + " İndirelemedi!"));
            }
            return result;
        }
        #endregion

        #region Bu alan şirketlerin upload/download bölümü için yazıldı

        [Route("PostSti")]// file ve object bir arada gönderebiliyoruz.
        [HttpPost]
        public async Task<IEnumerable<FileUploadResult>> PostSti()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }
            string fullPath = HttpContext.Current.Server.MapPath("~/uploads");
            var streamProvider = new CustomMultipartFormDataStreamProvider(fullPath);
            try
            {
                CustomMultipartFormDataStreamProvider filesReadToProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
                IEnumerable<FileUploadResult> fileInfo = streamProvider.FileData.Select(i =>
                {
                    var uploadFiles = new SirketUpload();
                    var info = new FileInfo(i.LocalFileName);
                    uploadFiles.DosyaTipi = Task.Run(async () => await filesReadToProvider.Contents[1].ReadAsStringAsync()).Result;
                    if (uploadFiles.DosyaTipi == null) throw new ArgumentException("Dosya Türünü Giriniz!");
                    uploadFiles.DosyaTipiID = Task.Run(async () => await filesReadToProvider.Contents[2].ReadAsStringAsync()).Result; ;
                    uploadFiles.Konu = Task.Run(async () => await filesReadToProvider.Contents[3].ReadAsStringAsync()).Result;
                    uploadFiles.Hazırlayan = Task.Run(async () => await filesReadToProvider.Contents[4].ReadAsStringAsync()).Result;

                    uploadFiles.Sirket_Id = Convert.ToInt32(Task.Run(async () => await filesReadToProvider.Contents[5].ReadAsStringAsync()).Result);
                    if (uploadFiles.Sirket_Id == 0) throw new ArgumentException("Şirketi Giriniz!");
                    uploadFiles.FileLenght = Convert.ToInt32(info.Length);
                    var sdff = i.Headers.ContentDisposition.FileName;
                    uploadFiles.FileName = sdff.Substring(1, sdff.Length - 1).Substring(0, sdff.Substring(1, sdff.Length - 1).Length - 1);
                    //  son karakter silindi                     başındaki karakter silindi.
                    uploadFiles.GenericName = info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1];
                    //split array neslesine dönüşüp son elemanı aldım.
                    uploadFiles.MimeType = i.Headers.ContentType.MediaType;
                    uploadFiles.UserId = User.Identity.GetUserId();
                    var result = _sirketUploadService.AddAsync(uploadFiles);
                    UploadFileFtp(info.Name, info.FullName, WebRequestMethods.Ftp.UploadFile);
                    uploadFiles = null;
                    return new FileUploadResult()
                    {
                        FileLength = Convert.ToInt32(info.Length),
                        FileName = sdff.Substring(1, sdff.Length - 1).Substring(0, sdff.Substring(1, sdff.Length - 1).Length - 1),
                        LocalFilePath = ConfigurationManager.AppSettings["storageLink"] + "uploads/" + info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1],
                        GenericName = info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1]
                    };
                });
                return fileInfo;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format(ex.Message)),
                    ReasonPhrase = ex.Source

                };
                throw new HttpResponseException(resp);
            }
        }

        [HttpDelete]
        [Route("stimg/{genericName}/{mime}")]
        public async Task<int> DeleteSti(string genericName, string mime)
        {
            try
            {
                imageUploadFilter asd = new imageUploadFilter() { GenericName = mime != "x" ? genericName + "." + mime : genericName };
                SirketUpload imu = await _sirketUploadService.FindAsync(asd);
                int iReturnValue = await _sirketUploadService.DeleteAsync(imu.SirketUpload_Id);
                DeleteFtpFile(imu.GenericName);
                if (iReturnValue > 0)
                {
                    //UploadFileFtp(genericName, Pathx, WebRequestMethods.Ftp.DeleteFile);
                    var Pathx = Path.Combine(HttpContext.Current.Server.MapPath("~/uploads"), genericName = mime != "x" ? genericName + "." + mime : genericName);
                    System.IO.File.Delete(Pathx);
                    return iReturnValue;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                return 1;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("stiDown/{genericName}")]
        public async Task<HttpResponseMessage> DownloadSti(string genericName)
        {
            string fullPath = HttpContext.Current.Server.MapPath("~/uploads");
            string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
            string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
            string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                imageUploadFilter asd = new imageUploadFilter() { GenericName = genericName };
                SirketUpload imu = await _sirketUploadService.FindAsync(asd);
                if (ftp_Work == "true")
                {
                    var ftpRequest = (FtpWebRequest)WebRequest.Create(ftp_Link + genericName);
                    ftpRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    var ftpStream = ftpResponse.GetResponseStream();
                    result.Content = new StreamContent(ftpStream);
                }
                else
                    result.Content = new StreamContent(new FileStream(fullPath + "\\" + genericName, FileMode.Open, FileAccess.Read));

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = imu.FileName;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(imu.MimeType);
                result.Content.Headers.ContentLength = imu.FileLenght;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, await Task.Run(() => ex.Message.ToString() + " İndirelemedi!"));
            }
            return result;
        }

       
        [Route("GetAllSti/{DosyaTipi}/{Sirket_Id}/{DosyaTipiID?}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllSti(string DosyaTipi,int Sirket_Id ,string DosyaTipiID)
        {
            imageUploadFilter asd = new imageUploadFilter();
            asd.DosyaTipi = DosyaTipi;
            asd.DosyaTipiID = DosyaTipiID;
            asd.Sirket_Id = Sirket_Id;
            IEnumerable<SirketUpload> qw = DosyaTipiID!= "0" ? await _sirketUploadService.FindAllFileAsync(asd) :
                await _sirketUploadService.FindAllAsync(asd);
            List<ExpandoObject> FUR = new List<ExpandoObject>();
            foreach (SirketUpload i in qw)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                dictionary.Add("FileLenght", i.FileLenght);
                dictionary.Add("FileName", i.FileName);
                dictionary.Add("GenericName", i.GenericName);
                dictionary.Add("LocalFilePath", ConfigurationManager.AppSettings["storageLink"] + "uploads/" + i.GenericName);
                dictionary.Add("konusu", i.Konu);
                dictionary.Add("stili", i.DosyaTipiID);
                dictionary.Add("hazirlayan", i.Hazırlayan);
                FUR.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => FUR));
        }
        #endregion

        #region yardımcı prosedürler

        public static string UploadFileFtp(string localFile, string UploadDirectory, string method)
        {
            try
            {
                string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
                string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
                string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
                string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];
                if (ftp_Work == "true")
                {
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                        Uri uri = new Uri(ftp_Link + localFile);
                        client.UploadFile(uri, method, UploadDirectory);
                    }
                    
                }
                return "Tamam";
            }
            catch (Exception ex )
            {
                return ex.Message.ToString();
            }
        }
        public static string UploadFileStreamFtp(string localFile, Stream st)
        {// dosya izinleri aşılmazsa buradan stream gönderilir.
            try
            {
                string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
                string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
                string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
                string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];
                if (ftp_Work == "true")
                {
                    st.Seek(0, SeekOrigin.Begin);

                    WebRequest request =
                        WebRequest.Create(ftp_Link + localFile);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    using (Stream ftpStream = request.GetRequestStream())
                    {
                        st.CopyTo(ftpStream);
                    }

                }
                return "Tamam";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string DeleteFtpFile(string fileName)
        {
            string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
            string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
            string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];

            try
            {
                if (ftp_Work == "true")
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp_Link + fileName);
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        return response.StatusDescription;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static bool IsPropertyExist(dynamic dynamicObj, string property)
        {
            try
            {
                var value = dynamicObj[property].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }
        }
        public static dynamic Cast(dynamic obj, Type castTo)
        {
            try
            {
                return Convert.ChangeType(obj, castTo);
            }
            catch (FormatException)
            {
                return castTo == typeof(int) ? 0 : 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}

public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider// upload ettiğimiz dosyaların adını burada değiştiriyoruz.
{
    public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

    public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
    {// dosya adını burada belirleyebiliyoruz
        string fileName;
        if (!string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))//dosya adı boş değilse 
        {
            var ext = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));//uzantısnı almak istiyoruz
            var isImg = headers.ContentType.MediaType.ToString().StartsWith("image");
            //blob ise boş gelecek png ile birleşecek
            //image ise kendi dosyası ne ise onla
            ext = !string.IsNullOrEmpty(ext) && isImg ? ext : string.IsNullOrEmpty(ext) ? ".png" : "";
            fileName = Guid.NewGuid() + ext;
        }
        else
        {
            fileName = Guid.NewGuid() + ".data";
        }
        return fileName;
    }
}

/*
 try
            {
                Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
                MemoryStream tempStream = new MemoryStream();
                reqStream.CopyTo(tempStream);

                tempStream.Seek(0, SeekOrigin.End);
                StreamWriter writer = new StreamWriter(tempStream);
                writer.WriteLine();
                writer.Flush();
                tempStream.Position = 0;

                StreamContent streamContent = new StreamContent(tempStream);
                foreach (var header in Request.Content.Headers)
                {
                    streamContent.Headers.Add(header.Key, header.Value);
                }
                if (Request.Content.IsMimeMultipartContent())
                {
                    var task = streamContent.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider());
                    task.Wait();
                    MultipartMemoryStreamProvider provider = task.Result;

                    foreach (HttpContent content in provider.Contents)
                    {
                        //  WebsiteFile newFile = new WebsiteFile();

                        imageUpload newFile = new imageUpload(); ;
                        newFile.IdGuid = Guid.NewGuid().ToString();

                        Stream stream = content.ReadAsStreamAsync().Result;
                        string filePath = HostingEnvironment.MapPath("~/uploads/");
                        string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", "");
                        newFile.FileName = fileName;

                       UploadFileStreamFtp("deneme",stream);// buradan ftp ile stream gönderebilirsin.
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, await Task.Run(() => ex.Message.ToString() + " sorun var "));
            }




*/