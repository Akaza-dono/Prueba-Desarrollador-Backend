using System;
using System.Collections.Generic;

namespace API.DDBBModels
{
    public partial class Class
    {
        public Class()
        {
            Grades = new HashSet<Grade>();
        }

        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? Year { get; set; }
        public string? Semester { get; set; }
        public int? ProfessorId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Professor? Professor { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
