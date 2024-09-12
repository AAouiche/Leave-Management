using AutoMapper;
using LeaveManagement.Shared.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Errors;
using Identity.Dtos;
using Identity.Interfaces;

namespace LeaveManagement.Application.Features.Authentication.Commands.GetAllUsers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersCommand, Result<List<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllUsersHandler> _logger;

    public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetAllUsersHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<UserDto>>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync();

        if (users == null || users.Count == 0)
        {
            _logger.LogError("No users found in the database.");
            return Result.Failure<List<UserDto>>(UserErrors.NoUsersFound());
        }

            
        var userDtos = _mapper.Map<List<UserDto>>(users);

        return Result<List<UserDto>>.Success(userDtos);
    }
}

