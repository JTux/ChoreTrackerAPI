using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.ResponseModels
{
    public class ModelRequestResponse<T> : RequestResponse
    {
        public T Model { get; set; }
    }
}
