using Demo_1_Ecommerce.Reposatories;
using Microsoft.EntityFrameworkCore;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.Data;
using System.Linq.Expressions;

namespace Demo_1_Ecommerce.Implementation
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await GetReviewByIdAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
        public Review GetByID(Expression<Func<Review, bool>> predicate)
        {
            return _context.Reviews.FirstOrDefault(predicate); // Adjust if necessary
        }

        public void Remove(Review review)
        {
            _context.Reviews.Remove(review);
        }
    }

}
