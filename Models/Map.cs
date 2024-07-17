using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace robot_controller_api.Models
{
    //Declaring public class Map
    public class Map
    {
        //Declaration of all variables for class Map
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool? IsSquare { get; set; }

        // Contructor for class Map
        public Map(int id, string name, int rows, int columns, string? description, DateTime createdDate, DateTime modifiedDate, bool? isSquare)
        {
            this.Id = id;
            this.Name = name;
            this.Rows = rows;
            this.Columns = columns;
            this.Description = description;
            this.CreatedDate = createdDate;
            this.ModifiedDate = modifiedDate;
            this.IsSquare = isSquare;
        }

        // Empty Constructor for class Map
        public Map() {}
    }
}
