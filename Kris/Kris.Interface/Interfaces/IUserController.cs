﻿using Kris.Interface.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Interfaces;

public interface IUserController
{
    Task<ActionResult<object>> RegisterUser(RegisterUserRequest request);
    Task<ActionResult<object>> LoginUser(LoginUserRequest request);
    Task<ActionResult<object>> StoreUserSettings(StoreUserSettingsRequest request);
}
