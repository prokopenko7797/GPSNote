using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GPSNote.Models
{
    [Table(nameof(User))]
    public class User : IEntityModel
    {
        [PrimaryKey, AutoIncrement, Column(nameof(id))]
        public int id { get; set; }

        [Column(nameof(Name))]
        public string Name { get; set; }

        [Unique, Column(nameof(Email))]
        public string Email { get; set; }

        [Column(nameof(Password))]
        public string Password { get; set; }

    }
}