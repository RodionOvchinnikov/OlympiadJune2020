using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors
{
    public class StrongMoveBehavior : IBehavior
    {
        public MoveDirection Move(MoveState move)
        {
            if (move?.DeadSnakes?.Any(ds => string.Equals(ds.Id, move.You)) == true)
            {
                return new MoveDirection { Move = "up", Taunt = "No way" };
            }

            return new MoveDirection { Move = "right", Taunt = "!!Kavabanga!!" };
        }
    }
}
