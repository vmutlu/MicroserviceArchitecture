using DotnetMicroserviceArchitecture.UI.Settings;
using Microsoft.Extensions.Options;

namespace DotnetMicroserviceArchitecture.UI.Helpers
{
    public class PhotoURLEditHelper
    {
        private readonly ApiSettings _apiSettings;

        public PhotoURLEditHelper(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
        }

        public string GetPhotoURL(string imageUrl) => $"{_apiSettings.ImageURL}/images/{imageUrl}";

        public string GetPhotoRemovePath(string imageUrl)
        {
            var splitURL = imageUrl.Split("/");
            return splitURL[4];
        }
    }
}
