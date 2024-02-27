using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using ISG.WebApi.Models;
using ISG.WebApi.Infrastructure;
using ISG.WebApi.Providers;

namespace ISG.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }

        [Authorize(Roles = "Admin , ISG_Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var isgUsers = this.AppUserManager.Users.ToList();
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize]
        //http://localhost:1940/api/accounts/GetIsgUser?name=ısg_hekim
        public IHttpActionResult GetIsgUser(string name)
        {
            var dr = this.AppUserManager.Users.ToList().Where(s => s.Meslek == name).Select(u => new drListesi() { adi = u.FirstName, soyadi = u.LastName });
            //where sorgulamada select seçileçek yer hakkıda yeni drlistesi oluşturuluyor.
            return Ok(dr);
        }

        [Authorize]
        [Route("user/{id:guid}", Name = "GetUserById")]
        //http://localhost:1940/api/accounts/user/114f7396-98e8-4b2b-bb47-f1e93c8394bf
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize]
        [Route("user/{username}")]
        //http://localhost:1940/api/accounts/user/dilem
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Level = 3,
                JoinDate = GuncelTarih().Date,
                Meslek =createUserModel.Meslek,
                Gorevi =createUserModel.Gorevi,
                Resim = createUserModel.Resim,
                Tel = createUserModel.Tel,
                Tel1 = createUserModel.Tel1,
                MedullaPassw=createUserModel.MedullaPassw,
                key=createUserModel.key,
                TcNo=createUserModel.TcNo,
                doktorBransKodu=createUserModel.doktorBransKodu,
                doktorSertifikaKodu=createUserModel.doktorSertifikaKodu,
                doktorTesisKodu=createUserModel.doktorTesisKodu
            };

        IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            
            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);


            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(user.Id, code);

            //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

            //await this.AppUserManager.SendEmailAsync(user.Id,
            //                                        "Confirm your account",
            //                                        "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            //return Created(locationHeader, TheModelFactory.Create(user));

            return Ok();
        }

        [Authorize]
        [Route("update")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateUser(UpdateUserBindingModel updateUserBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(updateUserBindingModel.Id);
            //update uygun model alınır.

            user.UserName = updateUserBindingModel.Username;
            user.Email = updateUserBindingModel.Email;
            user.FirstName = updateUserBindingModel.FirstName;
            user.LastName = updateUserBindingModel.LastName;
            user.Meslek = updateUserBindingModel.Meslek;
            user.Gorevi = updateUserBindingModel.Gorevi;
            user.Resim = updateUserBindingModel.Resim;
            user.Tel = updateUserBindingModel.Tel;
            user.Tel1 = updateUserBindingModel.Tel1;
            user.MedullaPassw = updateUserBindingModel.MedullaPassw;
            user.key = updateUserBindingModel.key;
            user.TcNo = updateUserBindingModel.TcNo;
            user.doktorBransKodu = updateUserBindingModel.doktorBransKodu;
            user.doktorSertifikaKodu = updateUserBindingModel.doktorSertifikaKodu;
            user.doktorTesisKodu = updateUserBindingModel.doktorTesisKodu;

            IdentityResult updateUserResult = await this.AppUserManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                return GetErrorResult(updateUserResult);
            }
            return Ok();
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Lockout/{userId:guid}/{isLockoutEnabled}")]
        //http://localhost:1943/api/accounts/Lockout/6067373f-d5de-42c4-804e-f1838fad9fc1/false
        public async Task<IHttpActionResult> SetLockoutEnabled(string userId, bool isLockoutEnabled)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                ModelState.AddModelError("", "User Id ve Code gereklidir.");
                return BadRequest(ModelState);
            }

            IdentityResult result = await AppUserManager.SetLockoutEnabledAsync(userId,isLockoutEnabled);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]//kullanıcının güncelleyeceği metod
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model,string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await this.AppUserManager.ResetPasswordAsync(userId, model.Code, model.Password);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }


        [HttpPost]
        [Authorize]
        [Route("aup/{userId:guid}/{newPassword}")]
        public async Task<IHttpActionResult> AdminUpdatePassword(string userId,string newPassword)
        {
            var appUser = await this.AppUserManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                await this.AppUserManager.RemovePasswordAsync(userId);
                IdentityResult result = await AppUserManager.AddPasswordAsync(userId,newPassword);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok();
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.AppUserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }

    }
}
