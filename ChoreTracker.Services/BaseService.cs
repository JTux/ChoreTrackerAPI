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
        protected static Random RandomGenerator = new Random();
        protected RequestResponse OkResponse(string message) => new RequestResponse { Succeeded = true, Message = message };
        protected RequestResponse BadResponse(string message) => new RequestResponse { Succeeded = false, Message = message };

        protected ModelRequestResponse<T> OkModelResponse<T>(string message, T model) => new ModelRequestResponse<T> { Succeeded = true, Message = message, Model = model };
        protected ModelRequestResponse<T> BadModelResponse<T>(string message, T model) => new ModelRequestResponse<T> { Succeeded = false, Message = message, Model = model };
    }
}
