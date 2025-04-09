namespace RoyPhishingProj.APILayer
{
    using RoyPhishingProj.BusinessLogicLayer.request;
    using System.Text.Json.Serialization;

    [JsonSerializable(typeof(ResponseData))]
    [JsonSerializable(typeof(PhishingRequest))]
    internal partial class MyJsonContext : JsonSerializerContext
    {
    }
}
