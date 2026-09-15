using Tasks.Api.Exceptions;

namespace Tasks.Api.Users;

public class UserService(UserRepository _userRepository)
{
    public async Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        if(await _userRepository.GetUserByEmail(request.Email) != null)
            throw new ValidationEx("User with this email already exists.");

        var user = new User 
        { 
            Name = request.Name, 
            Email = request.Email,
            PasswordHash = request.Password
        };
        
        await _userRepository.CreateUser(user);
        
        return new UserResponse 
        { 
            Id = user.Id, 
            Name = user.Name, 
            Email = user.Email 
        };
    }
}