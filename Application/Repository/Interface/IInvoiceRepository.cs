using DAL.Entity;
using Core.Domain;

namespace Application.Repository.Interface;
public interface IInvoiceRepository:IRepository<InvoiceDomain,InvoiceEntity,int>{
}