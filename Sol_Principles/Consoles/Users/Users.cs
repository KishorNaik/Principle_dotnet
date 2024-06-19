using Sol_Day1.Users.Features;

namespace Consoles.Users;

public class Users
{
    public Task<DataResponse<AddUserResponseDTO>> Add(AddUserRequestDTO addUserRequestDTO)
    {
        IAddUsersHandler handler = new AddUsersHandler(new AddUserRepository());
        return handler.HandleAsync(addUserRequestDTO);
    }

    public Task<DataResponse<UpdateUserResponseDTO>> Update(UpdateUserRequestDTO updateUserRequestDTO)
    {
        IUpdateUserHandler handler = new UpdateUserHandler(new UpdateUserRepository());
        return handler.HandleAsync(updateUserRequestDTO);
    }

    public void Delete()
    {
    }
}