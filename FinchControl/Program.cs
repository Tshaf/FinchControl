using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;


namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: CIT Finch Control Project
    // Description: Application that controls different
    // functions of the finch             
    // Application Type: Console
    // Author: Shaffran, Tyler
    // Dated Created: 2/18/2020
    // Last Modified: 2/23/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcome();
            DisplayMenu();
            DisplayProgramEnd();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenu()
        {

            Console.CursorVisible = true;
            bool quitApplication = false;
            string menuChoice;
            Finch finchRobot = new Finch();
            bool finchRobotConnected = false;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // user picks menu
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                Console.CursorVisible = true;
                menuChoice = Console.ReadLine().ToLower();
                
                //
                // switch/case for user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        finchRobotConnected = ConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        if (finchRobotConnected)
                        {
                            DisplayTalentShowMenuScreen(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Please make sure the robot is connected");
                            DisplayContinuePrompt();
                        }
                        break;
                        
                    case "c":
                        if (finchRobotConnected)
                        {
                            DataMenu(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Please make sure the robot is connected");
                            DisplayContinuePrompt();
                        }
                        break;

                    case "d":
                        if (finchRobotConnected)
                        {
                            AlarmMenu(finchRobot);
                        } 
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Please make sure the robot is connected");
                            DisplayContinuePrompt();
                        }

                        break;

                    case "e":
                        if (finchRobotConnected)
                        {
                            UserInputMenu(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Please make sure the robot is connected");
                            DisplayContinuePrompt();
                        }

                        break;

                    case "f":

                        DisconnectFinchRobot(finchRobot);
                        finchRobotConnected = false;
                        
                        break;

                    case "q":
                        if (finchRobotConnected)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Make sure the robot is disconnected before quitting");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            quitApplication = true;
                        }
                        
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;
            

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // menu for user choices
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Movement");
                Console.WriteLine("\tc) Song");
                Console.WriteLine("\td) Surprise");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                Console.CursorVisible = true;
                menuChoice = Console.ReadLine().ToLower();

                //
                // switch/case for user choices
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(myFinch);
                        break;

                    case "b":
                        DisplayMovement(myFinch);
                        break;

                    case "c":
                        SingSong(myFinch);
                        break;

                    case "d":
                        SurprisePerformance(myFinch);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show  Light and Sound                    *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            
            Console.CursorVisible = false;

            DisplayScreenHeader("Sound and Light Display");

            Console.WriteLine("\tThe Finch Robot will now display light for you");
            DisplayContinuePrompt();

            // light
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(1000);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(1000);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(1000);
            finchRobot.setLED(0, 0, 0);

            Console.WriteLine("");
            Console.WriteLine("\t The Finch will now make some sounds");
            DisplayContinuePrompt();

            //sound
            finchRobot.noteOn(523);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.noteOn(1047);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.noteOn(587);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.noteOn(988);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.noteOn(659);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.noteOn(880);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.noteOn(698);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.noteOn(784);
            finchRobot.wait(500);
            finchRobot.noteOff();


            //user input
           string color = GetUserColor();
           if (color == "red")
            {
                FinchRed(finchRobot);
            }
           else if ( color == "blue")
            {
                FinchBlue(finchRobot);
            }
           else if (color == "green")
            {
                FinchGreen(finchRobot);
            }
           else if (color == "surprise")
            {
                ColorSurprise(finchRobot);
            }

            else
            
            DisplayContinuePrompt();
            Console.Clear();

            // Light and Sound
            Console.CursorVisible = false;
            Console.WriteLine("\tThe Finch will now combine light and sound");
            DisplayContinuePrompt();
            finchRobot.setLED(200, 0, 0);
            finchRobot.noteOn(500);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0); 
            finchRobot.noteOff();

            finchRobot.setLED(0, 200, 0);
            finchRobot.noteOn(1000);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 200);
            finchRobot.noteOn(1500);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            finchRobot.setLED(200, 100, 50);
            finchRobot.noteOn(2000);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            finchRobot.setLED(50, 200, 100);
            finchRobot.noteOn(2500);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            finchRobot.setLED(100, 50, 200);
            finchRobot.noteOn(3000);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();



            DisplayMenuPrompt("Talent Show Menu");

            ResetFinch(finchRobot);

        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show Movement                  *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>

        static void DisplayMovement(Finch finchrobot)
        {
            DisplayScreenHeader("\t The Finch will now show you its movement capabilities");
            Console.CursorVisible = false;
            Console.WriteLine("");
            Console.WriteLine("\tForward and Backward");
            DisplayContinuePrompt();

            //forward speed up
            finchrobot.setLED(100, 100, 100);
            for (int i = 0; i < 200; i = i + 1)
            {
                finchrobot.setMotors(i, i);
            }
            finchrobot.wait(1500);
            finchrobot.setMotors(0, 0);
            finchrobot.setLED(0, 0, 0);
;
            //reverse speed up
            finchrobot.setLED(200, 200, 200);
            for (int i = 0; i > -200; i = i - 1)
            {
                finchrobot.setMotors(i, i);
            }

            finchrobot.wait(1500);
            finchrobot.setMotors(0, 0);
            finchrobot.setLED(0, 0, 0);

            Console.WriteLine("");
            Console.WriteLine("\t Three Point Turns");
            DisplayContinuePrompt();
            finchrobot.setLED(0, 150, 0);

            finchrobot.setMotors(50, 100);
            finchrobot.wait(3000);
            finchrobot.setMotors(-100, -50);
            finchrobot.wait(3000);
            finchrobot.setMotors(0, 0);

            finchrobot.setMotors(50, 100);
            finchrobot.wait(3000);
            finchrobot.setMotors(-100, -50);
            finchrobot.wait(3000);
            finchrobot.setMotors(0, 0);

            finchrobot.setMotors(50, 100);
            finchrobot.wait(3000);
            finchrobot.setMotors(-100, -50);
            finchrobot.wait(3000);
            finchrobot.setMotors(0, 0);
            finchrobot.setLED(0, 0, 0);

            Console.WriteLine("");
            Console.WriteLine("\t Duck-Walk");
            DisplayContinuePrompt();

            //Duck Walk Forwards

            finchrobot.setMotors(75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 75);
            finchrobot.wait(500);

            finchrobot.setMotors(75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 75);
            finchrobot.wait(500);

            finchrobot.setMotors(75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 75);
            finchrobot.wait(500);

            finchrobot.setMotors(75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 75);
            finchrobot.wait(500);

            finchrobot.setMotors(75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 75);
            finchrobot.wait(500);

            finchrobot.setMotors(75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 75);
            finchrobot.wait(500);

            //Duck Walk Backwards

            finchrobot.setMotors(-75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, -75);
            finchrobot.wait(500);

            finchrobot.setMotors(-75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, -75);
            finchrobot.wait(500);

            finchrobot.setMotors(-75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, -75);
            finchrobot.wait(500);

            finchrobot.setMotors(-75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, -75);
            finchrobot.wait(500);

            finchrobot.setMotors(-75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, -75);
            finchrobot.wait(500);

            finchrobot.setMotors(-75, 0);
            finchrobot.wait(500);
            finchrobot.setMotors(0, -75);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 0);
            finchrobot.setLED(0, 0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }
        /// <summary>
        /// *****************************************************************
        /// *               Talent Show Song                  *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void SingSong(Finch finchrobot)
        {
            Console.CursorVisible = true;
            DisplayScreenHeader("Finch Song");
            string userNum;
            int userNote;
            bool validResponse;
            do
            {
                Console.WriteLine("Enter a number between 500 and 5000 and " +
                    "the Finch will play you the corresponding note");
                userNum = Console.ReadLine();
                validResponse = int.TryParse(userNum, out userNote) && (userNote >= 500) && (userNote <= 5000);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Please enter a number between 500 and 5000");
                }
            }
            while (!validResponse);

            FinchSound(finchrobot, userNote);
            Console.CursorVisible = false;
            Console.WriteLine("");
            
            Console.WriteLine("The Finch will now play a song for you");
            Console.WriteLine("");
            DisplayContinuePrompt();

            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(620); //D#
            finchrobot.wait(1300);
            finchrobot.noteOff();

            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(580); //D
            finchrobot.wait(1300);
            finchrobot.noteOff();

            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(830);//G#
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(830);//G#
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(830);//G#
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(830);//G#
            finchrobot.wait(250);
            finchrobot.noteOff();

            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(580); //D
            finchrobot.wait(800);
            finchrobot.noteOff();

            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(620); //D#
            finchrobot.wait(800);
            finchrobot.noteOff();

            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(784); //G
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(700); //F
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(620); //D#
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(520); //C
            finchrobot.wait(250);
            finchrobot.noteOff();
            finchrobot.noteOn(784); //G
            finchrobot.wait(800);
            finchrobot.noteOff();


            DisplayMenuPrompt("Talent Show Menu");
        }
        /// <summary>
        /// *****************************************************************
        /// *               Finch Surprise                 *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void SurprisePerformance(Finch finchrobot)
        {
            DisplayScreenHeader("\tFinch Surprise");
            Console.WriteLine("");
            DisplayContinuePrompt();
            Console.CursorVisible = false;

            for (int green = 0; green < 200; green = green + 3)
            {
                finchrobot.setLED(0, green, 0);
                finchrobot.wait(10);
            }
            for (int sound = 0; sound < 4000; sound = sound + 100)
            {
                finchrobot.noteOn(sound);
                finchrobot.wait(100);
            }
            for (int move = 0; move < 250; move = move +20)
            {
                finchrobot.setMotors(move, move);
                finchrobot.wait(300);
            }
            
            ResetFinch(finchrobot);
            Console.WriteLine();
            DisplayContinuePrompt();

            finchrobot.setLED(100, 100, 100);
            finchrobot.noteOn(300);
            finchrobot.setMotors(150, -150);
            finchrobot.wait(500);
            finchrobot.setMotors(0, 0);
            finchrobot.setMotors(-150, 150);
            finchrobot.wait(500);
            ResetFinch(finchrobot);

            DisplayContinuePrompt();

            finchrobot.setMotors(-25, 25);
            finchrobot.setLED(100, 0, 50);
            finchrobot.noteOn(247);
            finchrobot.wait(750);
            finchrobot.noteOff();

            finchrobot.setMotors(-25, -25);
            finchrobot.setLED(15, 0, 200);
            finchrobot.noteOn(139);
            finchrobot.wait(750);
            finchrobot.noteOff();

            finchrobot.setMotors(25, -25);
            finchrobot.setLED(100, 200, 0);
            finchrobot.noteOn(139);
            finchrobot.wait(750);
            finchrobot.noteOff();

            finchrobot.setMotors(25, 25);
            finchrobot.setLED(10, 48, 230);
            finchrobot.noteOn(165);
            finchrobot.wait(750);
            finchrobot.noteOff();

            finchrobot.setMotors(-25, 25);
            finchrobot.setLED(0, 30, 150);
            finchrobot.noteOn(139);
            finchrobot.wait(750);
            finchrobot.noteOff();

            finchrobot.setMotors(-25, -25);
            finchrobot.setLED(100, 100, 100);
            finchrobot.noteOn(247);
            finchrobot.wait(750);
            finchrobot.noteOff();

            finchrobot.setMotors(25, 25);
            finchrobot.setLED(150, 15, 200);
            finchrobot.noteOn(208);
            finchrobot.wait(750);
            finchrobot.noteOff();
            ResetFinch(finchrobot);

            DisplayMenuPrompt("Talent Show Menu");

        }
        static void DataMenu(Finch finchrobot)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("\tFunction under maintenance, check back later");
            DisplayMenuPrompt("Main Menu");
        }
        static void AlarmMenu(Finch finchrobot)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("\tFunction under maintenance, check back later");
            DisplayMenuPrompt("Main Menu");
        }
        static void UserInputMenu(Finch finchrobot)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("\tFunction under maintenance, check back later");
            DisplayMenuPrompt("Main Menu");
        }
        #endregion

        #region FINCH ROBOT MANAGEMENT
        //**********************
        //Get user color display
        //**********************
        static string GetUserColor()
        {
            {
                Console.Clear();
                Console.CursorVisible = true;

                bool validResponse = false;
                string userResponse;

                do

                {
                    Console.Write("\tEnter the color \"red\" \"green\" \"blue\" or \"surprise\" " +
                        "and the finch will display it for you");
                    Console.WriteLine("");
                    userResponse = Console.ReadLine().ToLower();
                    switch (userResponse)

                    {

                        case "red":
                            validResponse = true;
                            break;

                        case "green":
                            validResponse = true;
                            break;
                        
                        case "blue":
                            validResponse = true;
                            break;

                        case "surprise":
                            validResponse = true;
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("\tPlease enter a valid color");

                            break;

                    }

                    

                } while (!validResponse);


                return userResponse;

            }
        }
        //**********************
        //User LED color choices
        //**********************

        //Display red led
        
        static void FinchRed(Finch finchRobot)
        {
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(2000);
            finchRobot.setLED(0, 0, 0);
        }

        //display green led
        static void FinchGreen(Finch finchRobot)
        {
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(2000);
            finchRobot.setLED(0, 0, 0);
        }
        //display blue led
        static void FinchBlue(Finch finchRobot)
        {
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(2000);
            finchRobot.setLED(0, 0, 0);
        }
        
        //display surpise color
        static void ColorSurprise(Finch finchRobot)
        {
            finchRobot.setLED(100, 255, 175);
            finchRobot.wait(2000);
            finchRobot.setLED(0, 0, 0);
        }

        //**********
        //User Sound
        //**********
        static void FinchSound(Finch finchRobot, int note)
        {
            finchRobot.noteOn(note);
            finchRobot.wait(1000);
            finchRobot.noteOff();
            
        }

        //***********
        //Reset Finch
        //***********
        static void ResetFinch(Finch finchRobot)
        {
            finchRobot.setMotors(0, 0);
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

        }


        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// 


       
        static void DisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;
            

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

           
            for (int sound = 5000; sound > 0; sound = sound - 100)
            {
                finchRobot.noteOn(sound);
                finchRobot.wait(50);
            }
            finchRobot.disConnect();
            
            
            Console.WriteLine("\tThe Finch robot is now disconnected.");

            DisplayMenuPrompt("Main Menu");
            
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool ConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tConnecting Finch Robot");
            Console.WriteLine("\tPlease make sure the usb cable is connected to your computer and the finch robot");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();
            for (int sound = 0; sound < 5000; sound = sound + 100)
            {
                finchRobot.noteOn(sound);
                finchRobot.wait(50);
            }

            ResetFinch(finchRobot);

            


            DisplayMenuPrompt("Main Menu");

           

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcome()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control Program");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayProgramEnd()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
