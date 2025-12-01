using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.Models.SpaceXApi.Core;
    public class SpaceXQueryOptions
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("sort")]
        public object Sort { get; set; } = new { };

        [JsonPropertyName("populate")]
        public object[] Populate { get; set; } = [];
    }
