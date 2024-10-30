using System;
using System.Collections.Generic;

namespace bangnaAPI.ModelsTemp
{
    public partial class Bed
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? WardId { get; set; }
        public string? Remarks { get; set; }
        public int? Status { get; set; }
        public int? Actived { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
