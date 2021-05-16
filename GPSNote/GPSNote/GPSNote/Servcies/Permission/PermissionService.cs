using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GPSNote.Servcies.Permission
{
    public class PermissionService : IPermissionService
    {
		public async Task<PermissionStatus> CheckPermissionsAsync(BasePermission permission)
		{
			return await permission.CheckPermissionStatusAsync();
		}

		public async Task<PermissionStatus> RequestPermissionAsync(BasePermission permission)
		{

			return await permission.RequestPermissionAsync();
		}
	}
}
