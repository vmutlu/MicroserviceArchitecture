using Microsoft.AspNetCore.Identity;

namespace DotnetMicroserviceArchitecture.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
