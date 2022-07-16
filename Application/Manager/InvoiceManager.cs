using Application.Extension;
using Application.Manager.Interface;
using Application.Repository.Interface;
using Core.Domain;
using DAL.Entity;

namespace Application.Manager;
public class InvoiceManager : SinglerKeyManager<IInvoiceRepository,InvoiceDomain,InvoiceEntity,int>, IInvoiceManager {
    public InvoiceManager(
        IInvoiceRepository repository
    ){
        Repository = repository;
    }
}