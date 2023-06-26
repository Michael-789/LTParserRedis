using NetTopologySuite.Geometries;

namespace LTParser.DTO;

[Serializable]
public class Flight
{
    public int Id { get; set; }
    public int Speed { get; set; }
   // [Newtonsoft.Json.JsonConverter(typeof(NetTopologySuite.IO.Converters.GeometryConverter))]
    public Point? Location { get; set; }
    public double Altitude { get; set; }
}