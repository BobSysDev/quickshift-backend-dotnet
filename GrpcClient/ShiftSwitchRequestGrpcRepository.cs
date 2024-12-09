using DTOs;
using DTOs.Shift;
using DTOs.ShiftSwitching;
using Grpc.Core;
using Grpc.Net.Client;
using RepositoryContracts;

namespace GrpcClient
{
    public class ShiftSwitchSwitchRequestGrpcRepository : IShiftSwitchRequestRepository
    {
        private string _grpcAddress { get; set; }
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ShiftSwitchSwitchRequestGrpcRepository(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
        {
            _grpcAddress = "http://192.168.195.143:50051";
            _shiftRepository = shiftRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Entities.ShiftSwitchRequest> AddAsync(Entities.ShiftSwitchRequest request)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(_grpcAddress);
                var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);

                var reply = await client.AddRequestAsync(new NewRequestDTO
                {
                    OriginEmployeeId = request.OriginEmployee.Id,
                    OriginShiftId = request.OriginShift.Id,
                    Details = request.Details
                });
                
                Entities.ShiftSwitchRequest shiftSwitchRequestReceived =
                    GrpcDtoConverter.GrpcRequestDtoToShiftSwitchRequest(reply);
                return shiftSwitchRequestReceived;
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.AlreadyExists)
                {
                    throw new ArgumentException($"Shift switch request already exists: {request.Id}", nameof(request.Id));
                }

                throw;
            }
        }

        public async Task<Entities.ShiftSwitchRequest> UpdateAsync(Entities.ShiftSwitchRequest request)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(_grpcAddress);
                var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
                
                var reply = await client.UpdateRequestAsync(new UpdateRequestDTO
                {
                    Details = request.Details
                });
             
                return  GrpcDtoConverter.GrpcRequestDtoToShiftSwitchRequest(reply);
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.NotFound)
                {
                    throw new ArgumentException(e.Message + ": " + request.Id, nameof(request.Id));
                }

                throw;
            }
        }

        public async Task DeleteAsync(long id)
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
            var request = new Id { Id_ = id };

            try
            {
                await client.DeleteRequestAsync(request);
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.NotFound)
                {
                    throw new ArgumentException(e.Message + ": " + id, nameof(id));
                }

                throw;
            }
        }

        public IQueryable<Entities.ShiftSwitchRequest> GetManyAsync()
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
            var response = client.GetAll(new Empty());
            var shiftSwitchRequests =
                response.Dtos.Select(GrpcDtoConverter.GrpcRequestDtoToShiftSwitchRequest);
            return shiftSwitchRequests.AsQueryable();
        }

        public async Task<Entities.ShiftSwitchRequest> GetSingleAsync(long id)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(_grpcAddress);
                var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
                var request = new Id { Id_ = id };
                var response = await client.GetSingleByIdAsync(request);
                return GrpcDtoConverter.GrpcRequestDtoToShiftSwitchRequest(response);
            } catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.NotFound)
                {
                    throw new ArgumentException(e.Message + ": " + id, nameof(id));
                }

                throw;
            }
        }
        
        public async Task<List<Entities.ShiftSwitchRequest>> GetByEmployeeAsync(long employeeId)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(_grpcAddress);
                var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
                var response = await client.GetRequestsByOriginEmployeeIdAsync(new Id { Id_ = employeeId });
                return response.Dtos.Select(dto => GrpcDtoConverter.GrpcRequestDtoToShiftSwitchRequest(dto)).ToList();
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.NotFound)
                {
                    throw new ArgumentException(e.Message + ": " + employeeId, nameof(employeeId));
                }

                throw;
            }
        }

        public async Task<List<Entities.ShiftSwitchRequest>> GetByShiftAsync(long shiftId)
        {
            using var channel = GrpcChannel.ForAddress(_grpcAddress);
            var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
            var response = await client.GetRequestsByOriginShiftIdAsync(new Id { Id_ = shiftId });
            return response.Dtos.Select(dto => GrpcDtoConverter.GrpcRequestDtoToShiftSwitchRequest(dto)).ToList();
        }
    }
}