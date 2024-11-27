namespace Entities;

public class ShiftSwitchRequest
{
    public Shift OriginShift { get; set; }
    public Employee OriginEmployee { get; set; }
    public long Id { get; set; }
    public string Details { get; set; }
    public List<ShiftSwitchRequestTimeframe> Timeframes { get; set; }
    public List<ShiftSwitchReply> SwitchReplies { get; set; }
}