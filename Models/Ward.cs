using System;
using System.Collections.Generic;

namespace bangnaAPI.Models
{
    public partial class Ward
    {
        public int Id { get; set; }
        public string? WardName { get; set; }
        public string? Remarks { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
