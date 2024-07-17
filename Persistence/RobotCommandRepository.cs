using robot_controller_api.Persistence;
using robot_controller_api.Models;
using robot_controller_api;
using Npgsql;

namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
    {
        private IRepository _repo => this;
        public List<RobotCommand> GetAllRobotCommands()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM public.robotcommand");
            return commands;
        }

        public List<RobotCommand> GetMoveCommandsOnly()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM public.robotcommand where ismovecommand = true");
            return commands;
        }

        public RobotCommand GetRobotCommandById(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new("id", id) };
            var command = _repo.ExecuteReader<RobotCommand>("SELECT * FROM public.robotcommand where id = @id;", sqlParams).SingleOrDefault();
            return command;
        }

        public RobotCommand AddRobotCommand(RobotCommand newCommand)
        {
            var sqlParams = new NpgsqlParameter[]{
        new("name", newCommand.Name),
        new("description", newCommand.Description ?? (object)DBNull.Value),
        new("ismovecommand", newCommand.IsMoveCommand)
        };

            var result = _repo.ExecuteReader<RobotCommand>("INSERT INTO public.robotcommand (Name, description, ismovecommand, createddate, modifieddate) VALUES (@name, @description, @ismovecommand, current_timestamp, current_timestamp)", sqlParams).Single();
            return result;
        }

        public RobotCommand UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]{
        new("id", id),
        new("name", updatedCommand.Name),
        new("description", updatedCommand.Description ?? (object)DBNull.Value),
        new("ismovecommand", updatedCommand.IsMoveCommand)
        };

            var result = _repo.ExecuteReader<RobotCommand>("UPDATE public.robotcommand SET name=@name, description=@description, ismovecommand = @ismovecommand, modifieddate = current_timestamp WHERE id = @id RETURNING *; ", sqlParams).Single();
            return result;
        }

        public int DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new("id", id) };
            var command = _repo.ExecuteReader<RobotCommand>("DELETE FROM public.robotcommand WHERE id = @Id RETURNING id", sqlParams).Single().Id;
            return command;
        }
    }
}
