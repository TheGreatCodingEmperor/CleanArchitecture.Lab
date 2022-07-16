using Core.Domain;
namespace Application.Manager.Interface;
public interface ICustomerManager:IManager<CustomerDomain,int>{
    (int count,IEnumerable<CustomerDomain> list) PageQuery(string? firstName, int? page,int? pageSize);
}