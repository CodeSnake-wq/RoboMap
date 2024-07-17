using System.Collections.Generic;
using System.Linq;
using robot_controller_api.Models;
using robot_controller_api.Controllers;
using HandlebarsDotNet;

namespace robot_controller_api.Persistence
{
    public class RobotCommandEF : IRobotCommandDataAccess
    {
        private readonly RobotContext _context;

        public RobotCommandEF(RobotContext context)
        {
            _context = context;
        }

        public List<RobotCommand> GetAllRobotCommands()
        {
            return _context.RobotCommands.ToList();
        }

        public List<RobotCommand> GetMoveCommandsOnly()
        {
            return _context.RobotCommands.Where(x => x.IsMoveCommand == true).ToList();
        }

        public RobotCommand GetRobotCommandById(int id)
        {
            return _context.RobotCommands.FirstOrDefault(x => x.Id == id);
        }

        public RobotCommand AddRobotCommand(RobotCommand command)
        {
            command.CreatedDate = DateTime.Now;
            command.ModifiedDate = DateTime.Now;
            _context.RobotCommands.Add(command);
            _context.SaveChanges();

            // Retrieve the map from the database based on its unique properties
            var addedCommand = _context.RobotCommands.FirstOrDefault(m => m.Name == command.Name);

            return addedCommand;

        }

        public RobotCommand UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var existingCommand = GetRobotCommandById(id);

            // Preserve the createdDate from the existing command
            updatedCommand.CreatedDate = existingCommand.CreatedDate;

            // Update the modifiedDate to the current time
            existingCommand.ModifiedDate = DateTime.Now;

            // Update other properties if needed
            existingCommand.Name = updatedCommand.Name;
            existingCommand.Description = updatedCommand.Description;
            existingCommand.IsMoveCommand = updatedCommand.IsMoveCommand;

            // Update the entity in the context
            _context.RobotCommands.Update(existingCommand);
            _context.SaveChanges();

            return GetRobotCommandById(id);
        }

        public int DeleteRobotCommand(int id)
        {
            var command = GetRobotCommandById(id);
            _context.RobotCommands.Remove(command);
            _context.SaveChanges();
            return command.Id;
        }
    }
}
