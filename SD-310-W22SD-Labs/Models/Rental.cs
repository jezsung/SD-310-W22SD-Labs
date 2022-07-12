using System;
using System.Collections.Generic;

namespace SD_310_W22SD_Labs.Models
{
    public partial class Rental
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EquipmentId { get; set; }
        public int RentalHours { get; set; }
        public DateTime RentedAt { get; set; }
        public bool IsCurrent { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Equipment Equipment { get; set; } = null!;
    }
}
