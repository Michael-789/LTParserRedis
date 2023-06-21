namespace LTParser.DTO;

[Serializable]
public class Location
{
    public int Longitude { get; set; }
    public int Latitude { get; set; }
    public int Altitude { get; set; }


    public Location()
    {
        //  
    }

    public Location(int x, int y, int z)
    {
        Longitude = x;
        Latitude = y;
        Altitude = z;
    }
}