using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brimma.LOSService.Services
{
    public interface IBorrowerService
    {
        Task<Object> GetBorrowerContacts(Object request,int start, int limit);
    }
}
