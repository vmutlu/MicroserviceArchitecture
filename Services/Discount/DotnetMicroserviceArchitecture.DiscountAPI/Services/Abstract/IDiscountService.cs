using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.DiscountAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.DiscountAPI.Services.Abstract
{
    public interface IDiscountService
    {
        Task<Response<List<Discount>>> GetAll();
        Task<Response<Discount>> GetById(int id);
        Task<Response<NoContent>> Add(Discount discount);
        Task<Response<NoContent>> Update(Discount discount);
        Task<Response<NoContent>> Delete(int id);
        Task<Response<Discount>> GetByCodeAndUserId(string code, string userId);
    }
}
