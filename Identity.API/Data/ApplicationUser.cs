using System.Collections;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.API.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Nickname { get; set; }
        public string Role { get; set; }
        public string PhotoUrl { get; set; }
    }
}