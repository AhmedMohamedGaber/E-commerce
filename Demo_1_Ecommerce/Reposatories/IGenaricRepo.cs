using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Reposatories
{
    public interface IGenaricRepo<T> where T : class
    {
       
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? predacate=null , string? icludeWord = null);

        T GetByID(Expression<Func<T, bool>>? predacate=null, string? icludeWord = null);

        void add(T item);
        void remove(T item);

        void removeRange(List<T> entities);
       
    }
}
