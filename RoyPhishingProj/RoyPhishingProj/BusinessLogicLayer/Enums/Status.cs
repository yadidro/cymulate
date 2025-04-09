using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace RoyPhishingProj.BusinessLogicLayer.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "Created")]
        Created,

        [EnumMember(Value = "Clicked")]
        Clicked,

        [EnumMember(Value = "Failed")]
        Failed
    }
}
