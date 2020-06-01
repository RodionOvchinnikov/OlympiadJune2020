using BehavioralAlgorithms.Models;

namespace BehavioralAlgorithms.Interfaces
{
    public interface IBehavior
    {
        MoveDirection Move(MoveState state);
    }
}
