namespace Application.Repository.Interface;
public interface IRepository<TDomain,TEntity,TKey>
where TDomain: class,new()
where TEntity: class,new(){
    IEnumerable<TDomain> GetAll ();
    TDomain? FindById (TKey id);
    TDomain Add(TDomain entity);
    IEnumerable<TDomain> Add(IEnumerable<TDomain> domains);
    TDomain Update(TDomain domain);
    IEnumerable<TDomain> Update(IEnumerable<TDomain> domains);
    void Delete(TKey id);
    void Delete(IEnumerable<TKey> ids);
    void BeginTransaction();
    void Commit();
    void RollBack();
}