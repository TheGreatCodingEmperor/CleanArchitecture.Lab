using DAL.Entity;

namespace Application.Repository.Interface;
public interface ICustomerRepository:IRepository<CustomerEntity,int>{
    (int count,IEnumerable<CustomerEntity> list) PageQuery(string? firstName, int? page, int? pageSize);
}