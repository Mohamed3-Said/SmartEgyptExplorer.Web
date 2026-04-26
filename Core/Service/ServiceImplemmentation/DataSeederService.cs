using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceAbstraction.Services;
using DomainLayer.Contracts.Repo;

namespace Service.ServiceImplementation
{
    public class DataSeederService : IDataSeederService
    {
        private readonly IDataSeederRepo _repo;

        public DataSeederService(IDataSeederRepo repo)
        {
            _repo = repo;
        }

        public async Task<string> SeedAllAsync(string csvFolderPath)
        {
            return await _repo.SeedAllAsync(csvFolderPath);
        }
    }
}
