using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
{
    public class PatientAddress
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        // Patient FK
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
