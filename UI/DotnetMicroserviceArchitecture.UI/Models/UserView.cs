using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class UserView
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

        //Get properties
        public IEnumerable<string> GetUserProperties()
        {
            yield return Id;
            yield return UserName;
            yield return Email;
            yield return City;
        }
    }
}
