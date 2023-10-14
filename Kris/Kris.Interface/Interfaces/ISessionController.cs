﻿namespace Kris.Interface
{
    public interface ISessionController
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task UpdateUserName(UpdateUserNameRequest request);
        Task<bool> UserExists(UserExistsRequest request);
    }
}