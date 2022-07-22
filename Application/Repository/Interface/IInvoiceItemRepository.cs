using DAL.Entity;
using Core.Domain;

namespace Application.Repository.Interface;
public interface IInvoiceItemRepository:IRepository<InvoiceItemDomain,InvoiceItemEntity,int>{
}