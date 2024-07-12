using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingMall.Models.Admin
{
    public class AdminUserDataDtoResponse
    {
        public int Id { get; set; }
        public string Acc { get; set; }
        public string Name { get; set; }
        public int Enabled { get; set; }
        public List<AdminUserRoleData> Role { get; set; }
    }
    public class AdminUserRoleData
    {
        public int RoleId { get; set; }
    }
    public class InsertAdminDataDto
    {
        public string Name { get; set; }
        public string Acc { get; set; }
        public string Pwd { get; set; }
        public List<int> Roles { get; set; }
        public int Enabled { get; set; }
    }
}