using RoverOnMarsApi.Enums;

namespace RoverOnMarsApi.Entities
{
    public class MovementResult
    {
        public Obstacle Obstacle { get; set; }

        public Position Position { get; set; }

        public DirectionEnum Direction { get; set; }

        public bool Result { get; set; }
    }
}
