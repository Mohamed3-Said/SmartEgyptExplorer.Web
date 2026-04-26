using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services
{
    public interface IDataSeederService
    {
        Task<string> SeedAllAsync(string csvFolderPath);
    }
}
