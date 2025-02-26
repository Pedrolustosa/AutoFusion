using AutoFusion.Domain.Entities;

namespace AutoFusion.Domain.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer, int>
{
    Task<Customer?> FindByCpfAsync(string cpf);
}
