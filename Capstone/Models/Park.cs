using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public int Area { get; set; }
        public int AnnualVisitors { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Name.ToString() + Location.ToString() + EstablishDate.ToString() + Area.ToString() + AnnualVisitors.ToString() + Description.ToString();
        }

    }
}
