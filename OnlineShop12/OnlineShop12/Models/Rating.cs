﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Rating
    {
        [Key]
        public int Id_Rating { get; set; }

        public int Id_User { get; set; }

        public int? Id_Product { get; set; }
        public virtual Product? Product { get; set; }

        public DateTime Date {get; set; }
    }
}
