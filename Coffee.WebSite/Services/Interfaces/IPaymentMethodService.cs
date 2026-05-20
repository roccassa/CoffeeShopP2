using Coffee.Core.Dto;
using Coffee.Core.Http;

namespace Coffee.WebSite.Services.Interfaces;

public interface IPaymentMethodService
{
    Task<Response<List<PaymentMethodDto>>> GetAllAsync();
    Task<Response<PaymentMethodDto>> GetByIdAsync(int id);
    Task<Response<PaymentMethodDto>> CreateAsync(PaymentMethodDto dto);
    Task<Response<PaymentMethodDto>> UpdateAsync(PaymentMethodDto dto);
    Task<Response<bool>> DeleteAsync(int id);
}
