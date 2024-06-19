// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var users = new Users();

var addUserRequestDTO = new AddUserRequestDTO("dummay1", "dummay2", 10);

var result = await users.Add(addUserRequestDTO);
Console.WriteLine(result.Success);