using System;
using System.Collections.Generic;
using RoverOnMarsApi.Swagger.Model;

namespace RoverOnMarsClient
{
    class Program
    {
        private static List<Obstacle> obstacles;
        private static MapPlanet mapPlanet;
        private static RoverOnMarsApi.Swagger.Api.RoverApi roverApi;

        private const string missionToMars = @"
#############################################
############ MISSION TO MARS #################
#############################################
";

        static void Main(string[] args)
        {
            roverApi = new RoverOnMarsApi.Swagger.Api.RoverApi("http://localhost:5000");

            //Load map and obstacles that rover had elaborated
            obstacles = roverApi.RoverGetObstaclesGet();
            mapPlanet = roverApi.RoverGetMapPlanetGet();

            //Initialize rover position and direction 
            Position currentPosition = new Position(1, 1);
            DirectionEnum currentDirection = DirectionEnum.Est;
            
            //Draw map with rover in current position and obstacles
            DrawUpdatedMap(currentPosition.X.Value, currentPosition.Y.Value, currentDirection);

            while (true)
            {
                Console.WriteLine("q = exit - Commands: f = forward, b = backward, l = left, r = right. es. f,f,l,r,b");
                //wait the command series
                string command = Console.ReadLine();

                if (command == "q")
                {
                    break;
                }
                else
                {
                    //move rover from remote
                    var movement = roverApi.RoverMoveRoverPost(
                        currentPosition,
                        command,
                        currentDirection);

                    currentPosition = movement.Position;
                    currentDirection = movement.Direction;

                    //update map
                    DrawUpdatedMap(movement.Position.X.Value, movement.Position.Y.Value, currentDirection);

                    if (movement.Obstacle != null)
                    {
                        Console.WriteLine($"The rover was stopped by obstacle in coordinate [{movement.Obstacle.Position.X},{movement.Obstacle.Position.Y}]. Press Enter to be continue.");
                        Console.ReadLine();
                    }
                    else if (!movement.Result.Value)
                    {
                        Console.WriteLine("Command not supported. Press Enter to be continue.");
                        Console.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Method that return an arrow equals to cardinal direction
        /// </summary>
        /// <param name="direction">Cardinal direction (N,S,W,E)</param>
        /// <returns>Arrow string</returns>
        private static string GetDirectionCharacter(DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Nord:
                    return "↑";
                case DirectionEnum.Sud:
                    return "↓";
                case DirectionEnum.Est:
                    return "→";
                case DirectionEnum.West:
                    return "←";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Method that draw map with obstacles and rover in current position and direction
        /// </summary>
        /// <param name="x">Current rover X coordinate</param>
        /// <param name="y">Current rover Y coordinate</param>
        /// <param name="roverDirection">Current rover direction</param>
        private static void DrawUpdatedMap(int x, int y, DirectionEnum roverDirection)
        {
            Console.Clear();

            Console.Write(missionToMars);

            string direction = GetDirectionCharacter(roverDirection);

            for (int j = 1; j <= mapPlanet.Height; ++j)
            {
                Console.WriteLine();

                for (int i = 1; i <= mapPlanet.Length; ++i)
                {
                    if (x == i && y == j)
                    {
                        Console.Write($"|R{direction}_");
                    }
                    else if (obstacles.Contains(new Obstacle(new Position(i, j))))
                    {
                        Console.Write($"|_O_");
                    }
                    else
                    {
                        Console.Write("|___");
                    }

                    if (i == mapPlanet.Length)
                    {
                        Console.Write("|");
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
