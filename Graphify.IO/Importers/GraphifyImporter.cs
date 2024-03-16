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
        int indexStartItem1 = jsonString.IndexOf("[");
        int indexStartItem2 = jsonString.IndexOf("2\": [") + 4;

        string jsonStringItem1 = jsonString.Substring(indexStartItem1, jsonString.IndexOf("]") - indexStartItem1 + 1);
        string jsonStringItem2 = jsonString.Substring(indexStartItem2, jsonString.LastIndexOf('}') - indexStartItem2);

        _jsonPointObject = JsonConvert.DeserializeObject<List<JsonPointObject>>(jsonStringItem1);
        _jsonFigureObject = JsonConvert.DeserializeObject<List<JsonFigureObject>>(jsonStringItem2);

        AddObjects();

        _jsonPointObject.Clear();
        _jsonFigureObject.Clear();

        return result;
    }

    private void AddObjects()
    {
        foreach (var item in _jsonPointObject)
        {
            _points.Add(new Point(item.Position.X, item.Position.Y, item.Style));
        }

        foreach (var item in _jsonFigureObject)
        {
            switch (item.ObjectType)
            {
                case ObjectType.Circle: AddCircle(item); break;
                case ObjectType.Line: AddLine(item); break;
                case ObjectType.CubicBezier: AddCubicBezire(item); break;
                case ObjectType.Polygon: AddPolygon(item); break;
            }
        }

    }

    private void AddLine(JsonFigureObject item)
    {
        var style = new CurveStyle(item.Style.PrimaryColor, item.Style.Name, (item.Style as CurveStyle ?? CurveStyle.Default).Size);

        //_figures.Add();
    }
    private void AddCircle(JsonFigureObject item) => throw new NotImplementedException();
    private void AddPolygon(JsonFigureObject item) => throw new NotImplementedException();
    private void AddCubicBezire(JsonFigureObject item) => throw new NotImplementedException();

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

            if (styleType != null)
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
}
