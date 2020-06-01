namespace BehavioralAlgorithms.Models
{
    public class MoveState
    {
        public Point[] Food { get; set; }

        public string GameId { get; set; }

        public Snake[] Snakes { get; set; }

        public Snake[] DeadSnakes { get; set; }

        public int Turn { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string You { get; set; }
    }
}
