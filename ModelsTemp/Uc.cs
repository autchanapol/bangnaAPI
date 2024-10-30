using System;
using System.Collections.Generic;

namespace bangnaAPI.ModelsTemp
{
    public partial class Uc
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public string Remarks { get; set; } = null!;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
