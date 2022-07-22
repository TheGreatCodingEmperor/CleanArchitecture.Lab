namespace Application.Repository.Interface;
public interface IRepository<TDomain,TEntity,TKey>
where TDomain: class,new()
where TEntity: class,new(){
    IEnumerable<TDomain> GetAll ();
    TDomain? FindById (TKey id);
    TDomain Add(TDomain entity);
    IEnumerable<TDomain> Add(IEnumerable<TDomain> entities);
    TDomain Update(TDomain entity);
    IEnumerable<TDomain> Update(IEnumerable<TDomain> entities);
    void Delete(TKey id);
    void Delete(IEnumerable<TKey> ids);
    void BeginTransaction();
    void Commit();
    void RollBack();
}