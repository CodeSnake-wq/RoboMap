using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using robot_controller_api.Models;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/robot-commands")]
    public class RobotCommandController : ControllerBase
    {
        private readonly IRobotCommandDataAccess _robotCommandsRepo;
        public RobotCommandController(IRobotCommandDataAccess robotCommandsRepo)
        {
            _robotCommandsRepo = robotCommandsRepo;
        }

        [HttpGet()]
        public IActionResult GetAllRobotCommands()
        {
            var robotCommand = _robotCommandsRepo.GetAllRobotCommands();
            if (robotCommand.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(robotCommand);
            }
        }

        [HttpGet("move")]
        public IActionResult GetMoveCommandsOnly()
        {
            var robotCommand = _robotCommandsRepo.GetMoveCommandsOnly();
            if (robotCommand.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(robotCommand);
            }
        }

        [HttpGet("{id}", Name = "GetRobotCommand")]
        public IActionResult GetRobotCommandById(int id)
        {
            Models.RobotCommand robotCommand = _robotCommandsRepo.GetRobotCommandById(id);
            if (robotCommand == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(robotCommand);
            }
        }

        [HttpPost()]
        public IActionResult AddRobotCommand(Models.RobotCommand newCommand)
        {
            if (newCommand == null) return BadRequest();

            else if (_robotCommandsRepo.GetAllRobotCommands().Any(x => x.Name == newCommand.Name)) return Conflict("Command name already exists!");

            else
            {
                Models.RobotCommand robotCommand = _robotCommandsRepo.AddRobotCommand(newCommand);
                return Created("Map: ", robotCommand);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRobotCommand(int id, Models.RobotCommand updatedCommand)
        {
            if (updatedCommand == null) { return BadRequest("Robot Command is null!"); }

            else if (_robotCommandsRepo.GetRobotCommandById(id) == null) { return NotFound("No robot command found with id: " + id + "!"); }

            else if (_robotCommandsRepo.GetAllRobotCommands().Any(x => x.Name == updatedCommand.Name && x.Id != id)) { return Conflict("Robot command name already exist!"); }

            else
            {
                Models.RobotCommand robotCommand = _robotCommandsRepo.UpdateRobotCommand(id, updatedCommand);
                return Ok(robotCommand);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRobotCommand(int id)
        {
            if (_robotCommandsRepo.GetRobotCommandById(id) == null) { return NotFound("No robot command found with id: " + id + "!"); }

            else
            {
                _robotCommandsRepo.DeleteRobotCommand(id);
                return Ok("Robot Command removed Successfully");
            }
        }
    }
}
