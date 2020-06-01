using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors
{
    public class MoveBehavior<T> : IMoveBehavior<T>
        where T : IBehavior, new()
    {
        private readonly T _behavior;

        private int _height;
        private int _width;
        private string _gameId;

        public MoveBehavior()
        {
            _behavior = new T();
        }

        public void Init(string gameId, int height, int width)
        {
            _height = height;
            _width = width;
            _gameId = gameId;
        }

        public MoveDirection Move(MoveState move)
        {
            return _behavior.Move(move);
        }
    }
}
