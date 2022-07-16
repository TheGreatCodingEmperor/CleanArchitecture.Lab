namespace Application.Repository.Interface;
public interface IRepository<TEntity,TKey>
where TEntity: class,new(){
    IEnumerable<TEntity> GetAll ();
    TEntity? FindById (TKey id);
    TEntity Add(TEntity entity);
    IEnumerable<TEntity> Add(IEnumerable<TEntity> entities);
    TEntity Update(TEntity entity);
    IEnumerable<TEntity> Update(IEnumerable<TEntity> entities);
    void Delete(TKey id);
    void Delete(IEnumerable<TKey> ids);
    void BeginTransaction();
    void Commit();
    void RollBack();
}