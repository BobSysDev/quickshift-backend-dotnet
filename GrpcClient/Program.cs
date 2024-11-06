using Grpc.Net.Client;
using GrpcClient;

Console.WriteLine("Starting the client...");

var client = new ClientRequester();
Console.WriteLine(await client.SendHello("Dupa"));