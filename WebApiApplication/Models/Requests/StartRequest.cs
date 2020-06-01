using System.Runtime.Serialization;

namespace WebApiApplication.Models.Requests
{
    public class StartRequest
    {
        public int Height { get; set; }

        public int Width { get; set; }

        [DataMember(Name = "game_id")]
        public string GameId { get; set; }
    }
}