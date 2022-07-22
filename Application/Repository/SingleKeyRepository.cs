using System.Globalization;
using System.Linq.Expressions;
using Application.Extension;
using Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Repository;
public class SingleKeyRepository<TDomain, TEntity, TKey> : IRepository<TDomain, TEntity, TKey>
    where TDomain : class, new ()
where TKey :  IComparable<TKey>
where TEntity : class, new () {
    protected DbContext? DbContext { get; set; }
    protected IDbContextTransaction? Transaction { get; set; }
    public virtual TDomain Add (TDomain domain) {
        TEntity entity = ToEntity (domain);
        DbContext.Set<TEntity> ().Add (entity);
        DbContext.SaveChanges ();
        return ToDomain (entity);
    }

    public virtual IEnumerable<TDomain> Add (IEnumerable<TDomain> domains) {
        IEnumerable<TEntity> entities = domains.Select (x => ToEntity (x));
        DbContext.Set<TEntity> ().AddRange (entities);
        DbContext.SaveChanges ();
        return entities.Select (x => ToDomain (x));
    }

    public virtual void Delete (TKey id) {
        TEntity? entity = FindEntityById (id);
        if (entity == null) {
            throw new Exception ($"{typeof(TEntity).Name} {id} Not Found!");
        }
        DbContext.Set<TEntity> ().Remove (entity);
        DbContext.SaveChanges ();
        return;
    }

    public virtual void Delete (IEnumerable<TKey> ids) {
        IEnumerable<TEntity> entities = ids.Select (id => {
            TEntity? entity = FindEntityById (id);
            if (entity == null) {
                throw new Exception ($"{typeof(TEntity).Name} {id} Not Found!");
            } else {
                return entity;
            }
        });
        DbContext.Set<TEntity> ().RemoveRange (entities);
        DbContext.SaveChanges ();
        return;
    }

    public virtual TDomain Update (TDomain domain) {
        TEntity entity = ToEntity (domain);
        TKey id = FindPrimaryKeyValue (entity);
        TEntity exist = FindEntityById (id);
        if (exist == null) {
            throw new Exception ($"{typeof(TEntity).Name} {id} Not Found!");
        }
        DbContext.ChangeTracker.Clear ();
        DbContext.Set<TEntity> ().Update (ToEntity (domain));
        DbContext.SaveChanges ();
        return domain;
    }

    public virtual IEnumerable<TDomain> Update (IEnumerable<TDomain> domains) {
        IEnumerable<TEntity> entities = domains.Select (x => ToEntity (x));
        foreach (TEntity entity in entities) {
            TKey id = FindPrimaryKeyValue (entity);
            TEntity exist = FindEntityById (id);
            if (exist == null) {
                throw new Exception ($"{typeof(TEntity).Name} {id} Not Found!");
            }
        }
        DbContext.ChangeTracker.Clear ();
        DbContext.Set<TEntity> ().UpdateRange (entities);
        DbContext.SaveChanges ();
        return domains;
    }

    public virtual IEnumerable<TDomain> GetAll () {
        return DbContext.Set<TEntity> ().AsNoTracking ().AsEnumerable ().Select (x => ToDomain (x));
    }

    public virtual TDomain? FindById (TKey id) {
        var entity = FindEntityById (id);
        return entity == null?null : ToDomain (entity);
    }

    private IReadOnlyList<IProperty> FindPrimaryKeys () {
        IEntityType entityType = DbContext.Model.FindEntityType (typeof (TEntity));
        return entityType.FindPrimaryKey ().Properties;
    }

    private TKey FindPrimaryKeyValue (TEntity entity) {
        var PrimaryKeys = FindPrimaryKeys ();

        string primaryKeyName = PrimaryKeys.Select (p => p.Name).FirstOrDefault ();
        Type primaryKeyType = PrimaryKeys.Select (p => p.ClrType).FirstOrDefault ();
        var tmp = typeof (TEntity).GetProperties ().Where (x => x.Name == primaryKeyName).Where (x => x.GetType () == primaryKeyType);

        System.Reflection.PropertyInfo? keyProperty = typeof (TEntity).GetProperties ().Where (x => x.Name == primaryKeyName).Where (x => x.PropertyType == primaryKeyType).SingleOrDefault ();
        if (keyProperty == null) {
            throw new Exception ($"{typeof(TEntity).Name} Has No Key!");
        }
        return (TKey) keyProperty.GetValue (entity);

    }

    public virtual TEntity? FindEntityById (TKey id) {
        IEntityType entityType = DbContext.Model.FindEntityType (typeof (TEntity));

        object primayKeyValue = null;

        IReadOnlyList<IProperty> ProimaryKeys = FindPrimaryKeys ();

        string primaryKeyName = ProimaryKeys.Select (p => p.Name).FirstOrDefault ();
        Type primaryKeyType = ProimaryKeys.Select (p => p.ClrType).FirstOrDefault ();

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

    public virtual IEnumerable<TDomain> Paginition (IQueryable<TEntity> query, int? page, int? pageSize) {
        if (page == -1 && pageSize == -1) {
            return query.AsEnumerable ().Select (x => ToDomain (x));
        } else {
            int realPage = page ?? 1;
            int realPageSize = pageSize ?? 15;
            return query.Skip ((realPage - 1) * realPageSize).Take (realPageSize).AsEnumerable ().Select (x => ToDomain (x));
        }
    }

    public virtual TEntity ToEntity (TDomain domain) {
        return domain.AutoMap<TEntity, TDomain> ();
    }

    public virtual TDomain ToDomain (TEntity entity) {
        return entity.AutoMap<TDomain, TEntity> ();
    }
}