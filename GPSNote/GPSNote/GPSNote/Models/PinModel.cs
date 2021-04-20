﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Models
{
    public class PinModel : IEntityModel
    {
        [PrimaryKey, AutoIncrement, Column(nameof(Id))]
        public int Id { get; set; }

        [Column(nameof(UserId))]
        public int UserId { get; set; }

        [Column(nameof(Label)), Unique]
        public string Label { get; set; }

        [Column(nameof(Description))]
        public string Description { get; set; }

        [Column(nameof(Latitude))]
        public double Latitude { get; set; }

        [Column(nameof(Longitude))]
        public double Longitude { get; set; }

        [Column(nameof(IsEnabled))]
        public bool IsEnabled { get; set; }


    }

}
