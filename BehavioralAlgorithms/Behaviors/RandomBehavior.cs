using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using System;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors
{
    public class RandomBehavior : IBehavior
    {
        public MoveDirection Move(MoveState move)
        {
            if (move?.DeadSnakes?.Any(ds => string.Equals(ds.Id, move.You)) == true)
            {
                return new MoveDirection { Move = "up", Taunt = "No way" };
            }

            var directions = new[] { "up", "left", "down", "right" };

            return new MoveDirection { Move = directions[new Random().Next(0, 3)], Taunt = "!!Kavabanga!!" };
        }
    }
}
