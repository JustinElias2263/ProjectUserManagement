using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UsersChangeLog
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
       
        public string HRID { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } 

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public string UserName { get; set; }

        public string HomeEmail { get; set; }
        public bool HomeEmailConfirmed { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
