namespace Kris.Client.Core
{
    // Source: https://stackoverflow.com/a/75574574
    public class PermissionsService : IPermissionsService
    {
        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<TPermission>() where TPermission : Permissions.BasePermission, new()
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                TPermission permission = new TPermission();
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
                TPermission permission = new TPermission();
                return await permission.CheckStatusAsync();
            });
        }
    }
}
