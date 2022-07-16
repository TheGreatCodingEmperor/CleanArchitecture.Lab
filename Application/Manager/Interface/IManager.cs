namespace Application.Manager.Interface;
public interface IManager<TDomain,TKey>{
    IEnumerable<TDomain> GetAll();
    TDomain? GetById(TKey Id);
    TDomain Add(TDomain domain);
    IEnumerable<TDomain>  Add(IEnumerable<TDomain> domains);
    TDomain Update(TDomain domain);
    IEnumerable<TDomain>  Update(IEnumerable<TDomain> domains);
    void Delete(TKey id);
    void Delete(IEnumerable<TKey> ids);
}