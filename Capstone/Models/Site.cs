﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsAccessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }

        public override string ToString()
        {
            return SiteID.ToString() + CampgroundID.ToString() + SiteNumber.ToString() + MaxOccupancy.ToString() + IsAccessible.ToString() + MaxRVLength.ToString() + Utilities.ToString();
        }
    }
}
