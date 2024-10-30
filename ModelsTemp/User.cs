using System;
using System.Collections.Generic;

namespace bangnaAPI.ModelsTemp
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Name { get; set; }
        public int? Role { get; set; }
        public int Status { get; set; }
        public int? StatusActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string? UpdateBy { get; set; }
    }
}
