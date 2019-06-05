using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.System.Models
{
    public class CreateUserViewModel
    {
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
        public string HomeEmail { get; set; }
        public string PhoneNumber { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
    }
}
