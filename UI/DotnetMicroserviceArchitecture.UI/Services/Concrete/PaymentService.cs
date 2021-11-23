using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> ReceivePayment(PaymentInfoRequest paymentInfoRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}
