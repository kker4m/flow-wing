using FlowWing.Business.Abstract;
using FlowWing.DataAccess.Abstract;
using FlowWing.DataAccess.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Concrete
{
    public class LoggingManager:ILoggingService
    {
        private ILoggingRepository _loggingRepository;

        public LoggingManager(ILoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }

        public async Task CreateLogAsync(string message)
        {
            await _loggingRepository.CreateLogAsync(message);
        }

    }
}
