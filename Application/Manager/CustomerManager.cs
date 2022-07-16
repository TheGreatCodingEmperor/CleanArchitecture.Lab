using Application.Extension;
using Application.Manager.Interface;
using Application.Repository.Interface;
using Core.Domain;
using DAL.Entity;

namespace Application.Manager;
public class CustomerManager : SinglerKeyManager<ICustomerRepository,CustomerDomain,CustomerEntity,int>, ICustomerManager {
    public CustomerManager(
        ICustomerRepository repository
    ){
        Repository = repository;
    }

    public override IEnumerable<CustomerDomain> GetAll(){
        return Repository.GetAll().Select(x => x.AutoMap<CustomerDomain,CustomerEntity>());
    }

    public (int,IEnumerable<CustomerDomain>) PageQuery(string? firstName, int? page, int? pageSize)
    {
        var result = Repository.PageQuery(firstName, page, pageSize);
        return (result.count,result.list.Select(x => x.AutoMap<CustomerDomain,CustomerEntity>()));
    }
}