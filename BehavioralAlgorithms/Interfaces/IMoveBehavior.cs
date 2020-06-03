using BehavioralAlgorithms.Models;

namespace BehavioralAlgorithms.Interfaces
{
    public interface IMoveBehavior
    {
        void Init(string gameId, int height, int width);

        MoveDirection Move(MoveState move);
    }

    public interface IMoveBehavior<T> : IMoveBehavior
    {        
    }
}
