using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient;

public class EmployeeGrpcRepository : IEmployeeRepository
{
    private string _grpcAddress { get; set; }

    public EmployeeGrpcRepository(string grpcAddress)
    {
        _grpcAddress = grpcAddress;
    }

    public async Task<Entities.Employee> AddAsync(Entities.Employee employee)
    {
        using var channel = GrpcChannel.ForAddress(_grpcAddress);
        var client = new Employee.EmployeeClient(channel);
        try
        {
            var reply = await client.AddSingleEmployeeAsync(GrpcDtoConverter.EmployeeToGrpcNewEmployeeDto(employee));
            Entities.Employee employeeReceived = GrpcDtoConverter.GrpcEmployeeDtoToEmployee(reply);
            
            return employeeReceived;
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Employee already exists: {employee.Id}", nameof(employee.Id));
            }

            throw new Exception("An error occurred while adding the employee.", e);
        }
    }

    public async Task<Entities.Employee> UpdateAsync(Entities.Employee employee)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.UpdateSingleEmployeeAsync(GrpcDtoConverter.EmployeeToGrpcUpdateEmployeeDto(employee));

            return GrpcDtoConverter.GrpcEmployeeDtoToEmployee(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Employee not found: {employee.Id}", nameof(employee.Id));
            }
            else if (e.StatusCode == StatusCode.AlreadyExists)
            {
                throw new ArgumentException($"Employee already exists: {employee.Id}", nameof(employee.Id));
            }

            throw new Exception("An error occurred while updating the employee.", e);
        }
    }

    public async Task DeleteAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            await client.DeleteSingleEmployeeAsync(new Id { Id_ = id });
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Employee not found: {id}", nameof(id));
            }

            throw new Exception("An error occurred while deleting the employee.", e);
        }
    }

    public IQueryable<Entities.Employee> GetManyAsync()
    {
        
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = client.GetAllEmployees(new Empty());
            var employees = GrpcDtoConverter.GrpcEmployeeDtoListToEntityEmployeeList(reply);

            return employees.AsQueryable();
        }
        catch (RpcException e)
        {
            throw new Exception("An error occurred while retrieving employees.", e);
        }
    }

    public async Task<Entities.Employee> GetSingleAsync(long id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.GetSingleEmployeeByIdAsync(new Id { Id_ = id });

            return GrpcDtoConverter.GrpcEmployeeDtoToEmployee(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException($"Employee not found: {id}", nameof(id));
            }

            throw new Exception("An error occurred while retrieving the employee.", e);
        }
    }

    public async Task<Entities.Employee> GetSingleEmployeeByWorkingNumberAsync(int WorkingNumber)
    {
        Entities.Employee employee = new Entities.Employee();
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress); 
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.GetSingleEmployeeByWorkingNumberAsync(new WorkingNumber { WorkingNumber_ = uint.CreateChecked(WorkingNumber)  }); 
            employee = GrpcDtoConverter.GrpcEmployeeDtoToEmployee(reply);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new ArgumentException(e.Message);
            }
        }

        return employee;
    }

    public async Task<bool> IsEmployeeInRepository(long Id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new Employee.EmployeeClient(channel);
            var reply = await client.IsEmployeeInRepositoryAsync(new Id { Id_ = Id });
            bool b = reply.Boolean_;
            return b;
        }
        catch(RpcException e)
        {
            throw new Exception("An error occurred while checking if the employee is in the repository.", e);
        }
    }
}