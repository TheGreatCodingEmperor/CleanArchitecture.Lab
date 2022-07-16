using System.Globalization;
using System.Linq.Expressions;
using Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Repository;
public class SingleKeyRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, new (){
    protected DbContext? DbContext {get;set;}
    protected IDbContextTransaction? Transaction { get; set; }
    public TEntity Add (TEntity entity) {
        DbContext.Set<TEntity> ().Add (entity);
        DbContext.SaveChanges ();
        return entity;
    }

    public IEnumerable<TEntity> Add (IEnumerable<TEntity> entities) {
        DbContext.Set<TEntity> ().AddRange ();
        DbContext.SaveChanges ();
        return entities;
    }

    public void Delete (TKey id) {
        TEntity? entity = FindById (id);
        DbContext.Remove (entity);
        DbContext.SaveChanges ();
        return;
    }

    public void Delete (IEnumerable<TKey> ids) {
        IEnumerable<TEntity> entities = ids.Select (id => FindById (id));
        DbContext.RemoveRange (entities);
        DbContext.SaveChanges ();
        return;
    }

    public TEntity Update (TEntity entity) {
        DbContext.Update (entity);
        DbContext.SaveChanges ();
        return entity;
    }

    public IEnumerable<TEntity> Update (IEnumerable<TEntity> entities) {
        DbContext.UpdateRange (entities);
        DbContext.SaveChanges ();
        return entities;
    }

    public IEnumerable<TEntity> GetAll(){
        return DbContext.Set<TEntity>().AsEnumerable();
    }

    public TEntity? FindById (TKey id) {
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

    public void BeginTransaction () {
        Transaction = DbContext.Database.BeginTransaction ();
    }

    public void Commit () {
        if (Transaction != null) {
            Transaction.Commit ();
        }
    }

    public void RollBack () {
        if (Transaction != null) {
            Transaction.Rollback ();
        }
    }
}