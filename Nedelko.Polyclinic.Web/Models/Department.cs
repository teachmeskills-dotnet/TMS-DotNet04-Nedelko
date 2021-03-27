using System.Collections.Generic;

namespace Nedelko.Polyclinic.Models
{
    public class Department
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        // Doctors FK
        public List<Doctor> Doctors { get; set; }
    }
}
