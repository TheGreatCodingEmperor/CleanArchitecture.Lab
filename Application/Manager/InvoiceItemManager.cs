using Application.Extension;
using Application.Manager.Interface;
using Application.Repository.Interface;
using Core.Domain;
using DAL.Entity;

namespace Application.Manager;
public class InvoiceItemManager : SinglerKeyManager<IInvoiceItemRepository,InvoiceItemDomain,InvoiceItemEntity,int>, IInvoiceItemManager {
    public InvoiceItemManager(
        IInvoiceItemRepository repository
    ){
        Repository = repository;
    }
}