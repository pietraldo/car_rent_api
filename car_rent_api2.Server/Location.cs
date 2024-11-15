﻿namespace car_rent_api2.Server
{
    public class Location
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Address { get; set; } = "";
        public string Name { get; set; } = "";
    }
}