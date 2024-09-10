using Core.DataAccess.Abstract;
using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess.Concrete;

public class BaseRepository<TEntiy, TContext>(TContext context) : IBaseRepository<TEntiy>
    where TEntiy : class, IEntity, new()
    where TContext : DbContext, new()
{
    private readonly TContext _context = context;

    public void Add(TEntiy entity)
    {
        var addedEntity = _context.Entry(entity);
        addedEntity.State = EntityState.Added;
        _context.SaveChanges();
    }

    public void Delete(TEntiy entity)
    {
        var deletedEntity = _context.Entry(entity);
        deletedEntity.State = EntityState.Modified;
        _context.SaveChanges();

    }

    public TEntiy Get(Expression<Func<TEntiy, bool>> filter)
    {
        return _context.Set<TEntiy>().SingleOrDefault(filter);
    }

    public List<TEntiy> GetAll(Expression<Func<TEntiy, bool>> filter = null)
    {
        return filter != null ? _context.Set<TEntiy>().Where(filter).ToList() : _context.Set<TEntiy>().ToList();
    }

    public void Update(TEntiy entity)
    {
        var modifyEntity = _context.Entry(entity);
        modifyEntity.State = EntityState.Modified;
        _context.SaveChanges();
    }
}
