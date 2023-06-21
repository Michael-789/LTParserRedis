namespace LTParser;

[Serializable]
public class Flight
{
    public int Id { get; set; }
    public int Speed { get; set; }
    public Location? Location { get; set; }
}