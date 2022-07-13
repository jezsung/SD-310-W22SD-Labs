using System;
using System.Collections.Generic;

namespace SD_310_W22SD_Labs.Models
{
    public partial class Equipment
    {
        public Equipment()
        {
            Rentals = new HashSet<Rental>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
