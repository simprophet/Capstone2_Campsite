using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Classes;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Classes
{
    public class CampgroundCLI
    {
        private ParkSqlDAL parkSqlDal;
        private CampgroundSqlDAL campGSqlDal;
        private ReservatonSqlDAL reservationSqlDal;
        private SiteSqlDAL siteSqlDal;

        public string connectionString;

        public CampgroundCLI()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
            this.parkSqlDal = new ParkSqlDAL(connectionString);
            this.campGSqlDal = new CampgroundSqlDAL(connectionString);
            this.reservationSqlDal = new ReservatonSqlDAL(connectionString);
            this.siteSqlDal = new SiteSqlDAL(connectionString);
        }

        public void DisplayParks()
        {
            List<Park> parkList = new List<Park>();
            parkList = parkSqlDal.GetParks();

            string userInput = "";
            bool isInputANumber = true;
            int InputAsNumber = 0;

            Console.WriteLine("Select a park for further details");

            /****** DISPLAY LIST OF PARKS AND OPTION TO QUIT******/
            for (int i = 0; i < parkList.Count; i++)
            {
                Console.WriteLine((i + 1) + ") " + parkList[i].Name);
            }
            Console.WriteLine("Q) quit");

            /******KEEP ASKING FOR USER INPUT UNTIL IT IS VALID******/
            while (true)
            {
                userInput = Console.ReadLine().ToLower();

                /******CHECK WHETHER OR NOT USER INPUT IS A VALID NUMBER******/
                isInputANumber = int.TryParse(userInput, out int result);

                if (userInput == "q")
                {
                    Environment.Exit(0);
                }
                else if (isInputANumber && result <= parkList.Count)
                {
                    InputAsNumber = result;
                    break;
                }
                Console.WriteLine("Invalid Entry. Please try again.");
            }
            DisplayParkDetails(parkList[InputAsNumber - 1]);
        }

        public void DisplayParkDetails(Park whichParkToDisplay)
        {
            string userInput = "";

            while (true)
            {

                /****** DISPLAY PARK DEATAILS******/
                Console.WriteLine("Park Information");
                Console.WriteLine(whichParkToDisplay.Name + " National Park");
                Console.WriteLine("Location:" + whichParkToDisplay.Location);
                Console.WriteLine("Established:" + whichParkToDisplay.EstablishDate);
                Console.WriteLine("Area:" + whichParkToDisplay.Area);
                Console.WriteLine("Annual Visitors :" + whichParkToDisplay.AnnualVisitors);
                Console.WriteLine("\n" + whichParkToDisplay.Description);

                /****** DISPLAY NEXT SET OF OPTIONS******/
                Console.WriteLine("1) View Campgrounds");
                Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("3) Return to Previous Screen");


                userInput = Console.ReadLine();
                if (userInput == "1")
                {
                    ViewCampgrounds(whichParkToDisplay);

                }
                else if (userInput == "2")
                {
                    SearchForAvailableReservations(campGSqlDal.GetCampgrounds(whichParkToDisplay));
                    break;
                }
                else if (userInput == "3")
                {
                    DisplayParks();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input, press 'Enter' to try again.");
                    Console.ReadLine();
                }

                Console.Clear();
            }
        }

        public void ViewCampgrounds(Park whichParkToDisplay)
        {
            List<Campground> campgroundList = new List<Campground>();
            campgroundList = campGSqlDal.GetCampgrounds(whichParkToDisplay);
            string userInput = "";

            Console.WriteLine("Park Campgrounds");
            Console.WriteLine($"{whichParkToDisplay.Name} National Park Campgrounds");
            Console.WriteLine("Name\tOpen\tClose\tDaily Fee");

            for (int i = 0; i < campgroundList.Count; i++)
            {
                Console.WriteLine("#" + (i + 1) + "\t" + campgroundList[i].Name + "\t" + campgroundList[i].OpenFromMonth + "\t" + campgroundList[i].OpenTillMonth + "\t" + campgroundList[i].DailyFee.ToString("C"));
            }

            Console.WriteLine("Select a Command");
            Console.WriteLine("1) Search for Available Reservation");
            Console.WriteLine("2) Return to Previous Screen");
            userInput = Console.ReadLine();

            if (userInput == "1")
            {
                SearchForAvailableReservations(campgroundList);
            }
            else if (userInput == "2")
            {
                return;
            }

        }

        public void SearchForAvailableReservations(List<Campground> listOfCampgrounds)
        {
            List<Site> sites = new List<Site>();
            List<Site> checkedSites = new List<Site>();
            DateTime userArrivalInput;
            DateTime userDepartInput;
                        
            int selectedCampground;
            int selectedSite;
            int confirmationId;

            Console.WriteLine("Search for Campground Reservation");
            Console.WriteLine("Name\tOpen\tClose\tDaily Fee");

            for (int i = 0; i < listOfCampgrounds.Count; i++)
            {
                Console.WriteLine("#" + (i + 1) + "\t" + listOfCampgrounds[i].Name + "\t" + listOfCampgrounds[i].OpenFromMonth + "\t" + listOfCampgrounds[i].OpenTillMonth + "\t" + listOfCampgrounds[i].DailyFee.ToString("C"));
            }

            Console.WriteLine("Which campground (enter 0 to cancel)");
            selectedCampground = int.Parse(Console.ReadLine());

            if(selectedCampground == 0)
            {
                ViewCampgrounds(whichParkToDisplay)
            }
            do
            {
                Console.WriteLine("Please enter valid dates");

                userArrivalInput = GetArrivalDate();
                userDepartInput = GetDepartureDate();

            } while (!listOfCampgrounds[selectedCampground - 1].IsCampgroundOpen(userArrivalInput.Month, userDepartInput.Month));
            /******KEEP ASKING USER FOR DATES IF THE PARK IS NOT OPEN FOR THE MONTHS THEY ENTERED******/

            /******BUILD A LIST OF ALL SITES ON THE CAMPGROUND IN QUESTION******/
            sites = siteSqlDal.GetSites(listOfCampgrounds[selectedCampground - 1].CampgroundId);

            /******PASS THAT LIST INTO RESERVATION DAL AND CHECK USER ARRIVAL AND DEPARTS AGAINST EXISTING RESERVATIONS******/
            checkedSites = reservationSqlDal.CheckReservations(sites, userArrivalInput, userDepartInput);

            /******MAKE SURE THAT THE CHECK RESERVATIONS METHOD RETURNED ANY SITES******/
            if (checkedSites.Count != 0 && checkedSites != null)
            {
                Console.WriteLine("Results Matching Your Search Criteria");
                Console.WriteLine("Site No. \t Max Occup. \t Accessible? \t Max RV Length \t Utility \t Cost");
                for (int i = 0; i < checkedSites.Count; i++)
                {
                    Console.WriteLine(checkedSites[i].SiteNumber + "\t" + checkedSites[i].MaxOccupancy + "\t" + checkedSites[i].IsAccessible + "\t" + checkedSites[i].MaxRVLength + "\t" + checkedSites[i].Utilities + "\t" + listOfCampgrounds[selectedCampground - 1].DailyFee);
                }
            }
            else
            {
                Console.WriteLine("There are no results matching your criteria. Return to selection.");
                DisplayParks();
            }

            Console.WriteLine("Which site should be reserved(enter 0 to cancel)?");
            selectedSite = int.Parse(Console.ReadLine());
            Console.WriteLine("What name should the reservation be made under?");
            string reservationName = Console.ReadLine() + " Family Reservation";

            /******INSERT THE RESERVATION INTO THE DATABASE AND RETURN A CONFIRMATION ID******/
            confirmationId = reservationSqlDal.ConfirmReservation(checkedSites[selectedSite - 1], reservationName, userArrivalInput, userDepartInput);
            //confirmationId = reservationSqlDal.GetConfirmationId(reservationName);
            Console.WriteLine($"The reservation has been made and the confirmation id is {confirmationId}");
            Console.ReadLine();
        }

        private DateTime GetDepartureDate()
        {
            /******GET USER DEPARTURE DATE******/
            Console.WriteLine("What is the departure date?\t__/__/____");
            Console.WriteLine("Month: ?");
            int userDepartMonthInput = int.Parse(Console.ReadLine());
            Console.WriteLine("Day: ?");
            int userDepartDayInput = int.Parse(Console.ReadLine());
            Console.WriteLine("Year: ?");
            int userDepartYearInput = int.Parse(Console.ReadLine());

            return new DateTime(userDepartYearInput, userDepartMonthInput, userDepartDayInput);
        }

        private DateTime GetArrivalDate()
        {            
            Console.WriteLine("What is the arrival date?\t__/__/____");
            Console.WriteLine("Month: ?");
            int userArrivalMonthInput = int.Parse(Console.ReadLine());
            Console.WriteLine("Day: ?");
            int userArrivalDayInput = int.Parse(Console.ReadLine());
            Console.WriteLine("Year: ?");
            int userArrivalYearInput = int.Parse(Console.ReadLine());
            return new DateTime(userArrivalYearInput, userArrivalMonthInput, userArrivalDayInput);
        }
    }
}
