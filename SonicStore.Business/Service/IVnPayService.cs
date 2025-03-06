using Microsoft.AspNetCore.Http;
using SonicStore.Business.Dto;

namespace SonicStore.Business.Service;
public interface IVnPayService
{
    string CreatePaymentURL(HttpContext context, VnPayRequestModel model);
    VnPaymentResponseModel PaymentExecute(IQueryCollection collection);
}
