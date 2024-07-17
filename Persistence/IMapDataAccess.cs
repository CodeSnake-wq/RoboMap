using robot_controller_api.Controllers;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IMapDataAccess
    {
        Map AddRobotMap(Map newMap);
        bool CheckCoordinate(int id, int x, int y);
        List<Map> GetAllMaps();
        Map GetMapById(int id);
        List<Map> GetSquareMapsOnly();
        Map UpdateRobotMap(int id, Map newMap);
        int DeleteRobotMap(int id);
    }
}