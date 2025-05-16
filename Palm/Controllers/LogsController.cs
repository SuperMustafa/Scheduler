using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Palm.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly SchedulerDbContext _context;

        public LogsController(SchedulerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleExecutionLog>>> GetLogs()
        {
            return await _context.ScheduleExecutionLogs
                .OrderByDescending(l => l.SentAt)
                .ToListAsync();
        }
    
    }
}
