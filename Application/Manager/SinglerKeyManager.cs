using Application.Extension;
using Application.Manager.Interface;
using Application.Repository.Interface;

namespace Application.Manager;
public class SinglerKeyManager<TIRepository, TDomain, TEntity, TKey> : IManager<TDomain, TKey>
    where TDomain : class, new ()
where TEntity : class, new ()
where TIRepository : IRepository<TDomain,TEntity, TKey> {
    protected TIRepository Repository { get; set; }

    public virtual IEnumerable<TDomain> GetAll () {
        return Repository.GetAll ();
    }

    public virtual TDomain? GetById (TKey id) {
        return Repository.FindById (id);
    }

    public virtual TDomain Add (TDomain domain) {
        TDomain result = Repository.Add (domain);
        return result;
    }

    public virtual IEnumerable<TDomain> Add (IEnumerable<TDomain> domains) {
        IEnumerable<TDomain> result = Repository.Add (domains);
        return result;
    }

    public virtual void Delete (TKey id) {
        Repository.Delete (id);
    }

    public virtual void Delete (IEnumerable<TKey> ids) {
        Repository.Delete (ids);
    }

    public virtual TDomain Update (TDomain domain) {
        TDomain result = Repository.Update (domain);
        return result;
    }

    public virtual IEnumerable<TDomain> Update (IEnumerable<TDomain> domains) {
        IEnumerable<TDomain> result = Repository.Update (domains);
        return result;
    }
}