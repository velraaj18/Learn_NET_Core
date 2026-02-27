using System;

namespace FirstAPI.DTO;

public class UserRequest
{
    public string Email {get; set;}
    public string PasswordHash {get; set;}
}
