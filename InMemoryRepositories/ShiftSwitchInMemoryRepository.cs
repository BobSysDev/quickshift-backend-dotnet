using Entities;
using RepositoryContracts;

namespace InMemoryRepositories
{
    public class ShiftSwitchInMemoryRepository : IShiftSwitchRepository
    {
        private readonly List<ShiftSwitchRequest> _requests;

        public ShiftSwitchInMemoryRepository()
        {
            _requests = new List<ShiftSwitchRequest>();
        }
        

        public async Task<ShiftSwitchRequest> AddShiftSwitchRequestAsync(ShiftSwitchRequest request)
        {
            
                _requests.Add(request);
                return request;
            
        }

        public async Task<ShiftSwitchRequest> UpdateShiftSwitchRequestAsync(ShiftSwitchRequest request)
        {
           var existingRequest = _requests.SingleOrDefault(r => r.Id == request.Id);

                if (existingRequest == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {request.Id} not found.");

                _requests.Remove(existingRequest);
                _requests.Add(request);
                return request;
            
        }


        public async Task DeleteShiftSwitchRequestAsync(long id)
        {
            
                var requestToRemove = _requests.SingleOrDefault(r => r.Id == id);

                if (requestToRemove == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {id} not found.");

                _requests.Remove(requestToRemove);
           
        }
        
        public IQueryable<ShiftSwitchRequest> GetManyShiftSwitchRequestAsync()
        {
            return _requests.AsQueryable();
        }
        
        public async Task<ShiftSwitchRequest> GetSingleShiftSwitchRequestAsync(long id)
        {
            
                var request = _requests.SingleOrDefault(r => r.Id == id);

                if (request == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {id} not found.");

                return request;
           
        }

        public async Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByEmployeeIdAsync(long employeeId)
        {
           
                var request = _requests.Where(r => r.OriginEmployee.Id == employeeId).ToList();
                return await Task.FromResult(request);
            
        }

        public async Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByShiftIdAsync(long shiftId)
        {
            
                var request = _requests.Where(r => r.OriginShift.Id == shiftId).ToList();
                return await Task.FromResult(request);
           
        }

        public async Task<List<ShiftSwitchRequest>> GetShiftSwitchRequestByEmployeeAsync(long employeeId)
        {
           
                return _requests.Where(r => r.OriginEmployee.Id == employeeId).ToList();
            
        }

        public async Task<List<ShiftSwitchRequest>> GetShiftSwitchRequestByShiftAsync(long shiftId)
        {
           
                return _requests.Where(r => r.OriginShift.Id == shiftId).ToList();
           
        }
        
        public async Task<ShiftSwitchReply> AddShiftSwitchReplyAsync(ShiftSwitchReply reply, long requestId)
        {
            
                var request = _requests.SingleOrDefault(r => r.Id == requestId);

                if (request == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {requestId} not found.");

                request.SwitchReplies.Add(reply);

                return reply;
            
        }

        public async Task<ShiftSwitchReply> UpdateShiftSwitchReplyAsync(ShiftSwitchReply reply)
        {

            ShiftSwitchReply? existingReply = null;
            ShiftSwitchRequest? parentRequest = null;

            foreach (var request in _requests)
            {
                foreach (var replyFromRequest in request.SwitchReplies)
                {
                    if (replyFromRequest.Id == reply.Id)
                    {
                        existingReply = replyFromRequest;
                        parentRequest = request;
                        break;
                    }
                }

                if (existingReply != null)
                {
                    break;
                }
            }

            if (existingReply == null)
                throw new InvalidOperationException($"ShiftSwitchReply with ID {reply.Id} not found.");

            parentRequest.SwitchReplies.Remove(existingReply);
            parentRequest.SwitchReplies.Add(reply);
            return reply;
        }

        public async Task DeleteShiftSwitchReplyAsync(long id)
        {
            
                var replyToRemove = await GetSingleShiftSwitchReplyAsync(id);

                if (replyToRemove == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {id} not found.");

                var parentRequest =
                    await GetSingleShiftSwitchRequestAsync(await GetShiftSwitchRequestIdByShiftSwitchReplyId(id));

                parentRequest.SwitchReplies.Remove(replyToRemove);
            
        }
        
        public async Task<ShiftSwitchReply> GetSingleShiftSwitchReplyAsync(long id)
        {
           
                ShiftSwitchReply? foundReply = null;

                foreach (var request in _requests)
                {
                    foreach (var reply in request.SwitchReplies)
                    {
                        if (reply.Id == id)
                        {
                            foundReply = reply;
                            break;
                        }
                    }

                    if (foundReply != null)
                    {
                        break;
                    }
                }

                if (foundReply == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {id} not found.");

                return foundReply;
            
            
        }

        public async Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByRequestIdAsync(long requestId)
        {
            
                var reply = _requests.Where(r => r.Id == requestId).SelectMany(r => r.SwitchReplies).ToList();

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReplies with ID {requestId} not found.");

                return await Task.FromResult(reply);
            
        }
        
        

        public async Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByTargetEmployeeAsync(long employeeId)
        {
            
                var reply = _requests.SelectMany(r => r.SwitchReplies)
                    .Where(reply => reply.TargetEmployee.Id == employeeId).ToList();

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {employeeId} not found.");

                return reply;
            
        }
        
        
        public async Task<ShiftSwitchReply> SetShiftSwitchReplyTargetAcceptedAsync(long id, bool accepted)
        {
            
                var reply = await GetSingleShiftSwitchReplyAsync(id);

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {id} not found.");

                reply.TargetAccepted = accepted;
                return reply;
           
        }

        public async Task<ShiftSwitchReply> SetShiftSwitchReplyOriginAcceptedAsync(long id, bool accepted)
        {
            
                var reply = await GetSingleShiftSwitchReplyAsync(id);

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {id} not found.");

                reply.OriginAccepted = accepted;
                return reply;
            
        }

        public async Task<long> GetShiftSwitchRequestIdByShiftSwitchReplyId(long id)
        {
            
                var request = _requests.SingleOrDefault(r => r.SwitchReplies.Any(reply => reply.Id == id));
                return request.Id;
           
        }

        public async Task<ShiftSwitchRequestTimeframe> AddShiftSwitchRequestTimeframeAsync(ShiftSwitchRequestTimeframe timeframe, long requestId)
        {
            
                var request = _requests.SingleOrDefault(r => r.Id == requestId);

                if (request == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {requestId} not found.");

                request.Timeframes.Add(timeframe);
                return timeframe;
            
        }
        
        public async Task DeleteShiftSwitchRequestTimeframeAsync(long id)
        {
            
                var timeframeToRemove = await GetShiftSwitchRequestTimeframeSingleAsync(id);

                if (timeframeToRemove == null)
                    throw new InvalidOperationException($"ShiftSwitchRequestTimeframe with ID {id} not found.");

                var request = await GetSingleShiftSwitchRequestAsync(await GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(id));

                request.Timeframes.Remove(timeframeToRemove);
            
        }

        public async Task<ShiftSwitchRequestTimeframe> GetShiftSwitchRequestTimeframeSingleAsync(long id)
        {
            
                ShiftSwitchRequestTimeframe? foundTimeframe = null;

                foreach (var request in _requests)
                {
                    foreach (var timeframe in request.Timeframes)
                    {
                        if (timeframe.Id == id)
                        {
                            foundTimeframe = timeframe;
                            break;
                        }
                    }

                    if (foundTimeframe != null)
                    {
                        break;
                    }
                }

                if (foundTimeframe == null)
                    throw new InvalidOperationException($"ShiftSwitchRequestTimeframe with ID {id} not found.");

                return foundTimeframe;
           
        }

        public async Task<List<ShiftSwitchRequestTimeframe>> GetManyShiftSwitchRequestTimeframesByRequestIdAsync(long requestId)
        {
                var request = _requests.SingleOrDefault(r => r.Id == requestId);
                if (request is null)
                {
                    throw new ArgumentException($"There is no request with ID: {requestId}", nameof(requestId));
                }
                return request.Timeframes;
            
        }

        public async Task<long> GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(long id)
        {
           
                var request = _requests.FirstOrDefault(r => r.Timeframes.Any(timeframe => timeframe.Id == id));
                return request.Id;
            
        }
    }
}
