using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Curves;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Graphify.IO.Importers;

public partial class GraphifyImporter
{
    public class StyleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStyle));
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject styleObject = JObject.Load(reader);

            string? styleType = styleObject["LineColor"]?.ToString();

            if (styleType is not null)
            {
                throw new Exception("Unknown Exception. There was styleObject.ToObject<PolygonStyle>(serializer);");
            }

            return styleObject.ToObject<CurveStyle>(serializer);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
