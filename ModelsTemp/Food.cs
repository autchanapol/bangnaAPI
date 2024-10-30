using System;
using System.Collections.Generic;

namespace bangnaAPI.ModelsTemp
{
    public partial class Food
    {
        public int Id { get; set; }
        public string? FoodName { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
