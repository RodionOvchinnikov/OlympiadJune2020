using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using BehavioralAlgorithms.Behaviors.Hamiltonian;

namespace BehavioralAlgorithms.Behaviors
{
    public class HamiltonianBehavior : IBehavior
    {
        public MoveDirection Move(MoveState state)
        {
            var direction = HamiltonianEngine.Move(state);

            return new MoveDirection {Move = direction, Taunt = $"Moving {direction}" };
        }
    }
}
