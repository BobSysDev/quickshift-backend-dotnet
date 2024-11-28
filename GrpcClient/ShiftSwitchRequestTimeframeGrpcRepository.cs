namespace GrpcClient;

public class ShiftSwitchRequestTimeframeGrpcRepository
{
    private string _grpcAddress { get; set; }

    public ShiftSwitchRequestTimeframeGrpcRepository()
    {
        _grpcAddress = "http://192.168.140.143:50051";
    }
}