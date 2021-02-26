using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Doctors FK
        public List<Doctor> Doctors { get; set; }
    }
}
