using DotnetMicroserviceArchitecture.UI.Models;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Abstract
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoRequest paymentInfoRequest);
    }
}
