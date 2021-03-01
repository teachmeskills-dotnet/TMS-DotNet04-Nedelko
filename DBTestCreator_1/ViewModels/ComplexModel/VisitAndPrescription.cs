using DBTestCreator_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels.ComplexModel
{
    public class VisitAndPrescription
    {
        public VisitModel Visit { get; set; }
        public PrescriptionModel prescription { get; set; }
    }
}
