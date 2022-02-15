using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCaseBinderService.Services
{
    public interface IDraftService
    {
        Task<string> CreateDatabase(string dbName);
        Task<string> CreateCollection(string collectionName);
        Task<bool> CreateLoan(JObject request);
        Task<dynamic> GetLoans(string userName, int start, int limit);
        Task<dynamic> GetLoan(string id);
        Task<bool> UpdateLoan(string id, JObject request);
        Task<Object> DeleteLoan(string id);
        Task<string> DeleteCollection(string collectionName);
        Task<string> DeleteDatabase(string dbName);
    }
}
