using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using robot_controller_api.Models;
using System.Linq;
using System.Windows.Input;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/maps")]
    public class MapController : ControllerBase
    {

        private readonly IMapDataAccess _mapRepo;
        public MapController(IMapDataAccess mapRepo)
        {
            _mapRepo = mapRepo;
        }

        [HttpGet()]
        public IActionResult GetAllMaps()
        {
            var map = _mapRepo.GetAllMaps();
            if (map.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(map);
            }
        }

        [HttpGet("square")]
        public IActionResult GetSquareMapsOnly()
        {
            var map = _mapRepo.GetSquareMapsOnly();
            if (map.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(map);
            }
        }

        [HttpGet("{id}", Name = "GetMap")]
        public IActionResult GetMapById(int id)
        {
            Models.Map map = _mapRepo.GetMapById(id);
            if(map == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(map);
            }
        }

        [HttpPost()]
        public IActionResult AddRobotMap(Models.Map newMap)
        {
            if (newMap == null) return BadRequest();

            else if(_mapRepo.GetAllMaps().Any(x => x.Name == newMap.Name)) return Conflict("Map name already exists!");

            else
            {
                Models.Map map = _mapRepo.AddRobotMap(newMap);
                return Created("Map: ", map);
            }            
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRobotMap(int id, Models.Map updatedMap)
        {
            if (updatedMap == null) { return BadRequest("Map is null!"); }

            else if(_mapRepo.GetMapById(id) == null) { return NotFound("No map found with id: "+id+"!"); }

            else if (_mapRepo.GetAllMaps().Any(x => x.Name == updatedMap.Name && x.Id != id)) { return Conflict("Map name already exist!"); }

            else
            {
                Models.Map map = _mapRepo.UpdateRobotMap(id, updatedMap);
                return Ok(map);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRobotMap(int id)
        {
            if (_mapRepo.GetMapById(id) == null) { return NotFound("No map found with id: " + id + "!"); }

            else
            {
                _mapRepo.DeleteRobotMap(id);
                return Ok("Map removed Successfully");
            }
        }

        [HttpGet("{id}/{x}-{y}")]
        public IActionResult CheckCoordinate(int id, int x, int y)
        {
            // return NotFound() if the map does not exist
            if (_mapRepo.GetMapById(id) == null) { return NotFound("No map found with id: " + id + "!"); }

            // return BadRequest() if coordinate provided is in the wrong format, e.g.negative values
            else if (x < 0) return BadRequest("Coordinates can't be negative!");

            else
            {          
                return Ok(_mapRepo.CheckCoordinate(id, x, y));
            }
        }

    }
}
