using Demo_1_Ecommerce.ViewModels;
using System.Linq.Expressions;

namespace Demo_1_Ecommerce.Reposatories
{
    public interface IReviewRepo
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Review GetByID(Expression<Func<Review, bool>> predicate);
        void Remove(Review review);
    }
}
