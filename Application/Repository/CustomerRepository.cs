using System.Globalization;
using System.Linq.Expressions;
using Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using DAL.Entity;
using Application.Models;

namespace Application.Repository;
public class CustomerRepository:SingleKeyRepository<CustomerEntity,int>, ICustomerRepository{
    public CustomerRepository(MyContext dbContext){
        base.DbContext = dbContext;
    }
}