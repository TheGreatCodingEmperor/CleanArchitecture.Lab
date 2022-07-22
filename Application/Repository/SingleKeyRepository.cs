using System.Globalization;
using System.Linq.Expressions;
using Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Application.Extension;

namespace Application.Repository;
public class SingleKeyRepository<TDomain,TEntity, TKey> : IRepository<TDomain,TEntity,TKey>
    where TDomain : class, new ()
    where TEntity : class, new (){
    protected DbContext? DbContext {get;set;}
    protected IDbContextTransaction? Transaction { get; set; }
    public virtual TDomain Add (TDomain domain) {
        TEntity entity = ToEntity(domain);
        DbContext.Set<TEntity> ().Add (entity);
        DbContext.SaveChanges ();
        return ToDomain(entity);
    }

    public virtual IEnumerable<TDomain> Add (IEnumerable<TDomain> domains) {
        IEnumerable<TEntity> entities = domains.Select(x => ToEntity(x));
        DbContext.Set<TEntity> ().AddRange (entities);
        DbContext.SaveChanges ();
        return entities.Select(x => ToDomain(x));
    }

    public virtual void Delete (TKey id) {
        TEntity? entity = FindEntityById (id);
        DbContext.Set<TEntity> ().Remove (entity);
        DbContext.SaveChanges ();
        return;
    }

    public virtual void Delete (IEnumerable<TKey> ids) {
        IEnumerable<TEntity> entities = ids.Select (id => FindEntityById (id));
        DbContext.Set<TEntity> ().RemoveRange (entities);
        DbContext.SaveChanges ();
        return;
    }

    public virtual TDomain Update (TDomain domain) {
        DbContext.ChangeTracker.Clear();
        DbContext.Set<TEntity> ().Update (ToEntity(domain));
        DbContext.SaveChanges ();
        return domain;
    }

    public virtual IEnumerable<TDomain> Update (IEnumerable<TDomain> domains) {
        DbContext.ChangeTracker.Clear();
        DbContext.Set<TEntity> ().UpdateRange (domains.Select(x => ToEntity(x)));
        DbContext.SaveChanges ();
        return domains;
    }

    public virtual IEnumerable<TDomain> GetAll(){
        return DbContext.Set<TEntity>().AsNoTracking().AsEnumerable().Select(x => ToDomain(x));
    }

    public virtual TDomain? FindById (TKey id) {
        var entity = FindEntityById(id);
        return entity==null?null:ToDomain(entity);
    }

    public virtual TEntity? FindEntityById (TKey id) {
        IEntityType entityType = DbContext.Model.FindEntityType (typeof (TEntity));

        object primayKeyValue = null;

        string primaryKeyName = entityType.FindPrimaryKey ().Properties.Select (p => p.Name).FirstOrDefault ();
        Type primaryKeyType = entityType.FindPrimaryKey ().Properties.Select (p => p.ClrType).FirstOrDefault ();

        try {
            primayKeyValue = Convert.ChangeType (id, primaryKeyType, CultureInfo.InvariantCulture);
        } catch (Exception) {
            throw new ArgumentException ($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
        }

        ParameterExpression pe = Expression.Parameter (typeof (TEntity), "entity");
        MemberExpression me = Expression.Property (pe, primaryKeyName);
        ConstantExpression constant = Expression.Constant (primayKeyValue, primaryKeyType);
        BinaryExpression body = Expression.Equal (me, constant);
        Expression<Func<TEntity, bool>> expressionTree = Expression.Lambda<Func<TEntity, bool>> (body, new [] { pe });

        IQueryable<TEntity> query = DbContext.Set<TEntity> ();
        TEntity enity = query.FirstOrDefault (expressionTree);
        return enity;
    }

    public virtual void BeginTransaction () {
        Transaction = DbContext.Database.BeginTransaction ();
    }

    public virtual void Commit () {
        if (Transaction != null) {
            Transaction.Commit ();
        }
    }

    public virtual void RollBack () {
        if (Transaction != null) {
            Transaction.Rollback ();
        }
    }

    public virtual IEnumerable<TDomain> Paginition(IQueryable<TEntity> query, int? page,int? pageSize)
    {
        if (page == -1 && pageSize == -1)
        {
            return query.AsEnumerable().Select(x => ToDomain(x));
        }
        else
        {
            int realPage = page ?? 1;
            int realPageSize = pageSize ?? 15;
            return query.Skip((realPage - 1) * realPageSize).Take(realPageSize).AsEnumerable().Select(x => ToDomain(x));
        }
    }

    public virtual TEntity ToEntity(TDomain domain){
        return domain.AutoMap<TEntity,TDomain>();
    }

    public virtual TDomain ToDomain(TEntity entity){
        return entity.AutoMap<TDomain,TEntity>();
    }
}