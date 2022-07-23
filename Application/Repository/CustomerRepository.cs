using System.Globalization;
using System.Linq.Expressions;
using Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using DAL.Entity;
using Application.Models;
using Core.Domain;

namespace Application.Repository;
public class CustomerRepository:SingleKeyRepository<CustomerDomain,CustomerEntity,int>, ICustomerRepository{
    public CustomerRepository(MyContext dbContext){
        base.DbContext = dbContext;
    }

    public (int,IEnumerable<CustomerDomain>) PageQuery(string? firstName, int? page, int? pageSize)
    {
        var query = DbContext.Set<CustomerEntity>().AsQueryable();
        query = string.IsNullOrEmpty(firstName)? query:query.Where(x => EF.Functions.Like(firstName, x.FirstName));
        return (query.Count(),Paginition(query,page,pageSize));
    }
}