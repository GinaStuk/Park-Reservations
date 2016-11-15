using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.CLI
{
    public class ParkCLI
    {
        private string connectionString;

        public ParkCLI(string dbConnectionString)
        {
            //get the connection string from the main program
            connectionString = dbConnectionString;
            //begin the startscreen loop
            StartScreen();
        }

        public void StartScreen()
        {
            //loop to repeat until the user (Q)uits
            bool isRunning = true;
            while (isRunning)
            {
                //make a list of parks
                ParkSqlDAL dal = new ParkSqlDAL(connectionString);
                List<Park> parks = dal.GetParks();

                //display the list of parks and the quit option
                Console.WriteLine("VIEW PARKS INTERFACE");
                Console.WriteLine("Select a Park for further details:");
                for (int i = 1; i <= parks.Count; i++)
                {
                    Console.WriteLine(i.ToString() + ") " + parks[i - 1].Name);
                }
                Console.WriteLine("Q) quit");

                //read input
                string input = Console.ReadLine();
                int selection = -1;
                //check that the input is an integer, set it to selection
                //selection set to 0 if input is not an integer
                bool isInt = Int32.TryParse(input, out selection);

                //if/elseif to determine course of action.  
                if (input.ToLower() == "q")
                {
                    //quit current loop (ends program)
                    Console.WriteLine("Thank you for visiting today.");
                    isRunning = false;

                }
                else if (selection > 0 && selection <= parks.Count)
                {
                    //moves to the display park loop
                    Console.Clear();
                    DisplayPark(parks[selection - 1]);
                }
                else
                {
                    //display message, go back to start of this loop
                    Console.Clear();
                    Console.WriteLine("Please make a valid selection.");
                }

            }
        }

        private void DisplayPark(Park p)
        {
            //loop to repeat until the user selects 3 to go back
            bool thisLoop = true;
            while (thisLoop)
            {
                //display inforamtion from selected park
                Console.WriteLine("PARK INFORMATION SCREEN");
                Console.WriteLine(p.Name + " National Park");
                Console.WriteLine("Location: ".PadRight(20) + p.Location);
                Console.WriteLine("Established: ".PadRight(20) + p.EstablishedDate.Date);
                Console.WriteLine("Area: ".PadRight(20) + p.Area);
                Console.WriteLine("Annual Visitors: ".PadRight(20) + p.Visitors);
                Console.WriteLine(p.Description + "\n");
                //display the user's options
                Console.WriteLine("Select a command:");
                Console.WriteLine("1) View campgrounds");
                Console.WriteLine("2) Search for reservations");
                Console.WriteLine("3) Return to previous screen");

                //read input
                string input = Console.ReadLine();
                int selection = -1;

                //check that the input is an integer, set it to selection
                //selection set to 0 if input is not an integer
                bool isInt = Int32.TryParse(input, out selection);

                //if/elseif to determine course of action.  
                if (selection == 1)
                {
                    //move to display list of campgrounds loop
                    Console.Clear();
                    DisplayCampground(p);
                }
                else if (selection == 2)
                {
                    //move to select campgrounds loop
                    Console.Clear();
                    SelectCampground(p);
                }
                else if (selection == 3)
                {
                    //exit loop, go back to previous (startscreen)
                    Console.Clear();
                    thisLoop = false;
                }
                else
                {
                    //display message, go back to start of this loop
                    Console.Clear();
                    Console.WriteLine("Please make a valid selection.");
                }

            }
        }

        private void DisplayCampground(Park p)
        {
            //loop to repeat until the user presses 2 to go back
            bool thisLoop = true;
            while (thisLoop)
            {

                //make a list of campgrounds in the park that was selected easier
                CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
                List<Campground> campgrounds = dal.GetCampgroundsByPark(p.ParkID);

                //initial display
                Console.WriteLine("PARK CAMPGROUNDS ");
                Console.WriteLine("This is a list of campgrounds at the park");

                //displays the list of campgrounds available at this park
                DisplayCampgrounds(p, campgrounds);

                //displays the list of options
                Console.WriteLine("Select a command");
                Console.WriteLine("1) Search for Available Reservation");
                Console.WriteLine("2) Return to previous screen");

                //read input
                string input = Console.ReadLine();
                int selection = -1;

                //check that the input is an integer, set it to selection
                //selection set to 0 if input is not an integer
                bool isInt = Int32.TryParse(input, out selection);

                //if/elseif to determine course of action. 
                if (selection == 1)
                {
                    //move to select campgrounds loop
                    Console.Clear();
                    SelectCampground(p);
                }
                else if (selection == 2)
                {
                    //exit loop, go back to previous (display park)
                    Console.Clear();
                    thisLoop = false;
                }
                else
                {
                    //display message, go back to start of this loop
                    Console.Clear();
                    Console.WriteLine("Please make a valid selection.");
                }

            }

        }

        private void SelectCampground(Park p)
        {
            //loop to repeat until the user presses b to go back
            bool thisLoop = true;
            while (thisLoop)
            {
                //make a list of campgrounds in the park that was selected easier
                CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
                List<Campground> campgrounds = dal.GetCampgroundsByPark(p.ParkID);

                //initial display
                Console.WriteLine("SEARCH FOR CAMPGROUND RESERVATION ");
                Console.WriteLine("Select a park from the following list:");

                //displays the list of campgrounds available at this park
                DisplayCampgrounds(p, campgrounds);

                //displays the list of options
                Console.WriteLine("\n What Campground would you like to select (press b to go Back)");

                //read input
                string input1 = Console.ReadLine();
                int selection = -1;

                //check that the input is an integer, set it to selection
                //selection set to 0 if input is not an integer
                bool isInt = Int32.TryParse(input1, out selection);

                //if/elseif to determine course of action. 
                if (input1.ToLower() == "b")
                {
                    //exit loop, go back to previous (display park OR display campground )
                    Console.Clear();
                    Console.WriteLine("returning to previous screen");
                    thisLoop = false;
                }
                else if (selection > 0 && selection <= campgrounds.Count)
                {
                    //displays the selected campground based on input number
                    bool anotherloop = true;
                    while (anotherloop)
                    {
                        Console.Clear();
                        Console.WriteLine("You selected " + campgrounds[selection - 1].Name);

                        Console.WriteLine("What day will you arrive?");
                        Console.WriteLine("Format: YYYY-MM-DD");
                        string input2 = Console.ReadLine();
                        DateTime selection2;
                        bool isDateTime = DateTime.TryParse(input2, out selection2);

                        if (isDateTime)
                        {
                            bool thirdloop = true;
                            while (thirdloop)
                            {
                                Console.WriteLine("What day will you depart?");
                                Console.WriteLine("Format: YYYY-MM-DD");
                                string input3 = Console.ReadLine();
                                DateTime selection3;
                                bool isDateTime2 = DateTime.TryParse(input3, out selection3);

                                if (isDateTime2)
                                {
                                    //this is where we do the crazy SQL thing
                                    //in SiteSqlDAL
                                    //test writelines to make sure this works                                    
                                    thirdloop = false;
                                    anotherloop = false;
                                    ShowAvailableSites(campgrounds[selection - 1], selection2, selection3);
                                }
                                else
                                {
                                    Console.WriteLine("the date was not formatted correctly");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("the date was not formatted correctly");
                        }
                    }
                    //NOT YET IMPLEMENTED: 
                    //go to a new loop that runs a SiteSqlDAL 
                    //that uses these 3 bits of info to get and display list of available sites
                    //Console.WriteLine("This isn't implemented yet");
                    //Console.WriteLine("returning to campground selection screen");
                }
                else
                {
                    //display message, go back to start of this loop
                    Console.Clear();
                    Console.WriteLine("invalid selection");
                }
            }
        }

        private void ShowAvailableSites(Campground c, DateTime startDate, DateTime endDate)
        {
            bool thisloop = true;
            while (thisloop)
            {
                SiteSqlDAL dal = new SiteSqlDAL(connectionString);
                List<Site> sites = dal.GetSitesByCampground(c.CampgroundID, startDate, endDate);

                int days = (int)(endDate - startDate).TotalDays;
                decimal totalCost = c.DailyFee * days;


                Console.WriteLine("RESULTS MATCHING YOUR CRITERIA");
                Console.WriteLine(c.Name + " From " + startDate.ToString() + " To " + endDate.ToString() + "\n");

                Console.WriteLine(" ".PadRight(5) + "Site No. ".PadRight(10) + "Max Occup. ".PadRight(15) + "Accessible? ".PadRight(15) +
                    "Max RV Length ".PadRight(15) + "Utility ".PadRight(10) + "Cost ");
                for (int i = 0; i < sites.Count; i++)
                {
                    Console.WriteLine((i + 1).ToString().PadRight(5) + sites[i].SiteNumber.ToString().PadRight(10) +
                        sites[i].MaxOccupancy.ToString().PadRight(15) + sites[i].Accessibile.ToString().PadRight(15) +
                        sites[i].MaxRVLength.ToString().PadRight(15) + sites[i].Utilities.ToString().PadRight(10) + totalCost.ToString("C"));
                }
                Console.WriteLine();

                Console.WriteLine("Which Site should be reserved (c to cancel)?");

                string input = Console.ReadLine();//this is currently eating everything somehow

                Console.WriteLine("TEST");
                int selection = -1;
                //check that the input is an integer, set it to selection
                //selection set to 0 if input is not an integer
                bool isInt = Int32.TryParse(input, out selection);

                if (input.ToLower() == "c")
                {
                    Console.Clear();
                    Console.WriteLine("Cancelling reservation, returning to previous screen");
                    thisloop = false;
                }
                else if (selection > 0 && selection <= sites.Count)
                {
                    Console.WriteLine("You Selected Site Number " + sites[selection - 1].SiteNumber);
                    Console.WriteLine("State the name for your reservation");

                    string reservationName = Console.ReadLine();

                    Console.WriteLine("Not quite done yet");
                    Console.WriteLine("site ID: " + sites[selection - 1].SiteID.ToString() + " name: " + reservationName +
                        " start date: " + startDate.ToString() + " end date: " + endDate.ToString());
                    //call the GetReservations function
                    ReservationSqlDAL dal2 = new ReservationSqlDAL(connectionString);
                    int reservationID = dal2.GetReservations(sites[selection - 1].SiteID, startDate, endDate, reservationName);
                    //need some way to get back the new reservation ID
                    Console.WriteLine($"The reservation has been made and the confirmation ID is {reservationID}");
                    thisloop = false;

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please make a valid selection");
                }

            }
        }


        private static void DisplayCampgrounds(Park p, List<Campground> campgrounds)
        {
            //display the name of the park
            Console.WriteLine(p.Name + " National Park Campgrounds \n");
            //display the spaced out categores from the campsites
            Console.WriteLine(" ".PadRight(5) + "Name".PadRight(35) + "Open".PadRight(10)
                + "Close".PadRight(10) + "Daily Fee");
            //loop through the campsites, displaying their number +1 and their information
            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine((i + 1).ToString().PadRight(5) + campgrounds[i].Name.PadRight(35) +
                        campgrounds[i].OpenFromMonth.ToString().PadRight(10) +
                        campgrounds[i].OpenToMonth.ToString().PadRight(10) +
                        campgrounds[i].DailyFee.ToString("C"));
            }
        }
    }
}

//TODO
//write tests for reservationSqlDAL
//convert displayed datetimes to not show anything beyond date in CLI
//convert displayed months to show month name instead of number
//finish commenting the code
//run more/other tests, possibly look into bonus stuff