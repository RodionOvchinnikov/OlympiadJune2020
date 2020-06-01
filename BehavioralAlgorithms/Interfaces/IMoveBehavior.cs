using BehavioralAlgorithms.Models;

namespace BehavioralAlgorithms.Interfaces
{
    public interface IMoveBehavior<T>
    {
        void Init(string gameId, int height, int width);

        MoveDirection Move(MoveState move);
    }
}
