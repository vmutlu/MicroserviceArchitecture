using DotnetMicroserviceArchitecture.UI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface IOrderService
    {
        Task<OrderStatusView> AddOrder(CheckOutInfoRequest checkOutInfoRequest);
        Task<OrderSuspendView> SuspendOrderAsync(CheckOutInfoRequest checkOutInfoRequest);
        Task<List<OrderView>> GetOrderAsync();
    }
}
