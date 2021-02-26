using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
