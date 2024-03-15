using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Polygons;
using Graphify.IO.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Graphify.IO.Importers;

public class GraphifyImporter : IImporter
{
    private List<JsonPointObject> _jsonPointObject = [];

    private List<JsonFigureObject> _jsonFigureObject = [];

    private List<Point> _points = [];

    private List<IFigure> _figures = [];

    public ImportResult ImportFrom(string path)
    {
        ImportResult result = new ImportResult();   

        string jsonString = File.ReadAllText(path);

        string jsonStringItem1 = jsonString.Substring(14,jsonString.IndexOf("],")-13);

        string b = jsonString.Replace(jsonStringItem1,"");
        string jsonStringItem2 = b.Substring(28,jsonStringItem1.IndexOf("]")+62);
        
        _jsonPointObject = JsonConvert.DeserializeObject<List<JsonPointObject>>(jsonStringItem1);

        _jsonFigureObject = JsonConvert.DeserializeObject<List<JsonFigureObject>>(jsonStringItem2);


        _jsonPointObject.Clear();
        _jsonFigureObject.Clear();

        return result;
    }

public class StyleConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(IStyle));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject styleObject = JObject.Load(reader);

        string styleType = (string)styleObject["LineColor"];

        if(styleType != null)
        {
            return styleObject.ToObject<PolygonStyle>(serializer);
        }

        styleType = (string)styleObject["Variant"];

        if (styleType != null)
        {
            return styleObject.ToObject<PointStyle>(serializer);
        }

        return styleObject.ToObject<CurveStyle>(serializer);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
    public override bool CanWrite => false;
}
