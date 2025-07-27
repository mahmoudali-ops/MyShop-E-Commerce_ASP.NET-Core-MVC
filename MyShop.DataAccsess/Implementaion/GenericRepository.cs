using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Repositories;

namespace MyShop.DataAccsess.Implementaion
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet=_context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate, string? includeword)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null) 
            {
                query=query.Where(predicate);
            }
            if (includeword != null) 
            {
                foreach (var item in includeword.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries)) 
                {
                    query=query.Include(item);  
                }
            }

            return query.ToList();
        }

        public T GetFirstorDefault(Expression<Func<T, bool>>? predicate=null, string? includeword = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includeword != null)
            {
                foreach (var item in includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return query.SingleOrDefault() ;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
