using System;
using System.Collections.Generic;

namespace bangnaAPI.ModelsTemp
{
    public partial class OrderFood
    {
        public int Id { get; set; }
        public int? BedActiveId { get; set; }
        public int? FoodId { get; set; }
        public int? Status { get; set; }
        public string? Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
