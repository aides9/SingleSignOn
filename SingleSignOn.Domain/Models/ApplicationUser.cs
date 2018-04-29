using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SingleSignOn.Domain.Models
{
    public class ApplicationUser { 
        public Guid Id { get; set; }

        public string SubjectId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string DisplayName { get; set; }

        public string Role { get; set; }
    }
}
