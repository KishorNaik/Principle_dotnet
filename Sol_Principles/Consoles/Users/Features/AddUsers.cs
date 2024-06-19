namespace Consoles.Users.Features;

#region Request DTO

public class AddUserRequestDTO
{
    public AddUserRequestDTO(string firstName, string lastName, int? age)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Age = age;
    }

    public string? FirstName { get; }

    public string? LastName { get; }

    public int? Age { get; }
}

#endregion Request DTO

#region Response DTO

public class AddUserResponseDTO
{
    public AddUserResponseDTO(int? id)
    {
        this.Id = id;
    }

    public int? Id { get; }
}

#endregion Response DTO

#region Handler

public interface IAddUsersHandler : IHandler<AddUserRequestDTO, DataResponse<AddUserResponseDTO>>
{
}

public sealed class AddUsersHandler : IAddUsersHandler
{
    private readonly IAddUserRepository _repository;

    public AddUsersHandler(IAddUserRepository repository)
    {
        this._repository = repository;
    }

    private UserEntity Map(AddUserRequestDTO request)
    {
        return new UserEntity()
        {
            Age = request.Age,
            FirstName = request.FirstName,
            LastName = request.FirstName
        };
    }

    private Task<DataResponse<AddUserResponseDTO>> ResponseAsync(UserEntity userResult)
    {
        var result = new AddUserResponseDTO(userResult.Id);

        var response = DataResponseFactory.Response(true, Convert.ToInt32(HttpStatusCode.OK), result, "Successfully");

        return Task.FromResult(response);
    }

    async Task<DataResponse<AddUserResponseDTO>> IHandler<AddUserRequestDTO, DataResponse<AddUserResponseDTO>>.HandleAsync(AddUserRequestDTO request)
    {
        try
        {
            // Guard Clause (Check Null Condition)
            if (request == null)
                return DataResponseFactory.Response<AddUserResponseDTO>(false, Convert.ToInt32(HttpStatusCode.BadRequest), null, "request object should not be empty.");

            //Map
            UserEntity userMap = this.Map(request);

            // Repository
            Result<UserEntity> userResult = await _repository.AddUserAsync(userMap);

            if (userResult.IsFailed)
                return DataResponseFactory.Response<AddUserResponseDTO>(false, Convert.ToInt32(userResult.Errors[0].Metadata["StatusCode"]), null, userResult.Errors[0].Message);

            // Response
            return await this.ResponseAsync(userResult.Value);
        }
        catch (Exception ex)
        {
            return DataResponseFactory.Response<AddUserResponseDTO>(false, Convert.ToInt32(HttpStatusCode.InternalServerError), null, ex.Message);
        }
    }
}

#endregion Handler

#region Repository

public interface IAddUserRepository
{
    Task<Result<UserEntity>> AddUserAsync(UserEntity user);
}

public sealed class AddUserRepository : IAddUserRepository
{
    async Task<Result<UserEntity>> IAddUserRepository.AddUserAsync(UserEntity user)
    {
        try
        {
            if (user is null)
                return Result.Fail<UserEntity>(new FluentResults.Error("UserEntity object should be empty").WithMetadata("StatusCode", HttpStatusCode.BadRequest));

            // DB Operation

            //Map User Id
            user.Id = 1;

            return Result.Ok<UserEntity>(user);
        }
        catch (Exception ex)
        {
            return Result.Fail<UserEntity>(new FluentResults.Error(ex.Message).WithMetadata("StatusCode", HttpStatusCode.InternalServerError));
        }
    }
}

#endregion Repository