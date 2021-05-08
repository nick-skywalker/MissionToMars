using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoverOnMarsApi.Entities;
using RoverOnMarsApi.Enums;
using System.Collections.Generic;

namespace RoverOnMarsTest
{
    [TestClass]
    public class RoverMovementTest
    {
        #region Wrapping Planet

        [TestMethod]
        public void TestWrappingPlanetWithOutObstaclesRightToLeft()
        {
            Rover rover = new Rover("Lolly", new MapPlanet() { Length = 10, Height = 10 });

            rover.SetPosition(10, 1);
            rover.SetDirection(DirectionEnum.Est);

            MovementResult result = rover.MoveForward(new List<Obstacle>());

            Position expeted = new Position()
            {
                X = 1,
                Y = 1
            };

            Assert.IsTrue(
                expeted.X == result.Position.X &&
                expeted.Y == result.Position.Y);
        }


        [TestMethod]
        public void TestWrappingPlanetWithOutObstaclesLeftToRight()
        {
            Rover rover = new Rover("Lolly", new MapPlanet() { Length = 10, Height = 10 });

            rover.SetPosition(1, 1);
            rover.SetDirection(DirectionEnum.Est);

            MovementResult result = rover.MoveBackward(new List<Obstacle>());

            Position expeted = new Position()
            {
                X = 10,
                Y = 1
            };

            Assert.IsTrue(
                expeted.X == result.Position.X &&
                expeted.Y == result.Position.Y);
        }


        [TestMethod]
        public void TestWrappingPlanetWithOutObstaclesNordToSud()
        {
            Rover rover = new Rover("Lolly", new MapPlanet() { Length = 10, Height = 10 });

            rover.SetPosition(5, 10);
            rover.SetDirection(DirectionEnum.Sud);

            MovementResult result = rover.MoveForward(new List<Obstacle>());

            Position expeted = new Position()
            {
                X = 5,
                Y = 1
            };

            Assert.IsTrue(
                expeted.X == result.Position.X &&
                expeted.Y == result.Position.Y);
        }

        [TestMethod]
        public void TestWrappingPlanetWithOutObstaclesSudToNord()
        {
            Rover rover = new Rover("Lolly", new MapPlanet() { Length = 10, Height = 10 });

            rover.SetPosition(5, 1);
            rover.SetDirection(DirectionEnum.Sud);

            MovementResult result = rover.MoveBackward(new List<Obstacle>());

            Position expeted = new Position()
            {
                X = 5,
                Y = 10
            };

            Assert.IsTrue(
                expeted.X == result.Position.X &&
                expeted.Y == result.Position.Y);
        }

        #endregion


        [TestMethod]
        public void TestRoverIncidentObstacle()
        {
            Rover rover = new Rover("Lolly", new MapPlanet() { Length = 10, Height = 10 });

            rover.SetPosition(1, 1);
            rover.SetDirection(DirectionEnum.Est);
            
            Obstacle obstacle = new Obstacle() { Position = new Position(2, 1) };

            MovementResult result = rover.MoveForward(new List<Obstacle>() { obstacle });

            Position expeted = new Position()
            {
                X = 1,
                Y = 1
            };

            Assert.IsTrue(
                expeted.X == result.Position.X &&
                expeted.Y == result.Position.Y &&
                obstacle.Position.X == result.Obstacle.Position.X &&
                obstacle.Position.Y == result.Obstacle.Position.Y &&
                !result.Result);
        }
    }
}
