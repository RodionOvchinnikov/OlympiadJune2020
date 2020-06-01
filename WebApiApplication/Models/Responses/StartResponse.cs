using System.Text.Json.Serialization;

namespace WebApiApplication.Models.Responses
{
    public class StartResponse
    {
        public string Color { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("head_url")]
        public string HeadUrl { get; set; }

        public string Taunt { get; set; }

        [JsonPropertyName("Head_type")]
        public string HeadType { get; set; }

        [JsonPropertyName("tail_type")]
        public string TailType { get; set; }

        [JsonPropertyName("secondary_color")]
        public string SecondaryColor { get; set; }
    }
}