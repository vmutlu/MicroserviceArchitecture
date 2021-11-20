using DotnetMicroserviceArchitecture.UI.Models;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface IBasketService
    {
        Task<bool> AddOrUpdateAsync(BasketView basketView);
        Task<BasketView> GetAsync();
        Task<bool> DeleteAsync();
        Task AddBasketItemAsync(BasketItemView basketItemView);
        Task<bool> DeleteBasketItemAsync(string courseId);
        Task<bool> ApplyDiscountAsync(string discountCode);
        Task<bool> CancelApplyDiscountAsync();
    }
}
