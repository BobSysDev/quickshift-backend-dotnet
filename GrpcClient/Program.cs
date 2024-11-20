// using Grpc.Net.Client;
// using GrpcClient;
//

using GrpcClient;
using NewEmployeeDTO = DTOs.NewEmployeeDTO;

Console.WriteLine("Starting the client...");

var repo = new GrpcRepo();

Console.WriteLine(await repo.CreateEmployee(new NewEmployeeDTO
{
    FirstName = "Sam",
    LastName = "Knieza",
    WorkingNumber = 2137,
    Email = "sam.knieza@gmail.com",
    Password = "dupadupa123"
}));
// var client = new ClientRequester();
// Console.WriteLine(await client.SendHello("Dupa"));