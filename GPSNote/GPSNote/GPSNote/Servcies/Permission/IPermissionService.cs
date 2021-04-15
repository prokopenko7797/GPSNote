﻿using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Servcies.Permission
{
    public interface IPermissionService
    {
        Task<PermissionStatus> CheckPermissions(BasePermission permission);
	}
}