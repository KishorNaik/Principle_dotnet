namespace Sol_Day1.Users.Features;

#region Request

public class UpdateUserRequestDTO
{
    public UpdateUserRequestDTO(int id, string firstName, string lastName, int age)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Age = age;
    }

    public int Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public int Age { get; }
}

#endregion Request

#region Response

public class UpdateUserResponseDTO
{
    public UpdateUserResponseDTO(string message, string fullName)
    {
        Message = message;
        FullName = fullName;
    }

    public string Message { get; }

    public string FullName { get; }
}

#endregion Response

#region Handler

public interface IUpdateUserHandler : IHandler<UpdateUserRequestDTO, DataResponse<UpdateUserResponseDTO>>
{
}

public sealed class UpdateUserHandler : IUpdateUserHandler
{
    private readonly IUpdateUserRepository _repository;

    public UpdateUserHandler(IUpdateUserRepository repository)
    {
        _repository = repository;
    }

    private Task<DataResponse<UpdateUserResponseDTO>> ResponseAsync(UserEntity userResult)
    {
        var updateUserResponse = new UpdateUserResponseDTO("Update successfully", $"{userResult.FirstName} {userResult.LastName}");

        var response = DataResponseFactory.Response(true, Convert.ToInt32(HttpStatusCode.OK), updateUserResponse, "User Updated");

        return Task.FromResult(response);
    }

    private UserEntity Map(UpdateUserRequestDTO request)
    {
        return new UserEntity()
        {
            Age = request.Age,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Id = request.Id,
        };
    }

    async Task<DataResponse<UpdateUserResponseDTO>> IHandler<UpdateUserRequestDTO, DataResponse<UpdateUserResponseDTO>>.HandleAsync(UpdateUserRequestDTO request)
    {
        if (request is null)
            return DataResponseFactory.Response<UpdateUserResponseDTO>(false, Convert.ToInt32(HttpStatusCode.BadRequest), null, "Update User request dto object should not be null");

        // Map
        UserEntity userMap = this.Map(request);

        // Repository
        Result<UserEntity> userResult = await _repository.UpdateAsync(userMap);
        if (userResult.IsFailed)
            return DataResponseFactory.Response<UpdateUserResponseDTO>(false, Convert.ToInt32(userResult.Errors[0].Metadata["StatusCode"]), null, userResult.Errors[0].Message);

        // Response
        return await this.ResponseAsync(userResult.Value);
    }
}

#endregion Handler

#region Repository

public interface IUpdateUserRepository
{
    Task<Result<UserEntity>> UpdateAsync(UserEntity userEntity);
}

public sealed class UpdateUserRepository : IUpdateUserRepository
{
    async Task<Result<UserEntity>> IUpdateUserRepository.UpdateAsync(UserEntity userEntity)
    {
        try
        {
            if (userEntity is null)
                return Result.Fail<UserEntity>(new FluentResults.Error("User entity object should not be empty").WithMetadata("StatusCode", HttpStatusCode.BadRequest));

            // DB Operation

            return Result.Ok<UserEntity>(userEntity);
        }
        catch (Exception ex)
        {
            return Result.Fail(new FluentResults.Error(ex.Message).WithMetadata("StatusCode", HttpStatusCode.InternalServerError));
        }
    }
}

#endregion Repository