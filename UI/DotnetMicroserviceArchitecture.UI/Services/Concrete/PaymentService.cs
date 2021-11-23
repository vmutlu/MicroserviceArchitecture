using DotnetMicroserviceArchitecture.UI.Models;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.UI.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> ReceivePayment(PaymentInfoRequest paymentInfoRequest)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInfoRequest>("payments/payments", paymentInfoRequest).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}
