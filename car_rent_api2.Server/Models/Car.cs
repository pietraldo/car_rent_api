﻿using System.ComponentModel.DataAnnotations.Schema;

namespace car_rent_api2.Server
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Photo { get; set; }
        public CarStatus Status { get; set; }
        public double Price { get; set; }
        public Location Location { get; set; }
        public List<CarDetail> Details { get; set; }

        public List<CarService> Services { get; set; }
    }
}
