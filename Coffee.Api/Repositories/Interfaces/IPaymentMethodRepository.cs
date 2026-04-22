using Coffee.Core.Entities;

namespace Coffee.Api.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
// Obtiene todas las formas de pago configuradas en el sistema
    Task<IEnumerable<PaymentMethod>> GetAllAsync();

    // Busca un método de pago específico por su identificador
    Task<PaymentMethod?> GetByIdAsync(int id);

    // Registra una nueva modalidad de pago (ej. Pago con QR, Cripto)
    Task<bool> SaveAsync(PaymentMethod method);

    // Modifica el nombre o configuración de un método de pago existente
    Task<bool> UpdateAsync(PaymentMethod method);

    // Elimina un método de pago del catálogo disponible
    Task<bool> DeleteAsync(int id);
}