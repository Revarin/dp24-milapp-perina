namespace Kris.Client.Core.Services;

// Source: https://stackoverflow.com/a/75574574
public sealed class PermissionService : IPermissionService
{
    public async Task<PermissionStatus> CheckAndRequestPermissionAsync<TPermission>() where TPermission : Permissions.BasePermission, new()
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var permission = new TPermission();
            var status = await permission.CheckStatusAsync();

            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        });
    }

    public async Task<PermissionStatus> CheckPermissionAsync<TPermission>() where TPermission : Permissions.BasePermission, new()
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var permission = new TPermission();
            return await permission.CheckStatusAsync();
        });
    }
}
