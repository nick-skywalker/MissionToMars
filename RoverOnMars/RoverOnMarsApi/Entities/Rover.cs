using System.Collections.Generic;
using System.Linq;
using RoverOnMarsApi.Enums;

namespace RoverOnMarsApi.Entities
{
    public class Rover
    {
        #region Private properties

        private MapPlanet PlanetGrid { get; }

        #endregion

        #region Public properties

        public string Name { get; internal set; }
        public Position Position { get; internal set; }
        public DirectionEnum Direction { get; internal set; }

        #endregion

        public Rover(string name, MapPlanet planetGrid)
        {
            Name = name;
            PlanetGrid = planetGrid;
            Position = new Position();
        }

        #region Private methods

        /// <summary>
        /// Get new position checking wrapping planet
        /// </summary>
        /// <param name="position">position desire</param>
        /// <returns></returns>
        private Position GetNewPositionCheckWrapping(Position position)
        {
            Position result = new Position();

            int xMax = PlanetGrid.Length;
            int yMax = PlanetGrid.Height;

            //wrapping planet
            result.X = position.X > xMax ?
                        1 :
                        position.X < 1 ?
                            xMax :
                            position.X;

            result.Y = position.Y > yMax ?
                        1 :
                        position.Y < 1 ?
                            yMax :
                            position.Y;

            return result;
        }

        /// <summary>
        /// Check if in coordinate exist an obstacle
        /// </summary>
        /// <param name="position">Coordinate</param>
        /// <param name="obstacles">List of coordinate of obstacle</param>
        /// <returns>Eventual obstacle</returns>
        private Obstacle GetPossibleObstacle(Position position, List<Obstacle> obstacles)
        {
            return obstacles
                        .FirstOrDefault(o => o.Position.X == position.X &&
                                             o.Position.Y == position.Y);
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Setter position
        /// </summary>
        /// <param name="xPosition">Coordinate X</param>
        /// <param name="yPosition">Coordinate Y</param>
        public void SetPosition(int xPosition, int yPosition)
        {
            int xMax = PlanetGrid.Length;
            int yMax = PlanetGrid.Height;

            //wrapping planet
            Position = GetNewPositionCheckWrapping(new Position(xPosition, yPosition));
        }

        /// <summary>
        /// Setter of direction
        /// </summary>
        /// <param name="direction">Enum direction: N,S,W,E</param>
        public void SetDirection(DirectionEnum direction)
        {
            Direction = direction;
        }

        /// <summary>
        /// Method that move forward the rover
        /// </summary>
        /// <param name="obstacles">List of coordinate of obstacles</param>
        /// <returns>Movement result with eventual obstacle</returns>
        public MovementResult MoveForward(List<Obstacle> obstacles)
        {
            Obstacle obstacle = null;
            int xPosition = Position.X, yPosition = Position.Y;
            Position newPosition = Position;

            switch (Direction)
            {
                case DirectionEnum.Nord:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition, yPosition - 1));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        yPosition -= 1;
                    }
                    break;
                case DirectionEnum.Est:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition + 1, yPosition));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        xPosition += 1;
                    }
                    break;
                case DirectionEnum.Sud:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition, yPosition + 1));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        yPosition += 1;
                    }
                    break;
                case DirectionEnum.West:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition - 1, yPosition));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        xPosition -= 1;
                    }
                    break;
            }

            if (obstacle != null)
            {
                return new MovementResult()
                {
                    Obstacle = obstacle,
                    Position = this.Position,
                    Result = false,
                    Direction = this.Direction
                };
            }
            else
            {
                SetPosition(xPosition, yPosition);

                return new MovementResult()
                {
                    Obstacle = null,
                    Position = this.Position,
                    Result = true,
                    Direction = this.Direction
                };
            }
        }

        /// <summary>
        /// Method that move backward rover
        /// </summary>
        /// <param name="obstacles">List of coordinate of obstacles</param>
        /// <returns>Movement result with eventual obstacle</returns>
        public MovementResult MoveBackward(List<Obstacle> obstacles)
        {
            Obstacle obstacle = null;
            int xPosition = Position.X, yPosition = Position.Y;

            Position newPosition = Position;

            switch (Direction)
            {
                case DirectionEnum.Nord:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition, yPosition + 1));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        yPosition += 1;
                    }
                    break;
                case DirectionEnum.Est:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition - 1, yPosition));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        xPosition -= 1;
                    }
                    break;
                case DirectionEnum.Sud:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition, yPosition - 1));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        yPosition -= 1;
                    }
                    break;
                case DirectionEnum.West:
                    //generate next position
                    newPosition =
                        GetNewPositionCheckWrapping(
                            new Position(xPosition + 1, yPosition));

                    //check any incident
                    obstacle = GetPossibleObstacle(newPosition, obstacles);

                    //if there isn't obstacle
                    if (obstacle == null)
                    {
                        xPosition += 1;
                    }
                    break;
            }

            if (obstacle != null)
            {
                return new MovementResult()
                {
                    Obstacle = obstacle,
                    Position = this.Position,
                    Result = false,
                    Direction = this.Direction
                };
            }
            else
            {
                SetPosition(xPosition, yPosition);

                return new MovementResult()
                {
                    Obstacle = null,
                    Position = this.Position,
                    Result = true,
                    Direction = this.Direction
                };
            }

        }

        /// <summary>
        /// Method that turn left rover
        /// </summary>
        public void TurnLeft()
        {
            switch (Direction)
            {
                case DirectionEnum.Nord:
                    SetDirection(DirectionEnum.West);
                    break;
                case DirectionEnum.Est:
                    SetDirection(DirectionEnum.Nord);
                    break;
                case DirectionEnum.Sud:
                    SetDirection(DirectionEnum.Est);
                    break;
                case DirectionEnum.West:
                    SetDirection(DirectionEnum.Sud);
                    break;
            }
        }

        /// <summary>
        /// Method that turn right rover
        /// </summary>
        public void TurnRight()
        {
            switch (Direction)
            {
                case DirectionEnum.Nord:
                    SetDirection(DirectionEnum.Est);
                    break;
                case DirectionEnum.Est:
                    SetDirection(DirectionEnum.Sud);
                    break;
                case DirectionEnum.Sud:
                    SetDirection(DirectionEnum.West);
                    break;
                case DirectionEnum.West:
                    SetDirection(DirectionEnum.Nord);
                    break;
            }
        }

        #endregion
    }
}
