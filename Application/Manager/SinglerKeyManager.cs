using Application.Extension;
using Application.Manager.Interface;
using Application.Repository.Interface;

namespace Application.Manager;
public class SinglerKeyManager<TIRepository, TDomain, TEntity, TKey> : IManager<TDomain, TKey>
    where TDomain : class, new ()
where TEntity : class, new ()
where TIRepository : IRepository<TEntity, TKey> {
    protected TIRepository Repository { get; set; }

    public virtual IEnumerable<TDomain> GetAll () {
        return Repository.GetAll ().Select (x => x.AutoMap<TDomain, TEntity> ());
    }

    public virtual TDomain? GetById (TKey id) {
        return Repository.FindById (id).AutoMap<TDomain, TEntity> ();
    }

    public virtual TDomain Add (TDomain domain) {
        TEntity entity = Repository.Add (domain.AutoMap<TEntity, TDomain> ());
        return entity.AutoMap<TDomain, TEntity> ();
    }

    public virtual IEnumerable<TDomain> Add (IEnumerable<TDomain> domains) {
        IEnumerable<TEntity> entities = Repository.Add (domains.Select (x => x.AutoMap<TEntity, TDomain> ()));
        return entities.Select (x => x.AutoMap<TDomain, TEntity> ());
    }

    public virtual void Delete (TKey id) {
        Repository.Delete (id);
    }

    public virtual void Delete (IEnumerable<TKey> ids) {
        Repository.Delete (ids);
    }

    public virtual TDomain Update (TDomain domain) {
        TEntity entity = Repository.Update (domain.AutoMap<TEntity, TDomain> ());
        return entity.AutoMap<TDomain, TEntity> ();
    }

    public virtual IEnumerable<TDomain> Update (IEnumerable<TDomain> domains) {
        IEnumerable<TEntity> entities = Repository.Update (domains.Select (x => x.AutoMap<TEntity, TDomain> ()));
        return entities.Select (x => x.AutoMap<TDomain, TEntity> ());
    }
}