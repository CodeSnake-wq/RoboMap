using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace robot_controller_api.Models
{
    //Declaring public class RobotCommand
    public class RobotCommand
    {
        //Declaration of all variables for class RobotCommand
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsMoveCommand { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Contructor for class RobotCommand
        public RobotCommand(int id, string name, string? description, bool isMoveCommand, DateTime createdDate, DateTime modifiedDate)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.IsMoveCommand = isMoveCommand;
            this.CreatedDate = createdDate;
            this.ModifiedDate = modifiedDate;
        }

        // Empty Constructor for class RobotCommand
        public RobotCommand() {}
    }
}
