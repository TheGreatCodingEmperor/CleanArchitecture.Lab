using DAL.Entity;
using Core.Domain;

namespace Application.Repository.Interface;
public interface ICustomerRepository:IRepository<CustomerDomain,CustomerEntity,int>{
    (int count,IEnumerable<CustomerDomain> list) PageQuery(string? firstName, int? page, int? pageSize);
}