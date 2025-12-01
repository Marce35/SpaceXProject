using System.Text.Json.Serialization;

namespace SpaceXProject.api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Asc,
    Desc
}
