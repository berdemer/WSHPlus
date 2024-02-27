using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ISG.WebApi.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public byte Level { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public string Meslek { get; set; }

        public string Gorevi { get; set; }

        public string Resim { get; set; }
        public string Tel { get; set; }
        public string Tel1 { get; set; }
        public string MedullaPassw { get; set; }
        public string key { get; set; }
        public string TcNo { get; set; }
        public string doktorBransKodu { get; set; }
        public string doktorSertifikaKodu { get; set; }
        public string doktorTesisKodu { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            return userIdentity;
        }
    }

}
