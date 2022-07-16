using Application.Extension;
using Application.Manager.Interface;
using Application.Repository.Interface;
using Core.Domain;
using DAL.Entity;

namespace Application.Manager;
public class CustomerManager : SinglerKeyManager<CustomerDomain,CustomerEntity,int>, ICustomerManager {
    public CustomerManager(
        ICustomerRepository repository
    ){
        Repository = repository;
    }
}