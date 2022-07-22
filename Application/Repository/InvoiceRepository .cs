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
public class InvoiceRepository :SingleKeyRepository<InvoiceDomain,InvoiceEntity,int>, IInvoiceRepository{
    public InvoiceRepository(MyContext dbContext){
        base.DbContext = dbContext;
    }
}