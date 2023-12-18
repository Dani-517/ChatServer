using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatServer
{
    public static class PermissionManager
    {
        // under construction, this class will contain some permission related code


        public class chatUserPermission
        {
            [JsonPropertyName("permissionID")]
            public uint? permissionID { get; set; }

            [JsonPropertyName("permissionName")]
            public string? permissionName { get; set; }
        }
    }
}