using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services.Schedulers;

namespace Palm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulerController(IScheduleService _scheduleService) :ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null) return NotFound();
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleDto dto)
        {
            var schedule = await _scheduleService.CreateScheduleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateScheduleDto dto)
        {
            var updated = await _scheduleService.UpdateScheduleAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _scheduleService.DeleteScheduleAsync(id);
            if (!deleted) return NotFound();
            return NoContent(); // 204
        }
    }
}
