using Npgsql;
using robot_controller_api;
using robot_controller_api.Persistence;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapRepository : IMapDataAccess, IRepository
    {
        private IRepository _repo => this;
        public List<Map> GetAllMaps()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM public.map");
            return maps;
        }

        public List<Map> GetSquareMapsOnly()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM public.map where issquare = true");
            return maps;
        }

        public  Map GetMapById(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new("id", id) };
            var map = _repo.ExecuteReader<Map>("SELECT * FROM public.map where id = @id;", sqlParams).SingleOrDefault();
            return map;
        }

        public Map AddRobotMap(Map newMap)
        {
            var sqlParams = new NpgsqlParameter[]{
            new("columns", newMap.Columns),
            new("rows", newMap.Rows),
            new("name", newMap.Name),
            new("description", newMap.Description ?? (object)DBNull.Value)
            };

            var result = _repo.ExecuteReader<Map>("INSERT INTO public.map (columns, rows, name, description, createddate, modifieddate) VALUES (@columns, @rows, @Name, @description, current_timestamp, current_timestamp)", sqlParams).Single();
            return result;
        }

        public Map UpdateRobotMap(int id, Map updatedMap)
        {
            var sqlParams = new NpgsqlParameter[]{
            new("id", id),
            new("columns", updatedMap.Columns),
            new("rows", updatedMap.Rows),
            new("name", updatedMap.Name),
            new("description", updatedMap.Description ?? (object)DBNull.Value),
            };

            var result = _repo.ExecuteReader<Map>("UPDATE public.map SET columns = @columns, rows = @rows, Name = @name, description = @description, modifieddate = current_timestamp WHERE id = @id RETURNING *; ", sqlParams).Single();
            return result;
        }

        public int DeleteRobotMap(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new("id", id) };
            var command = _repo.ExecuteReader<Map>("DELETE FROM public.map WHERE id = @Id RETURNING id", sqlParams).Single().Id;
            return command;
        }

        public bool CheckCoordinate(int id, int x, int y)
        {
            Map map = GetMapById(id);
            if (x < map.Columns && y < map.Rows)
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
