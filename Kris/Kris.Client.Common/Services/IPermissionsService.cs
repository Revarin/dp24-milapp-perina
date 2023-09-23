﻿namespace Kris.Client.Common
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckAndRequestPermissionAsync<TPermission>() where TPermission : Permissions.BasePermission, new();
        Task<PermissionStatus> CheckPermissionAsync<TPermission>() where TPermission : Permissions.BasePermission, new();
    }
}