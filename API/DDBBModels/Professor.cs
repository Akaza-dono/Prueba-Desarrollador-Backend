using System;
using System.Collections.Generic;

namespace API.DDBBModels
{
    public partial class Professor
    {
        public Professor()
        {
            Classes = new HashSet<Class>();
        }

        public int ProfessorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Specialty { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
