using System.Drawing;

namespace BehavioralAlgorithms.Models
{
    public class Snake
    {
        public string Taunt { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public int HealthPoints { get; set; }

        public Point[] Coords { get; set; }

        public Point HeadPosition => Coords[0];
    }
}
