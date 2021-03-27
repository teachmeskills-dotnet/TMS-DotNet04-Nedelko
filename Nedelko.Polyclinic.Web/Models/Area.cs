using System.Collections.Generic;

namespace Nedelko.Polyclinic.Models
{
    public class Area
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int RoomNumber { get; set; }
        
        public List<Doctor> Doctors { get; set; }
    }
}
