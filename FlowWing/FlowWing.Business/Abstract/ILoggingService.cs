using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Abstract
{
    public interface ILoggingService
    {
        Task CreateLogAsync(string message);
    }
}
