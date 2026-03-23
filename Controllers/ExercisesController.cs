using Microsoft.AspNetCore.Mvc;

namespace WorkoutApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private static List<Exercise> _exercises = new List<Exercise>
        {
            new Exercise { Id = 1, Name = "Жим лежачи", TargetMuscle = "Груди", WeightKg = 60 },
            new Exercise { Id = 2, Name = "Присідання", TargetMuscle = "Ноги", WeightKg = 80 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Exercise>> GetAll()
        {
            return Ok(_exercises);
        }

        [HttpGet("{id}")]
        public ActionResult<Exercise> GetById(int id)
        {
            var exercise = _exercises.FirstOrDefault(e => e.Id == id);
            if (exercise == null) return NotFound(new { Message = "Вправу не знайдено" });
            
            return Ok(exercise);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Exercise newExercise)
        {
            if (newExercise == null) return BadRequest();

            newExercise.Id = _exercises.Any() ? _exercises.Max(e => e.Id) + 1 : 1;
            _exercises.Add(newExercise);

            return CreatedAtAction(nameof(GetById), new { id = newExercise.Id }, newExercise);
        }

        [HttpPatch("{id}")]
        public ActionResult Patch(int id, [FromBody] Exercise updateInfo)
        {
            var exercise = _exercises.FirstOrDefault(e => e.Id == id);
            if (exercise == null) return NotFound();

            if (!string.IsNullOrEmpty(updateInfo.Name)) exercise.Name = updateInfo.Name;
            if (!string.IsNullOrEmpty(updateInfo.TargetMuscle)) exercise.TargetMuscle = updateInfo.TargetMuscle;
            if (updateInfo.WeightKg > 0) exercise.WeightKg = updateInfo.WeightKg;

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var exercise = _exercises.FirstOrDefault(e => e.Id == id);
            if (exercise == null) return NotFound();

            _exercises.Remove(exercise);
            return NoContent(); 
        }
    }


    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TargetMuscle { get; set; } = string.Empty;
        public int WeightKg { get; set; }
    }
}