using DotnetMicroserviceArchitecture.BasketAPI.DTOs;
using DotnetMicroserviceArchitecture.Core.Dtos;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.BasketAPI.Services.Abstract
{
    public interface IBasketService
    {
        Task<Response<BasketDTO>> GetBasket(string userId);
        Task<Response<bool>> AddOrUpdate(BasketDTO basketDTO);
        Task<Response<bool>> Delete(string userId);
    }
}
