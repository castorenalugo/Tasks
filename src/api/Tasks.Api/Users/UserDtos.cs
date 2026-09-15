namespace Tasks.Api.Users;

public class CreateUserRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class UserResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public static UserResponse FromUser(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };
}