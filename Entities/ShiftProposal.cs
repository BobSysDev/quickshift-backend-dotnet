using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ShiftProposal
    {
        
        public long Id { get; set; }
        
        public Shift OriginShift { get; set; }
        public long OriginShiftId { get; set; }
        
        public Employee OriginEmployee { get; set; }
        public long OriginEmployeeId { get; set; }
        
        public Shift TargetShift { get; set; }
        public long TargetShiftId { get; set; }
        
        public Employee TargetEmployee { get; set; }
        public long TargetEmployeeId { get; set; }
    }
}