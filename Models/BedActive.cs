using System;
using System.Collections.Generic;

namespace bangnaAPI.Models
{
    public partial class BedActive
    {
        public int Id { get; set; }
        public int? BedId { get; set; }
        public int? UdId { get; set; }
        public string? HnId { get; set; }
        public string? Remarks { get; set; }
        public string? HnName { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
