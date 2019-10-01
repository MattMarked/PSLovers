using PSLovers2.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSLovers2.Services
{
    public abstract class GenericService    {

        public void ServiceFailed<T>(ServiceOutput<T> output, Exception exception)
        {
            output.Success = false;
            output.ErrorMessage = exception.ToString();
        }

        public void ServiceFailed<T>(ServiceOutput<T> output, string message)
        {
            output.Success = false;
            output.ErrorMessage = message;
        }

    }
}
