using DomainLayer.Contracts.Repo;
using DomainLayer.Models.PlanModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo
{
    public class PlanRepository(SmartEgyptDbContext _context) : IPlanRepository
    {
        public async Task<Plan> CreatePlanAsync(Plan plan)
        {
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }
        public async Task<Plan?> GetPlanByIdAsync(int planId)
        {
            return await _context.Plans
               .Include(p => p.PlanDays)
                   .ThenInclude(d => d.Hotel)
                       .ThenInclude(h => h.Place)
               .Include(p => p.PlanDays)
                   .ThenInclude(d => d.Meals)
               .Include(p => p.PlanDays)
                   .ThenInclude(d => d.PlanActivities)
               .Include(p => p.BudgetBreakdown)
               .FirstOrDefaultAsync(p => p.PlanId == planId);
        }
        public async Task<bool> DeletePlanAsync(int planId, string userId)
        {
            var plan = await _context.Plans
                .Include(p => p.PlanDays)
                .ThenInclude(d => d.PlanActivities)
                .FirstOrDefaultAsync(p => p.PlanId == planId && p.UserId == userId);

            if (plan == null) return false;

            // EF Core هياخد باله من الـ Cascade Delete لو مضبوط في الـ DB Context
            // لو مش مضبوط، هتمسح الـ Activities والـ Days الأول
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Plan?> GetCurrentPlanAsync(string userId)
        {
            return await _context.Plans
                .AsNoTracking()
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.Hotel)
                        .ThenInclude(h => h.Place) // 🏨 مهم جدًا
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.Meals) // 🍽️ مهم جدًا
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.PlanActivities)
                        .ThenInclude(a => a.Place)
                .Include(p => p.BudgetBreakdown)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();
        }

        // 1. قائمة كل الخطط (مختصرة عشان السرعة)
        public async Task<IEnumerable<Plan>> GetUserPlansHistoryAsync(string userId)
        {
            return await _context.Plans
                .AsNoTracking()
                .Include(p => p.BudgetBreakdown)        // ✅ زود ده
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.Meals)
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.PlanActivities)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // 2. تفاصيل خطة معينة بالـ ID
        public async Task<Plan?> GetPlanByIdAsync(int planId, string userId)
        {
            return await _context.Plans
                .AsNoTracking()
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.Hotel)
                        .ThenInclude(h => h.Place)
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.Meals)
                .Include(p => p.PlanDays)
                    .ThenInclude(d => d.PlanActivities)
                        .ThenInclude(a => a.Place)
                 .Include(p => p.BudgetBreakdown)
                .FirstOrDefaultAsync(p => p.PlanId == planId && p.UserId == userId);
        }
    }
}
