// using DTOs;
// using DTOs.Shift;
// using Grpc.Core;
// using Grpc.Net.Client;
// using RepositoryContracts;
//
// namespace GrpcClient;
//
// public class ShiftSwitchSwitchRequestGrpcRepository : IShiftSwitchRequestRepository
// {
//     private string _grpcAddress { get; set; }
//
//     public ShiftSwitchSwitchRequestGrpcRepository()
//     {
//         _grpcAddress = "http://192.168.195.143:50051";
//     }
//
//
//     public async Task<Entities.ShiftSwitchRequest> AddAsync(Entities.ShiftSwitchRequest request)
//     {
//         using var channel = GrpcChannel.ForAddress(_grpcAddress);
//         var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
//         var reply = await client.AddRequestAsync(new NewRequestDTO
//         {
//             OriginEmployeeId = request.OriginEmployee.Id,
//             OriginShiftId = request.OriginShift.Id,
//             Details = request.Details
//         });
//
//         return new Entities.ShiftSwitchRequest
//         {
//             Id = reply.Id,
//             OriginEmployee = new Entities.Employee { Id = reply.OriginEmployeeId },
//             OriginShift = new Entities.Shift { Id = reply.OriginShiftId },
//             Details = reply.Details
//         };
//     }
//
//     public async Task<Entities.ShiftSwitchRequest> UpdateAsync(Entities.ShiftSwitchRequest request)
//     {
//         try
//         {
//             using var channel = GrpcChannel.ForAddress(_grpcAddress);
//             var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
//             var update = new ShiftSwitchRequestDTO
//             {
//                 Id = request.Id,
//                 OriginShiftId = request.OriginShift.Id,
//                 
//             };
//
//             var reply = await client.UpdateRequestAsync(update);
//             return new Entities.ShiftSwitchRequest
//             {
//                 Id = reply.Id,
//                 OriginEmployee = new Entities.Employee { Id = reply.OriginEmployeeId },
//                 OriginShift = new Entities.Shift { Id = reply.OriginShiftId },
//                 Details = reply.Details,
//             };
//         }
//         catch (RpcException e)
//         {
//             if (e.StatusCode == StatusCode.NotFound)
//             {
//                 throw new ArgumentException(e.Message + ": " + request.Id, nameof(request.Id));
//             }
//
//             throw;
//         }
//     }
//
//     public async Task DeleteAsync(long id)
//     {
//         using var channel = GrpcChannel.ForAddress(_grpcAddress);
//         var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
//         var request = new Id { Id_ = id };
//
//         try
//         {
//             await client.DeleteRequestAsync(request);
//         }
//         catch (RpcException e)
//         {
//             if (e.StatusCode == StatusCode.NotFound)
//             {
//                 throw new ArgumentException(e.Message + ": " + id, nameof(id));
//             }
//
//             throw;
//         }
//     }
//
//     public IQueryable<Entities.ShiftSwitchRequest> GetManyAsync()
//     {
//         using var channel = GrpcChannel.ForAddress(_grpcAddress);
//         var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
//         List<ShiftSwitchRequestDTO> shiftSwitchRequestDtos = client.
//     }
//
//     public Task<Entities.ShiftSwitchRequest> GetSingleAsync(long id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<bool> IsRequestInRepository(long id)
//     {
//         try
//         {
//             using var channel = GrpcChannel.ForAddress(_grpcAddress);
//             var client = new ShiftSwitchRequest.ShiftSwitchRequestClient(channel);
//             var reply = await client.
//         }
//     }
//
//     public Task<List<Entities.ShiftSwitchRequest>> GetByEmployeeAsync(long employeeId)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<List<Entities.ShiftSwitchRequest>> GetByShiftAsync(long shiftId)
//     {
//         throw new NotImplementedException();
//     }
//     
//     public static Entities.ShiftSwitchRequest ShiftSwitchRequestToEntity(ShiftSwitchRequestDTO shiftSwitchRequestDto)
//     {
//         return new Entities.ShiftSwitchRequest
//         {
//             Id = shiftSwitchRequestDto.Id,
//             OriginEmployee = new Entities.Employee { Id = shiftSwitchRequestDto.RequesterId },
//             OriginShift = new Entities.Shift { Id = shiftSwitchRequestDto.OriginShiftId },
//             Details = shiftSwitchRequestDto.Details
//         };
//     }
//     }
