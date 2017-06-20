using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundId { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpenFromMonth { get; set; }
        public int OpenTillMonth { get; set; }
        public decimal DailyFee { get; set; }

        public override string ToString()
        {
            return Name.ToString() + OpenFromMonth.ToString() + OpenTillMonth.ToString() + DailyFee.ToString("C");
        }

        public bool IsCampgroundOpen(int arrivalMonth, int departMonth)
        {
            if ((this.OpenFromMonth <= arrivalMonth) && (departMonth <= this.OpenTillMonth))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
