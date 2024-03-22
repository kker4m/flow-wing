using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Concrete
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly FlowWingDbContext _dbContext;

        public LoggingRepository(FlowWingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateLogAsync(string message)
        {
            _dbContext.Logs.Add(new Log
            {
                Message = message,
                LogTime = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();

        }
    }
}
