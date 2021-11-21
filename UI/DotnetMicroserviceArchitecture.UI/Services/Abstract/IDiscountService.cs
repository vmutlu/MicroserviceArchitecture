using DotnetMicroserviceArchitecture.UI.Models;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface IDiscountService
    {
        Task<DiscountView> GetDiscountAsync(string discountCode);
    }
}
