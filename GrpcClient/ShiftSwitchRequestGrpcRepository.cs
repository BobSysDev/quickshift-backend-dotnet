namespace GrpcClient;

public class ShiftSwitchRequestGrpcRepository
{
    private string _grpcAddress { get; set; }

    public ShiftSwitchRequestGrpcRepository()
    {
        _grpcAddress = "http://192.168.195.143:50051";
    }
}