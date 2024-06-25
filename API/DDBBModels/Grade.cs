using System;
using System.Collections.Generic;

namespace API.DDBBModels
{
    public partial class Grade
    {
        public int GradeId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public decimal? Grade1 { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Student? Student { get; set; }
    }
}
