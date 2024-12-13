using System.Text.Json.Serialization;

namespace MainProgram.Users
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        None = 0,
        User = 1,
        Admin = 2,
        All = User | Admin
    }
}
