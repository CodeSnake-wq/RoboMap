using robot_controller_api.Controllers;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IRobotCommandDataAccess
    {
        RobotCommand AddRobotCommand(RobotCommand newRobotCommand);
        int DeleteRobotCommand(int id);
        List<RobotCommand> GetAllRobotCommands();
        List<RobotCommand> GetMoveCommandsOnly();
        RobotCommand GetRobotCommandById(int id);
        RobotCommand UpdateRobotCommand(int id, RobotCommand newRobotCommand);
    }
}