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
        /////////////////معني البرامتر ده الجزء الاول ان هو مياخد لمدا اكسبرشن  /////////////
        ////////بتديله اسم الجدول اللي عايزه  Include معني البرامتر ده الجزء التاني انه لو عايز تضيف جمله //////
        ///////////////////    AllowNull عادي هي Include  ولو مفيش   /////////////
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? predacate=null , string? icludeWord = null);

        T GetByID(Expression<Func<T, bool>>? predacate=null, string? icludeWord = null);

        void add(T item);
        void remove(T item);

        void removeRange(List<T> entities);
       
    }
}
