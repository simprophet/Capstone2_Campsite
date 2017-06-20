using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Classes;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            CampgroundCLI cgCLI = new CampgroundCLI();

            cgCLI.DisplayParks();
        }
    }
}
