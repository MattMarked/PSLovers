using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSLovers2.Services.Model
{
    public class ServiceOutput<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ServiceOutput() => Success = true;
    }
}
