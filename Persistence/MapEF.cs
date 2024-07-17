using System.Collections.Generic;
using System.Linq;
using robot_controller_api.Models;
using robot_controller_api.Controllers;

namespace robot_controller_api.Persistence
{
    public class MapEF : IMapDataAccess
    {
        private readonly RobotContext _context;

        public MapEF(RobotContext context)
        {
            _context = context;
        }

        public List<Map> GetAllMaps()
        {
            return _context.Maps.ToList();
        }

        public List<Map> GetSquareMapsOnly()
        {
            return _context.Maps.Where(x => x.IsSquare == true).ToList();
        }

        public Map GetMapById(int id)
        {
            return _context.Maps.FirstOrDefault(m => m.Id == id);
        }

        public Map AddRobotMap(Map map)
        {
            map.CreatedDate = DateTime.Now;
            map.ModifiedDate = DateTime.Now;
            _context.Maps.Add(map);
            _context.SaveChanges();

            // Retrieve the map from the database based on its unique properties
            var addedMap = _context.Maps.FirstOrDefault(m => m.Name == map.Name);

            return addedMap;
        }

        public Map UpdateRobotMap(int id, Map updatedMap)
        {
            var existingMap = GetMapById(id);

            // Preserve the createdDate from the existing map
            updatedMap.CreatedDate = existingMap.CreatedDate;

            // Update the modifiedDate to the current time
            existingMap.ModifiedDate = DateTime.Now;

            // Update other properties if needed
            existingMap.Name = updatedMap.Name;
            existingMap.Description = updatedMap.Description;
            existingMap.Columns = updatedMap.Columns;
            existingMap.Rows = updatedMap.Rows;

            // Update the entity in the context
            _context.Maps.Update(existingMap);
            _context.SaveChanges();

            return GetMapById(id);
        }

        public int DeleteRobotMap(int id)
        {
            var map = GetMapById(id);
            _context.Maps.Remove(map);
            _context.SaveChanges();
            return map.Id;
        }

        public bool CheckCoordinate(int id, int x, int y)
        {
            Map map = GetMapById(id);

            if(x < map.Columns && y < map.Rows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}


