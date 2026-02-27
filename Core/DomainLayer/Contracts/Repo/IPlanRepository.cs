using DomainLayer.Models.PlanModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo
{
    public interface IPlanRepository
    {
        Task<Plan> CreatePlanAsync(Plan plan);

        Task<Plan?> GetPlanByIdAsync(int planId);
    }

}
