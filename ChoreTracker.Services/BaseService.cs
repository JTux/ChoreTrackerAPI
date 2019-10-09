using ChoreTracker.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services
{
    public abstract class BaseService
    {
        protected RequestResponse OkResponse(string message) => new RequestResponse { Succeeded = true, Message = message };
        protected RequestResponse BadResponse(string message) => new RequestResponse { Succeeded = false, Message = message };
    }
}
