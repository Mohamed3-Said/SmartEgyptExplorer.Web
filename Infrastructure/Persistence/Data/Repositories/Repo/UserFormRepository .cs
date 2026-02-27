using DomainLayer.Contracts.Repo;
using DomainLayer.Models.Remaining_Modules;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo
{
    public class UserFormRepository(SmartEgyptDbContext _context) : IUserFormRepository
    {
        public async Task<UserFormSubmission> AddSubmissionAsync(UserFormSubmission submission)
        {
            // 1. شيل الـ Answers من الأوبجكت مؤقتاً عشان نشوف الـ Submission لوحدها هتسيف ولا لأ
            var tempAnswers = submission.UserAnswers.ToList();
            submission.UserAnswers = null!;

            _context.UserFormSubmissions.Add(submission);

            // لو ضربت هنا يبقى الـ UserId هو اللي غلط 100%
            await _context.SaveChangesAsync();

            // 2. لو عدت، نرجع الـ Answers ونربطهم بالـ ID اللي لسه متكريت
            foreach (var ans in tempAnswers)
            {
                ans.UserFormSubmissionId = submission.UserFormSubmissionId;
                _context.UserAnswers.Add(ans);
            }

            await _context.SaveChangesAsync();
            return submission;
        }

        //public async Task<UserFormSubmission> AddSubmissionAsync(UserFormSubmission submission)
        //{
        //    _context.UserFormSubmissions.Add(submission);
        //    await _context.SaveChangesAsync();
        //    return submission;
        //}

        public async Task AddAnswersAsync(IEnumerable<UserAnswer> answers)
        {
            _context.UserAnswers.AddRange(answers);
            await _context.SaveChangesAsync();
        }
       
        public async Task<UserFormSubmission?> GetSubmissionWithAnswersAsync(int submissionId)
        {
            return await _context.UserFormSubmissions
                .Include(s=>s.UserAnswers)
                .FirstOrDefaultAsync(s => s.UserFormSubmissionId == submissionId);
        }
    }
}
