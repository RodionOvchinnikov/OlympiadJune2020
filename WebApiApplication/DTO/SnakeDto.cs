using BehavioralAlgorithms.Models;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebApiApplication.DTO
{
    public class SnakeDto
    {
        public string Taunt { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        [JsonPropertyName("health_points")]
        public int HealthPoints { get; set; }

        public int[][] Coords { get; set; }

        public static Snake[] MapFromDto(SnakeDto[] data)
        {
            return data?.Select(s =>
            new Snake
            {
                HealthPoints = s.HealthPoints,
                Id = s.Id,
                Name = s.Name,
                Taunt = s.Taunt,
                Coords = s.Coords?.Select(c => new Point {X = c[0], Y = c[1] })?.ToArray(),
            })?.ToArray();
        }
    }
}
