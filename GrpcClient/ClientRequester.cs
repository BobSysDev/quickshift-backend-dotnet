using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace GrpcClient;

public class ClientRequester
{
    public async Task<string> SendHello(string name)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(new HelloRequest { Name = name});
        Console.WriteLine(reply.Message);
        return reply.Message;
    }
    
    public async Task<List<String>> GetAllHellos()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new Greeter.GreeterClient(channel);
        var reply = await client.GetAllHellosAsync(new Empty()); 
        return reply.Replies.ToList();
    }
}