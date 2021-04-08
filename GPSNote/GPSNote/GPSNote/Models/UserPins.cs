using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Models
{
    public class UserPins : IEntityModel
    {
        [PrimaryKey, AutoIncrement, Column(nameof(id))]
        public int id { get; set; }


        [Column(nameof(user_id))]
        public int user_id { get; set; }

        [Column(nameof(Latitude))]
        public double Latitude { get; set; }

        [Column(nameof(Longitude))]
        public double Longitude { get; set; }

        [Column(nameof(Label))]
        public string Label { get; set; }

        [Column(nameof(Description))]
        public string Description { get; set; }

        [Column(nameof(IsEnabled))]
        public bool IsEnabled { get; set; }


    }

}
