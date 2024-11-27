using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ShiftProposal
    {
        public long Id { get; set; }
        public Shift OriginShift { get; set; }
        public Employee OriginEmployee { get; set; }
        public Shift TargetShift { get; set; }
        public Employee TargetEmployee { get; set; }

    }
}