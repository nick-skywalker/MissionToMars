using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using RoverOnMarsApi.Entities;
using RoverOnMarsApi.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RoverOnMarsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoverController : Controller
    {
        private readonly ILogger<RoverController> _logger;
        private readonly Rover _rover;
        private List<Obstacle> _obstacles;
        private MapPlanet _mapPlanet;

        /// <summary>
        /// Rover costructor
        /// </summary>
        /// <param name="logger">logger object</param>
        public RoverController(ILogger<RoverController> logger)
        {
            _logger = logger;

            LoadObstacles();
            LoadMapPlanet();
            _rover = new Rover("Spirit", _mapPlanet);
        }

        #region Private methods

        /// <summary>
        /// Validator of commands
        /// </summary>
        /// <param name="commands">Commands to validate</param>
        /// <returns>if valid true, else false</returns>
        private bool ValidateCommands(string commands)
        {
            if (string.IsNullOrEmpty(commands))
            {
                return false;
            }

            //Check if all characters are allowed
            char[] charactersAllowed = new char[] { 'l', 'r', 'f', 'b', ',' };
            foreach (char character in commands)
            {
                if (!charactersAllowed.Contains(character))
                {
                    return false;
                }
            }

            //Check if there are commands more big then 1 character
            if (commands.Split(',').Any(c => c.Length > 1))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loader of obstacles (example, by image recognition that write json file)
        /// </summary>
        private void LoadObstacles()
        {
            try
            {
                _obstacles = JsonConvert.DeserializeObject<List<Obstacle>>(System.IO.File.ReadAllText("Data/obstacles.json"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _obstacles = new List<Obstacle>();
            }
        }

        /// <summary>
        /// Load the map of planet (example, by image recognition that write json file)
        /// </summary>
        private void LoadMapPlanet()
        {
            try
            {
                _mapPlanet = JsonConvert.DeserializeObject<MapPlanet>(System.IO.File.ReadAllText("Data/mapPlanet.json"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                _mapPlanet = new MapPlanet() { Height = 10, Length = 10 };
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get the planet map
        /// </summary>
        /// <returns>Matrix of integers</returns>
        [HttpGet]
        [Route("GetMapPlanet")]
        public MapPlanet GetMapPlanet()
        {
            return _mapPlanet;
        }

        /// <summary>
        /// Get the obstacles
        /// </summary>
        /// <returns>List of obstacles</returns>
        [HttpGet]
        [Route("GetObstacles")]
        public List<Obstacle> GetObstacles()
        {
            return _obstacles;
        }

        /// <summary>
        /// Move the rover with a command series
        /// </summary>
        /// <param name="commands">string of commands (es. f,b,l,r)</param>
        /// <returns>Movement result 
        /// (result of movement, 
        /// eventual obstacle, 
        /// current position, 
        /// current direction)</returns>
        [HttpPost]
        [Route("MoveRover")]
        public MovementResult MoveRover(string commands, Position actualPosition, DirectionEnum actualDirection)
        {
            if (!ValidateCommands(commands))
            {
                _logger.LogError("Invalid commands.");
                return new MovementResult()
                {
                    Result = false,
                    Obstacle = null,
                    Position = actualPosition,
                    Direction = actualDirection
                };
            }

            _rover.SetPosition(actualPosition.X, actualPosition.Y);
            _rover.SetDirection(actualDirection);

            var arrCommands = commands.Split(',');

            foreach (var command in arrCommands)
            {
                //if empty command then continue without do anything
                if (string.IsNullOrEmpty(command.Trim()))
                {
                    continue;
                }

                MovementResult result = null;

                switch (command.Trim())
                {
                    case "l":
                        _rover.TurnLeft();
                        break;
                    case "r":
                        _rover.TurnRight();
                        break;
                    case "f":
                        result = _rover.MoveForward(_obstacles);
                        break;
                    case "b":
                        result = _rover.MoveBackward(_obstacles);
                        break;
                }

                //if movement result is negative becouse possible incident,
                //return result with obstacle 
                if (result != null && !result.Result)
                {
                    return result;
                }
            }

            //good movement
            return new MovementResult()
            {
                Result = true,
                Obstacle = null,
                Direction = _rover.Direction,
                Position = _rover.Position
            };
        }

        #endregion
    }
}
