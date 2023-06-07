﻿using System;
[Serializable]
public class Location
{
    public int Longitude { get; set; }
    public int Latitude { get; set; }
    public int Elevation{ get; set; }


    public Location()
	{
	}

    public Location(int x, int y, int z)
    {
        this.Longitude = x;
        this.Latitude = y;
        this.Elevation = z;
    }
}