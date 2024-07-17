using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace robot_controller_api.Models
{
    //Declaring public class User
    public class User
    {
        //Declaration of all variables for class User
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Description { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Contructor for class User
        public User(int id, string email, string firstName, string lastName, string passwordHash, string? description, string? role, DateTime createdDate, DateTime modifiedDate)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PasswordHash = passwordHash;
            this.Description = description;
            this.Role = role;
            this.CreatedDate = createdDate;
            this.ModifiedDate = modifiedDate;
        }

        // Empty Constructor for class User
        public User() {}
    }
}
