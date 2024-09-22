using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.Reposatories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Implementation
{
    public class GenaricRepo<T> : IGenaricRepo<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        private ApplicationDbContext context;

        public GenaricRepo(ApplicationDbContext context)
        {
            _context = context;
            //الجدول بتاعه Context يجيب من ال T يعني لما يعرف ايه نوع ال 
            _dbSet = _context.Set<T>();
        }

        //public GenaricRepo(ApplicationDbContext context)
        //{
        //    this.context = context;
        //}

        public void add(T item)
        {
            _dbSet.Add(item);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predacate= null, string? icludeWord = null)
        {
            IQueryable<T> query = _dbSet;
            if (predacate != null)
            {
                query = query.Where(predacate);
            }
            if (icludeWord != null)
            {
                foreach (var item in icludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetByID(Expression<Func<T, bool>>? predacate = null, string? icludeWord = null)
        {
            IQueryable<T> query = _dbSet;
            if (predacate != null)
            {
                query = query.Where(predacate);
            }
            if (icludeWord != null)
            {
                foreach (var item in icludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
        }

       

        public void remove(T item)
        {
            _dbSet.Remove(item);
        }

        public void removeRange(List<T> entities)
        {
           _dbSet.RemoveRange(entities);
        }
    }
}
