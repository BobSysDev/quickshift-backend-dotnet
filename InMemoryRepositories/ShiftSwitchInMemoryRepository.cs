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
            try
            {
                _requests.Add(request);
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the shift switch request.", ex);
            }
        }

        public async Task<ShiftSwitchRequest> UpdateShiftSwitchRequestAsync(ShiftSwitchRequest request)
        {
            try
            {
                var existingRequest = _requests.SingleOrDefault(r => r.Id == request.Id);

                if (existingRequest == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {request.Id} not found.");

                _requests.Remove(existingRequest);
                _requests.Add(request);
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the shift switch request.", ex);
            }
        }


        public async Task DeleteShiftSwitchRequestAsync(long id)
        {
            try
            {
                var requestToRemove = _requests.SingleOrDefault(r => r.Id == id);

                if (requestToRemove == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {id} not found.");

                _requests.Remove(requestToRemove);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the shift switch request.", ex);
            }
        }
        
        public IQueryable<ShiftSwitchRequest> GetManyShiftSwitchRequestAsync()
        {
            return _requests.AsQueryable();
        }
        
        public async Task<ShiftSwitchRequest> GetSingleShiftSwitchRequestAsync(long id)
        {
            try
            {
                var request = _requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {id} not found.");

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the shift switch request.", ex);
            }
        }

        public async Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByEmployeeIdAsync(long employeeId)
        {
            try
            {
                var request = _requests.Where(r => r.OriginEmployee.Id == employeeId).ToList();
                return await Task.FromResult(request);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving shift switch requests by employee ID.", ex);
            }
        }

        public async Task<List<ShiftSwitchRequest>> GetManyShiftSwitchRequestsByShiftIdAsync(long shiftId)
        {
            try
            {
                var request = _requests.Where(r => r.OriginShift.Id == shiftId).ToList();
                return await Task.FromResult(request);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving shift switch requests by shift ID.", ex);
            }
        }

        public async Task<List<ShiftSwitchRequest>> GetShiftSwitchRequestByEmployeeAsync(long employeeId)
        {
            try
            {
                return _requests.Where(r => r.OriginEmployee.Id == employeeId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving shift switch requests by employee.", ex);
            }
        }

        public async Task<List<ShiftSwitchRequest>> GetShiftSwitchRequestByShiftAsync(long shiftId)
        {
            try
            {
                return _requests.Where(r => r.OriginShift.Id == shiftId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving shift switch requests by shift.", ex);
            }
        }
        
        public async Task<ShiftSwitchReply> AddShiftSwitchReplyAsync(ShiftSwitchReply reply, long requestId)
        {
            try
            {
                var request = _requests.SingleOrDefault(r => r.Id == requestId);

                if (request == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {requestId} not found.");

                request.SwitchReplies.Add(reply);

                return reply;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the shift switch reply.", ex);
            }
        }
        
        public async Task<ShiftSwitchReply> UpdateShiftSwitchReplyAsync(ShiftSwitchReply reply)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the shift switch reply.", ex);
            }
        }
        
        public async Task DeleteShiftSwitchReplyAsync(long id)
        {
            try
            {
                var replyToRemove = await GetSingleShiftSwitchReplyAsync(id);

                if (replyToRemove == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {id} not found.");

                var parentRequest =
                    await GetSingleShiftSwitchRequestAsync(await GetShiftSwitchRequestIdByShiftSwitchReplyId(id));

                parentRequest.SwitchReplies.Remove(replyToRemove);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the shift switch reply.", ex);
            }
        }
        
        public async Task<ShiftSwitchReply> GetSingleShiftSwitchReplyAsync(long id)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the shift switch reply.", ex);
            }
        }

        public async Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByRequestIdAsync(long requestId)
        {
            try
            {
                var reply = _requests.Where(r => r.Id == requestId).SelectMany(r => r.SwitchReplies).ToList();

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReplies with ID {requestId} not found.");

                return await Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving shift switch replies by request ID.", ex);
            }
        }
        
        

        public async Task<List<ShiftSwitchReply>> GetManyShiftSwitchRepliesByTargetEmployeeAsync(long employeeId)
        {
            try
            {
                var reply = _requests.SelectMany(r => r.SwitchReplies)
                    .Where(reply => reply.TargetEmployee.Id == employeeId).ToList();

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {employeeId} not found.");

                return reply;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving shift switch replies by target employee.", ex);
            }
        }
        
        
        public async Task<ShiftSwitchReply> SetShiftSwitchReplyTargetAcceptedAsync(long id, bool accepted)
        {
            try
            {
                var reply = await GetSingleShiftSwitchReplyAsync(id);

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {id} not found.");

                reply.TargetAccepted = accepted;
                return reply;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting target accepted for the shift switch reply.", ex);
            }
        }

        public async Task<ShiftSwitchReply> SetShiftSwitchReplyOriginAcceptedAsync(long id, bool accepted)
        {
            try
            {
                var reply = await GetSingleShiftSwitchReplyAsync(id);

                if (reply == null)
                    throw new InvalidOperationException($"ShiftSwitchReply with ID {id} not found.");

                reply.OriginAccepted = accepted;
                return reply;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting origin accepted for the shift switch reply.", ex);
            }
        }

        public async Task<long> GetShiftSwitchRequestIdByShiftSwitchReplyId(long id)
        {
            try
            {
                var request = _requests.FirstOrDefault(r => r.SwitchReplies.Any(reply => reply.Id == id));
                return request.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the shift switch request ID by reply ID.", ex);
            }
        }

        public async Task<ShiftSwitchRequestTimeframe> AddShiftSwitchRequestTimeframeAsync(ShiftSwitchRequestTimeframe timeframe, long requestId)
        {
            try
            {
                var request = _requests.SingleOrDefault(r => r.Id == requestId);

                if (request == null)
                    throw new InvalidOperationException($"ShiftSwitchRequest with ID {requestId} not found.");

                request.Timeframes.Add(timeframe);
                return timeframe;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the shift switch request timeframe.", ex);
            }
        }
        
        public async Task DeleteShiftSwitchRequestTimeframeAsync(long id)
        {
            try
            {
                var timeframeToRemove = await GetShiftSwitchRequestTimeframeSingleAsync(id);

                if (timeframeToRemove == null)
                    throw new InvalidOperationException($"ShiftSwitchRequestTimeframe with ID {id} not found.");

                var request = await GetSingleShiftSwitchRequestAsync(await GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(id));

                request.Timeframes.Remove(timeframeToRemove);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the shift switch request timeframe.", ex);
            }
        }

        public async Task<ShiftSwitchRequestTimeframe> GetShiftSwitchRequestTimeframeSingleAsync(long id)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the shift switch request timeframe.", ex);
            }
        }

        public async Task<List<ShiftSwitchRequestTimeframe>> GetManyShiftSwitchRequestTimeframesByRequestIdAsync(long requestId)
        {
            try
            {
                var request = _requests.SingleOrDefault(r => r.Id == requestId);
                return request.Timeframes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving shift switch request timeframes by request ID.", ex);
            }
        }

        public async Task<long> GetShiftSwitchRequestIdByShiftSwitchRequestTimeframeId(long id)
        {
            try
            {
                var request = _requests.FirstOrDefault(r => r.Timeframes.Any(timeframe => timeframe.Id == id));
                return request.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the shift switch request ID by timeframe ID.", ex);
            }
        }
    }
}
