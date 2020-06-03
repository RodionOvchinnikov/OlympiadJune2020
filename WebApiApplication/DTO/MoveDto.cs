using BehavioralAlgorithms.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebApiApplication.DTO
{
    public class MoveDto
    {
        public int[][] Food { get; set; }

        [JsonPropertyName("game_id")]
        public string GameId { get; set; }

        public SnakeDto[] Snakes { get; set; }

        [JsonPropertyName("dead_snakes")]
        public SnakeDto[] DeadSnakes { get; set; }

        public int Turn { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string You { get; set; }

        public static MoveState MapFromDto(MoveDto data)
        {
            return new MoveState
            {
                GameId = data.GameId,
                Height = data.Height,
                Width = data.Width,
                Turn = data.Turn,
                You = data.You,
                Food = data.Food?.Select(f => new Point {X = f[0], Y = f[1] })?.ToArray(),
                Snakes = SnakeDto.MapFromDto(data.Snakes),
                DeadSnakes = SnakeDto.MapFromDto(data.DeadSnakes)
            };
        }
    }
}
