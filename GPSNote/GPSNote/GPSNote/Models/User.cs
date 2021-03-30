using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GPSNote.Models
{
    [Table("User")]
    public class User : IEntityModel
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Unique, Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

    }
}