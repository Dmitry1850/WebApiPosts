using System.Text.Json.Serialization;

namespace MainProgram.Users
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Author = 1,
        Reader = 2
    }
}