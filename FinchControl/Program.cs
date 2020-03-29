using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;
using System.Linq;
using System.Threading;
using System.Reflection;


namespace Project_FinchControl
{

    //------------------------
    //Enum for finch commands-
    //------------------------
    public enum Command
    {
        NONE,
        FORWARD,
        BACKWARD,
        STOPMOVING,
        WAIT,
        PLAYNOTE,
        NOTEOFF,
        RIGHTTURN,
        LEFTTURN,
        TURNONLED,
        TURNOFFLED,
        SURPRISE,
        GETLIGHTDATA,
        GETTEMPERATURE,
        ISFINCHLEVEL,
        AMIUPSIDEDOWN,
        DONE
    }

   

    // **************************************************
    //
    // Title: CIT Finch Control Project
    // Description: Application that controls different
    // functions of the finch             
    // Application Type: Console
    // Author: Shaffran, Tyler
    // Dated Created: 2/18/2020
    // Last Modified: 3/29/2020
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
            DisplayMenu();
            DisplayProgramEnd();
        }
        #region User Theme
        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            (ConsoleColor letterColor, ConsoleColor backgroundColor) userColors;
            bool colorsChosen = false;
            Console.CursorVisible = true;
            userColors = ReadColorSelection();
            Console.ForegroundColor = userColors.letterColor;
            Console.BackgroundColor = userColors.backgroundColor;
            Console.Clear();

            DisplayScreenHeader("Welcome to the Finch Control Program");
            Console.WriteLine();
            Console.WriteLine("\t The user may now set the colors for the rest of the program");
            Console.WriteLine();
            Console.WriteLine("\t The current colors are displayed below");
            Console.WriteLine();
            Console.WriteLine($"\t The current letter color is {Console.ForegroundColor}");
            Console.WriteLine($"\t The current background color is {Console.BackgroundColor}");
            Console.WriteLine();
            do
            {
                Console.Write("\t Would you like to change the current colors? Please enter yes or no: ");
                string userResponse = Console.ReadLine().ToLower();
                switch (userResponse)
                {
                    case "yes":
                        userColors.letterColor = GetLetterColor();
                        userColors.backgroundColor = GetBackgroundColor();
                        Console.ForegroundColor = userColors.letterColor;
                        Console.BackgroundColor = userColors.backgroundColor;
                        Console.Clear();
                        DisplayScreenHeader("New User Colors");
                        Console.WriteLine($"\t User has set new letter color to {Console.ForegroundColor}");
                        Console.WriteLine($"\t User has set new background color to {Console.BackgroundColor}");
                        Console.WriteLine();
                        Console.WriteLine("\t Is the user happy with this color selection?");
                        Console.Write("\t ");
                        if (Console.ReadLine().ToLower() == "yes")
                        {
                            colorsChosen = true;
                            WriteColorSelection(userColors.letterColor, userColors.backgroundColor);
                        }
                        break;

                    case "no":
                        colorsChosen = true;
                        break;

                    default:
                        Console.WriteLine("\t Please enter yes or no");
                        break;
                }


            } while (!colorsChosen);
        }

        private static ConsoleColor GetBackgroundColor()
        {
            Console.Clear();
            ConsoleColor userColor;
            int count = 1;
            bool validColor;
            Console.CursorVisible = true;
            DisplayScreenHeader("New Background Color");
            Console.WriteLine("\t   Valid colors are:");
            
            foreach (string colorOptions in Enum.GetNames(typeof(ConsoleColor)))
            {   
                Console.WriteLine($"\t\t{count}: {colorOptions.ToLower()}");
                count++;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine("\t Enter a new background color");
                Console.Write("\t ");
                validColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out userColor);

                if (!validColor)
                {
                    Console.WriteLine("\t That is not a valid color, Please try again");
                }
                else
                {
                    validColor = true;
                }
            } while (!validColor);
            string exceptionMessage;
            string catchColor = userColor.ToString();
            catchColor = CatchException(out exceptionMessage);
            Console.WriteLine();
            Console.WriteLine($"\t *{exceptionMessage}*");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to continue");
            Console.ReadKey();
            return userColor;
        }

        static ConsoleColor GetLetterColor()
        {
            Console.CursorVisible = true;
            Console.Clear();
            ConsoleColor userColor;
            int count = 1;
            bool validColor;
            DisplayScreenHeader("New Letter Color");
            Console.WriteLine("\t   Valid colors are:");
            foreach (string colorOptions in Enum.GetNames(typeof(ConsoleColor)))
            {
                Console.WriteLine($"\t\t {count}: {colorOptions.ToLower()}");
                count++;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine("\t Enter a new letter color");
                Console.Write("\t ");
                validColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out userColor);

                if (!validColor)
                {
                    Console.WriteLine("\t That is not a valid color, Please try again");
                }
                else
                {
                    validColor = true;
                }
            } while (!validColor);
            string exceptionMessage;
            string catchColor = userColor.ToString();
            catchColor = CatchException(out exceptionMessage);
            Console.WriteLine();
            Console.WriteLine($"\t *{exceptionMessage}*");
            Console.WriteLine();
            Console.WriteLine("\t Pressy any key to continue");
            Console.ReadKey();
            return userColor;
        }

        static string CatchException(out string exceptionMessage)
        {
            string path = @"Data/Colorchoice.txt";
            string userColor;

            try
            {
                userColor = File.ReadAllText(path);
                exceptionMessage = "Color successfully changed";
            }
            catch (DirectoryNotFoundException)
            {
                exceptionMessage = "Cannot find folder for the text file";
            }
            catch (FileNotFoundException)
            {
                exceptionMessage = "Cannot locate the requested file";
            }
            catch (Exception)
            {
                exceptionMessage = "Unable to read text file";
            }
            return exceptionMessage;
        }

        static (ConsoleColor letterColor, ConsoleColor backgroundColor) ReadColorSelection()
        {
            string dataPath = @"Data/Colorchoice.txt";
            string[] colorChoice;
            ConsoleColor letterColor;
            ConsoleColor backgroundColor;

            colorChoice = File.ReadAllLines(dataPath);
            Enum.TryParse(colorChoice[0], true, out letterColor);
            Enum.TryParse(colorChoice[1], true, out backgroundColor);

            return (letterColor, backgroundColor);
        }

        static void WriteColorSelection(ConsoleColor letterColor, ConsoleColor backgroundColor)
        {
            string dataPath = @"Data/Colorchoice.txt";
            string color = letterColor.ToString();
            string bColor = backgroundColor.ToString();
            File.WriteAllText(dataPath, color + "\n" + bColor);

        }
        #endregion
        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>

        #region Main Menu
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
                Console.WriteLine("\tf) Change Color Theme ");
                Console.WriteLine("\tg) Disconnect Finch Robot");
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
                        SetTheme();
                        break;

                    case "g":
                        DisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }
        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
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
                        DisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        DisplayMovement(finchRobot);
                        break;

                    case "c":
                        SingSong(finchRobot);
                        break;

                    case "d":
                        SurprisePerformance(finchRobot);
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
        #endregion

        #region Data Recorder
        static void DataMenu(Finch finchRobot)
        {
            int dataPoints =0;
            double frequencyofPoints =0;
            double[] temperature = null;
            bool quitDataRecorderMenu = false;
            string menuChoice;
            Console.CursorVisible = true;
            int lightData = 0;
            double lightFrequency = 0;
            double[] ambientLight = null;
            

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // menu for user choices
                //
                Console.WriteLine("\ta) Number Of Temperature Collections");
                Console.WriteLine("\tb) Temperature Gathering Frequency");
                Console.WriteLine("\tc) Gather Temperature Data");
                Console.WriteLine("\td) Data Display - Temperature");
                Console.WriteLine("\te) Number of Light Collections");
                Console.WriteLine("\tf) Light Gathering Frequency");
                Console.WriteLine("\tg) Gather Light Data");
                Console.WriteLine("\th) Data Display - Light");
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
                        dataPoints = GetUserNumberOfDataPoints();
                        break;

                    case "b":
                        frequencyofPoints = GetUserFrequencyOfDataCollection();
                        break;

                    case "c":
                        temperature = ReceiveData(dataPoints, frequencyofPoints, finchRobot);
                        break;

                    case "d":
                        DisplayDataForUser(temperature);
                        break;

                    case "e":
                        lightData = GetUserInputForLightData();
                        break;

                    case "f":
                        lightFrequency = GetUserLightFrequency();
                        break;

                    case "g":
                        ambientLight = GetUserInputForLightData(lightData, lightFrequency, finchRobot);
                        break;

                    case "h":
                        DispalyLightData(ambientLight);
                        break;
                                             

                    case "q":
                        quitDataRecorderMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitDataRecorderMenu);
        }

        /// <summary>
        /// Displays Light Data
        /// </summary>
        /// <param name="ambientLight"></param>

        private static void DispalyLightData(double[] ambientLight)
        {
            DisplayScreenHeader("Light Data Display");
            Console.WriteLine();
            Console.WriteLine("\t The Finch will now display the light readings from 0(dark) to 255(bright)");
            Console.WriteLine();
            DisplayContinuePrompt();

            Console.WriteLine("\tAmbient Light".PadLeft(20));
            Console.WriteLine("************".PadLeft(20));
            foreach (double light in ambientLight)
                Console.WriteLine(light.ToString().PadLeft(15));
            
           

            DisplayContinuePrompt();

            bool validResponse = false;
            string userResponse;
            do

            {
                Console.Write("\tEnter the way in which the finch will display the light data \"sort\" \"sum\" \"average\"  " +
                    "for you");
                Console.WriteLine();
                Console.Write("\t");
                userResponse = Console.ReadLine().ToLower();
                switch (userResponse)

                {

                    case "sort":
                        validResponse = true;
                        Array.Sort(ambientLight);
                        for (int i = 0; i < ambientLight.Length; i++)
                            Console.WriteLine("\t" + ambientLight[i].ToString("n0"));
                        Console.WriteLine();
                        


                        break;
                    case "sum":
                        validResponse = true;
                        double sum = ambientLight.Sum();
                        Console.WriteLine("\t The sum of the gathered light data is {0}", sum.ToString("n0"));
                        
                        break;

                    case "average":
                        validResponse = true;
                        double average = ambientLight.Average();
                        Console.WriteLine("\t Average: {0}", average.ToString("n0"));
                        Console.WriteLine();
                        
                        break;



                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a valid response");

                        break;

                }



            } while (!validResponse);

            DisplayContinuePrompt();
            
        }

        /// <summary>
        /// Records Light Data
        /// </summary>
        /// <param name="lightData"></param>
        /// <param name="lightFrequency"></param>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        private static double[] GetUserInputForLightData(int lightData, double lightFrequency, Finch finchRobot)
        {
            double[] ambientLight = new double[lightData];
            DisplayScreenHeader("Finch will now gather light data");
            // ask user to continue validate string
            Console.WriteLine($"\tNumber of light readings for the Finch to gather per user request {lightData}");
            Console.WriteLine();
            Console.WriteLine($"\tFrequency of light reading collection per user request {lightFrequency}");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot is ready to gather the light information that you have requested");
                
            DisplayContinuePrompt();

            for (int i = 0; i < lightData; i++)
            {
                ambientLight[i] = finchRobot.getRightLightSensor();
                Console.WriteLine($"\tlight reading {i + 1} is {ambientLight[i].ToString("n0")}");
                int multSec = Convert.ToInt32(lightFrequency);
                finchRobot.wait(multSec * 1000);

            }



            DisplayContinuePrompt();
            return ambientLight;
        }
        /// <summary>
        /// Records Frequency of light gathering
        /// </summary>
        /// <returns>Light gathering Frequency</returns>
        static double GetUserLightFrequency()
        {
            DisplayScreenHeader("Frequency of Light Collection");

            double frequencyOfLightGather;
            string userResponse;
            bool validResponse;
            do
            {
                Console.WriteLine("\tEnter the frequency in which you wish the " +
                    "Finch to gather light data for you (in seconds) ");
                Console.WriteLine();
                Console.Write("\t");

                userResponse = Console.ReadLine();
                validResponse = double.TryParse(userResponse, out frequencyOfLightGather);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\tPlease enter an integer number");
                }
            }
            while (!validResponse);

            DisplayContinuePrompt();



            return frequencyOfLightGather;
        }

        /// <summary>
        /// Gets the number of light readings
        /// </summary>
        /// <returns>Light Readings</returns>
        static int GetUserInputForLightData()
        {
            int userLightData;
            string userEntry;
            bool validResponse;

            DisplayScreenHeader("Number Of Light Readings For The Finch To Gather");
            do
            {
                Console.WriteLine("\tEnter the number of light readings you would like " +
                    "the finch to gather");
                Console.WriteLine();
                Console.Write("\t");

                userEntry = Console.ReadLine();
                validResponse = int.TryParse(userEntry, out userLightData);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\tPlease enter an integer number");
                }
            }
            while (!validResponse);

            DisplayContinuePrompt();

            return userLightData;

        }
        /// <summary>
        /// Displays temperature data
        /// </summary>
        /// <param name="temperature"></param>
        private static void DisplayDataForUser(double[] temperature)
        {
            DisplayScreenHeader("Graph of Temperature Data");
            Console.WriteLine();
            Console.WriteLine("\t The Finch will now display the temperature readings in degrees Fahrenheit");
            Console.WriteLine();
            DisplayContinuePrompt();
          
            Console.WriteLine(
                "\tRecording Number".PadLeft(15) +
                "Temperatures" .PadLeft(15) 
                );
            Console.WriteLine(
               "\t***************".PadLeft(15) +
               "**********".PadLeft(15)
               );
            for (int i = 0; i < temperature.Length; i++)
            {
                Console.WriteLine(("\t") + (i + 1).ToString("n0").PadLeft(10) +
                    (temperature[i] * 1.8 + 32).ToString("#°F").PadLeft(15));
            }

            DisplayContinuePrompt();
            bool validResponse = false;
            string userResponse;
            Console.Clear();
            DisplayScreenHeader("Array Display");

            do

            {
                Console.Write("\tEnter the way in which the finch will display the data \"sort\" \"sum\" \"average\"  " +
                    "and the finch will display it for you");
                Console.WriteLine("");
                Console.Write("\t");
                userResponse = Console.ReadLine().ToLower();
                switch (userResponse)

                {

                    case "sort":
                        validResponse = true;
                        Array.Sort(temperature);
                        for (int i = 0; i < temperature.Length; i++)
                            Console.WriteLine("\t" + temperature[i].ToString("n0"));
                        
                        

                        
                        break;
                    case "sum":
                        validResponse = true;
                        double sum = temperature.Sum();
                        double degF = CelsiusToFahrenheit(sum);
                        Console.WriteLine("\t The sum of temperatures in Fahrenheit is {0}", degF.ToString("#°F"));
                        
                        break;

                    case "average":
                        validResponse = true;
                        double average = temperature.Average();
                        double avgF = CelsiusToFahrenheit(average);
                        Console.WriteLine("\t Average in Fahrenheit: {0}", avgF.ToString("#°F"));
                        Console.WriteLine();
                        
                        break;

                    

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a valid response");

                        break;

                }

                

            } while (!validResponse);
            DisplayContinuePrompt();
        }

        static double CelsiusToFahrenheit(double userInput)
        {
            
            double degC = Convert.ToDouble(userInput);
            double degF = (degC * 1.8) + 32;
            return degF;
        }

        
        /// <summary>
        /// Takes Temperature Data
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <param name="frequencyofPoints"></param>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        static double[] ReceiveData(int dataPoints, double frequencyofPoints, Finch finchRobot)
        {
            double[] temperature = new double[dataPoints];
            DisplayScreenHeader("Finch will now gather data");
            
            Console.WriteLine("\tNumber of data points for the Finch to gather per user request {0}", dataPoints);
            Console.WriteLine();
            Console.WriteLine("\tFrequency of data point collection per user request {0}", frequencyofPoints);
            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot is ready to gather the temperature information that you have requested");
               
            DisplayContinuePrompt();

            for (int i = 0; i < dataPoints; i++)
            {
                temperature[i] = finchRobot.getTemperature();
                Console.WriteLine($"\tTemp reading {i + 1} is {temperature[i].ToString("n0")}");
                int multSec = Convert.ToInt32(frequencyofPoints);
                finchRobot.wait(multSec * 1000);
                    
            }

                
 
            DisplayContinuePrompt();
            return temperature;


        }



        /// <summary>
        /// Gets the frequency in which user wants to collect data points
        /// </summary>
        /// <returns>data point frequency</returns>

        static double GetUserFrequencyOfDataCollection()
        {
            DisplayScreenHeader("Frequency of Temperature Collection");

            double frequencyOfDataPoints;
            string userResponse;
            bool validResponse;
            do
            {
                Console.WriteLine("\tEnter the frequency in which you wish the " +
                    "Finch to gather temperature data for you (in seconds) ");
                Console.WriteLine();
                Console.Write("\t");

                userResponse = Console.ReadLine();
                validResponse = double.TryParse(userResponse, out frequencyOfDataPoints);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\tPlease enter an integer number");
                }
            }
            while (!validResponse);

            DisplayContinuePrompt();
            


            return frequencyOfDataPoints;

            
        }

        /// <summary>
        /// Gets users number of data points they wish to see
        /// </summary>
        /// <returns>user data points</returns>
        static int GetUserNumberOfDataPoints()
        {
            int userDataPoints;
            string userEntry;
            bool validResponse;

            DisplayScreenHeader("Number Of Data Points for Finch To Gather");
            do
            {
                Console.WriteLine("\tEnter the number of data points you would like " +
                    "the finch to gather");
                Console.WriteLine();
                Console.Write("\t");

                userEntry = Console.ReadLine();
                validResponse = int.TryParse(userEntry, out userDataPoints);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\tPlease enter an integer number");
                }
            }
            while (!validResponse);

            DisplayContinuePrompt();

            return userDataPoints;
        }
        #endregion

        #region Alarm System
        /// <summary>
        /// Light Alarm Menu 
        /// </summary>
        /// <param name="finchRobot"></param>
        static void AlarmMenu(Finch finchRobot)
        {
           Console.CursorVisible = true;
           bool quitAlarmMenu = false;
           string menuChoice;
           string sensorChoice = "";
           string minOrMax = "";
           int threshHoldValue = 0;
           int monitorTime = 0;
           string tempChoice = "";
           string tempThreshold = "";
           int aBValue = 0;
           int tempMonitorTime = 0;

            do
            {
              DisplayScreenHeader("Alarm Menu");
                    //
                    // menu for user choices
                    //
                Console.WriteLine("\ta) Choose Sensors");
                Console.WriteLine("\tb) Set Minimum or Maximum Light Value");
                Console.WriteLine("\tc) Set Light Threshold Value");
                Console.WriteLine("\td) Time to Monitor Light");
                Console.WriteLine("\te) Begin Light Monitioring");
                Console.WriteLine();
                Console.WriteLine("\tf) Temperature Monitoring");
                Console.WriteLine("\tg) Set Minimum or Maximum Temperature Value");
                Console.WriteLine("\th) Set Temperature Threshold Value");
                Console.WriteLine("\ti) Time to Monitor Temperature");
                Console.WriteLine("\tj) Begin Temperature Monitioring");
                Console.WriteLine();
                Console.WriteLine("\tk) Monitor Both Temperature and Light");
                Console.WriteLine();
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
                        sensorChoice = ChooseSensorToMonitor();
                        break;
                        
                    case "b":
                        minOrMax = ChooseMinOrMax();    
                        break;

                     case "c":
                        threshHoldValue = GetThreshHoldValue(minOrMax, finchRobot);
                        break;

                    case "d":
                        monitorTime = GetMonitorTime(minOrMax, threshHoldValue);
                        break;

                    case "e":
                        if (threshHoldValue == 0 || monitorTime == 0 || String.IsNullOrEmpty(sensorChoice) || String.IsNullOrEmpty(minOrMax))
                        {
                            DisplayErrorMessage();
                        }
                        else 
                        BeginLightAlarmMonitoring(sensorChoice, minOrMax, threshHoldValue, monitorTime, finchRobot);
                        break;

                    case "f":
                        tempChoice = GetTempChoice();
                        break;

                    case "g":
                        tempThreshold = GetTempThreshold(tempChoice, finchRobot);
                        break;

                    case "h":
                        aBValue = GetABValue(tempChoice, finchRobot);

                        break;
                    case "i":
                        tempMonitorTime = GetTempMonitorTime();
                        break;
                    case "j":
                        if (tempMonitorTime == 0 || aBValue == 0 || String.IsNullOrEmpty(tempChoice) || String.IsNullOrEmpty(tempThreshold))
                        {
                            DisplayErrorMessage();
                        }
                        else 
                        BeginTermperatureMonitoring(tempChoice, tempThreshold, aBValue, tempMonitorTime, finchRobot);
                        break;
                    
                    case "k":
                        if (tempMonitorTime == 0 || aBValue == 0 || String.IsNullOrEmpty(tempChoice) || String.IsNullOrEmpty(tempThreshold) ||
                            threshHoldValue == 0 || monitorTime == 0 || String.IsNullOrEmpty(sensorChoice) || String.IsNullOrEmpty(minOrMax))
                        {
                            DisplayErrorMessage();
                        }
                        else
                        MonitorLightAndTemp(sensorChoice, minOrMax, threshHoldValue, monitorTime, tempChoice, tempThreshold, aBValue, tempMonitorTime, finchRobot);
                        break;

                    case "q":
                             quitAlarmMenu = true;
                             break;

                        default:
                              Console.WriteLine();
                              Console.WriteLine("\t Please enter a letter for the menu choice.");
                              DisplayContinuePrompt();
                              break;
                 }

            } while (!quitAlarmMenu);
            
        }
        /// <summary>
        /// monitors temperature and light at the same time
        /// </summary>
        /// <param name="sensorChoice"></param>
        /// <param name="minOrMax"></param>
        /// <param name="threshHoldValue"></param>
        /// <param name="monitorTime"></param>
        /// <param name="tempChoice"></param>
        /// <param name="tempThreshold"></param>
        /// <param name="aBValue"></param>
        /// <param name="tempMonitorTime"></param>
        /// <param name="finchRobot"></param>
        static void MonitorLightAndTemp(string sensorChoice, 
            string minOrMax, 
            int threshHoldValue, 
            int monitorTime, 
            string tempChoice, 
            string tempThreshold, 
            int aBValue, 
            int tempMonitorTime, 
            Finch finchRobot)
        {
            int seconds = 0;
            bool overThreshHold = false;
            double currentTempValue = 0;
            int currentLightValue = 0;
            DisplayScreenHeader("Begin Monitoring Temperature and Light");

            Console.WriteLine($"\t Monitoring temperature sensor for {tempMonitorTime} second(s) in {tempChoice}");
            Console.WriteLine($"\t Alarm will sound if the temperature crosses the {tempThreshold} threshold of {aBValue} {tempChoice}");
            Console.WriteLine($"\t Monitoring the {sensorChoice} light sensors(s) {tempMonitorTime} second(s)");
            Console.WriteLine($"\t Alarm will sound if the light exceeds the {minOrMax} threshold of {threshHoldValue}");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to start monitoring temperature and light");
            Console.ReadKey();
            Console.WriteLine();
            do
            {
                switch (tempChoice)
                {
                    case "fahrenheit":
                        currentTempValue = finchRobot.getTemperature();
                        double tempF = CelsiusToFahrenheit(currentTempValue);
                        Console.Write($"\t The current temp is {tempF} {tempChoice}");
                        break;
                    case "celsius":
                        currentTempValue = finchRobot.getTemperature();
                        Console.Write($"\t The current temperature is {currentTempValue.ToString("n2")} {tempChoice}");
                        break;

                    default:
                        break;
                }
                switch (sensorChoice)
                {
                    case "left":
                        currentLightValue = finchRobot.getLeftLightSensor();
                        Console.WriteLine($"\t current light value is {currentLightValue}".PadLeft(20));
                        break;
                    case "right":
                        currentLightValue = finchRobot.getRightLightSensor();
                        Console.WriteLine($"\t current light value is {currentLightValue}".PadLeft(20));
                        break;
                    case "both":
                        currentLightValue = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                        Console.WriteLine($"\t current light value is {currentLightValue}".PadLeft(20));
                        break;
                }
                switch (tempThreshold)
                {
                    case "below":
                        if (currentTempValue < aBValue)
                        {
                            overThreshHold = true;

                        }
                        break;

                    case "above":
                        if (currentTempValue > aBValue)
                        {
                            overThreshHold = true;

                        }
                        break;
                }
               
                switch (minOrMax)
                {
                    case "minimum":
                        if (currentLightValue < threshHoldValue)
                        {
                            overThreshHold = true;

                        }
                        break;

                    case "maximum":
                        if (currentLightValue > threshHoldValue)
                        {
                            overThreshHold = true;

                        }
                        break;
                }

                finchRobot.wait(1000);
                seconds++;
            } while (!overThreshHold && seconds < tempMonitorTime);

            if (overThreshHold)
            {
                Console.WriteLine();
                Console.WriteLine($"\t The {tempThreshold} of {aBValue} {tempChoice} threshold has been exceeded");
                Console.WriteLine($"\t Or the light {minOrMax} value of {threshHoldValue} has been exceeded");
                finchRobot.setLED(200, 0, 0);
                finchRobot.noteOn(1000);
                finchRobot.wait(3000);
                ResetFinch(finchRobot);
                DisplayContinuePrompt();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"\t The temperature has not gone {tempThreshold} {aBValue} {tempChoice} ");
                Console.WriteLine($"\t The light value has not exceeded the {minOrMax} value of {threshHoldValue}");
                finchRobot.setLED(0, 200, 0);
                finchRobot.wait(2000);
                ResetFinch(finchRobot);

                DisplayContinuePrompt();
            }
            DisplayMenuPrompt("Alarm Menu");
        }

        static void DisplayErrorMessage()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("\t You have forgetten to enter some variables into the alarm menu");
            Console.WriteLine("\t The Finch cannot gather data without all of the variables");
            Console.WriteLine("\t Please go back to the alarm menu and re-enter the variables");
            DisplayMenuPrompt("Alarm Menu");
        }

        /// <summary>
        /// begins monitoring temperature
        /// </summary>
        /// <param name="tempChoice"></param>
        /// <param name="tempThreshold"></param>
        /// <param name="aBValue"></param>
        /// <param name="tempMonitorTime"></param>
        /// <param name="finchRobot"></param>
        private static void BeginTermperatureMonitoring(string tempChoice, 
            string tempThreshold, 
            int aBValue, 
            int tempMonitorTime, 
            Finch finchRobot)
        {
           
            DisplayScreenHeader("Temperature Monitoring");
          
            int seconds = 0;
            bool overThreshHold = false;
            double currentTempValue = 0;

            Console.WriteLine($"\t Monitoring temperature sensor for {tempMonitorTime} second(s) in {tempChoice}");
            Console.WriteLine($"\t Alarm will sound if the temperature crosses the {tempThreshold} threshold of {aBValue} {tempChoice}");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to start monitoring temperature");
            Console.ReadKey();
            Console.WriteLine();
            do
            {
                switch (tempChoice)
                {
                    case "fahrenheit":
                        currentTempValue = finchRobot.getTemperature();
                        Console.WriteLine("\t The current temp is " + CelsiusToFahrenheit(currentTempValue)  +  tempChoice);
                        break;

                    case "celsius":
                        currentTempValue = finchRobot.getTemperature();
                        Console.WriteLine($"\t The current temperature is {currentTempValue.ToString("n2")} {tempChoice}");
                        break;

                    default:
                        break;
                }
                switch (tempThreshold)
                {
                    case "below":
                        if (currentTempValue < aBValue)
                        {
                            overThreshHold = true;

                        }
                        break;

                    case "above":
                        if (currentTempValue > aBValue)
                        {
                            overThreshHold = true;

                        }
                        break;
                }

                finchRobot.wait(1000);
                seconds++;
            } while (!overThreshHold && seconds < tempMonitorTime);
           
            if (overThreshHold)
            {
                Console.WriteLine();
                Console.WriteLine($"\t The  temperature has crossed the threshold of {aBValue} {tempChoice}d");
                finchRobot.setLED(200, 0, 0);
                finchRobot.noteOn(1000);
                finchRobot.wait(3000);
                ResetFinch(finchRobot);

                DisplayContinuePrompt();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"\t The temperature has not gone {tempThreshold} {aBValue} {tempChoice} ");
                finchRobot.setLED(0, 200, 0);
                finchRobot.wait(2000);
                ResetFinch(finchRobot);

                DisplayContinuePrompt();
            }
            DisplayMenuPrompt("Alarm Menu");



        }
        /// <summary>
        /// Gets the time to monitor the temperature
        /// </summary>
        /// <returns></returns>
        static int GetTempMonitorTime()
        {
            int monitorTemp; 
            string userResponse;
            bool validResponse;

            DisplayScreenHeader("Set the Length of Monitoring");

            do
            {
                Console.WriteLine("\t Choose how long you wish to monitor the temperature in seconds");
                Console.WriteLine("\t User may select any whole number integer");
                Console.WriteLine();
                Console.Write("\t ");
                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out monitorTemp);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter an integer number");
                }
            }
            while (!validResponse);
            Console.WriteLine($"\t User has chosen a monitoring time of {monitorTemp} seconds");

            DisplayMenuPrompt("Alarm Menu");

            return monitorTemp;
        }

        /// <summary>
        /// gets the threshold to monitor from the user
        /// </summary>
        /// <param name="tempThreshold"></param>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        static int GetABValue(string tempChoice, Finch finchRobot)
        {

            int monitorTemp;
            string userResponse;
            bool validResponse;

            DisplayScreenHeader("Set the Temperature Threshold");

            do
            {
                Console.WriteLine("\t Choose the temperature to set as the threshold for alarm");
                Console.WriteLine("\t User may select any whole number integer");
                double currentTemp = finchRobot.getTemperature();
                if (tempChoice == "fahrenheit")
                {
                    double tempF = CelsiusToFahrenheit(currentTemp);
                    Console.WriteLine($"\t The current temperature is {tempF} {tempChoice}");
                }
                else
                {
                    Console.WriteLine($"\t The current temperature is {currentTemp.ToString("n2")} {tempChoice}");
                }
                Console.WriteLine();
                Console.Write("\t ");
                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out monitorTemp);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter an integer number");
                }
            }
            while (!validResponse);
            Console.WriteLine($"\t User has chosen a threshold value of {monitorTemp} {tempChoice} ");

            DisplayMenuPrompt("Alarm Menu");

            return monitorTemp;
        }



        /// <summary>
        /// gets user temp threshold
        /// </summary>
        /// <returns></returns>
        static string GetTempThreshold(string tempChoice,Finch finchRobot)
        {
            DisplayScreenHeader("Set Temperature Monitoring");
            
            string aboveBelow;
            bool validResponse = false;
            
            do
            {
                double currentTemp = finchRobot.getTemperature();
                if (tempChoice == "fahrenheit")
                {
                    double tempF = CelsiusToFahrenheit(currentTemp);
                    Console.WriteLine($"\t The current temperature is {tempF} {tempChoice}");
                }
                if (tempChoice == "celsius")
                {
                    Console.WriteLine($"\t The current temperature in celsius is {currentTemp.ToString("n2")} {tempChoice}");
                }
                
                Console.WriteLine();
                Console.WriteLine($"\t Select whether you want to monitor if temperature goes above or below ambient temperature");
                Console.WriteLine("\t You may choose \"above\" or \"below\"");
                Console.Write("\t ");
                aboveBelow = Console.ReadLine().ToLower();
                
                switch (aboveBelow)
                {
                    case "above":
                        validResponse = true;
                        break;

                    case "below":
                        validResponse = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t Please enter a valid response");
                        break;

                }



            } while (!validResponse);
            Console.WriteLine($"\t User has chosen {aboveBelow} ambient temperature");
            DisplayMenuPrompt("Alarm Menu");
            return aboveBelow;
        }

        /// <summary>
        /// Gets users temperature display choice
        /// </summary>
        /// <returns></returns>
        static string GetTempChoice()
        {
            bool validResponse = false;
            string tempChoice;
            DisplayScreenHeader("Fahrenheit or Celsius");

            do

            {
                Console.WriteLine("\t Choose how the temperature is displayed");
                Console.WriteLine("\t You may choose \"fahrenheit\" or \"celsius\"");
                Console.WriteLine();
                Console.Write("\t ");
                tempChoice = Console.ReadLine().ToLower();
                switch (tempChoice)

                {

                    case "fahrenheit":
                        validResponse = true;

                        break;

                    case "celsius":
                        validResponse = true;

                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t Please enter a valid response");

                        break;

                }

                Console.WriteLine($"\t User has chosen {tempChoice}");


            } while (!validResponse);
            DisplayMenuPrompt("Alarm Menu");
            return tempChoice;
            
        }

        static void BeginLightAlarmMonitoring(string sensorChoice, 
            string minOrMax, 
            int threshHoldValue, 
            int monitorTime, 
            Finch finchRobot)

        {
            DisplayScreenHeader("Begin Monitoring Alarm");
            int seconds = 0;
            bool overThreshHold = false;
            int currentLightValue;
            currentLightValue = 0;

            Console.WriteLine($"\t Monitoring {sensorChoice} sensor(s) for {monitorTime} second(s)");
            Console.WriteLine($"\t with a {minOrMax} value of {threshHoldValue}");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to start monitoring light values");
            Console.ReadKey();
            Console.WriteLine();

            while ((seconds < monitorTime) && !overThreshHold)
                
            {
                switch (sensorChoice)
                {
                    case "left":
                        currentLightValue = finchRobot.getLeftLightSensor();
                        Console.WriteLine($"\t current light value is {currentLightValue}");
                        break;
                    case "right":
                        currentLightValue = finchRobot.getRightLightSensor();
                        Console.WriteLine($"\t current light value is {currentLightValue}");
                        break;
                    case "both":
                        currentLightValue = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                        Console.WriteLine($"\t current light value is {currentLightValue}");
                        break;
                        
                }
                switch (minOrMax)
                {
                    case "minimum":
                        if (currentLightValue < threshHoldValue)
                        {
                            overThreshHold = true;
                            
                        }
                        break;

                    case "maximum":
                        if (currentLightValue > threshHoldValue)
                        {
                            overThreshHold = true;

                        }
                        break;
                        
                }
                
                finchRobot.wait(1000);
                seconds++;
            }




            if (overThreshHold)
            {
                Console.WriteLine();
                Console.WriteLine($"\t The {minOrMax} light threshold has been exceeded");
                finchRobot.setLED(200, 0, 0);
                finchRobot.noteOn(1000);
                finchRobot.wait(3000);
                ResetFinch(finchRobot);
                DisplayContinuePrompt();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"\t The {minOrMax} light threshold has not been exceeded");
                finchRobot.setLED(0, 200, 0);
                finchRobot.wait(2000);
                ResetFinch(finchRobot);

                DisplayContinuePrompt();
            }
            DisplayMenuPrompt("Alarm Menu");
        }

        static int GetMonitorTime(string minOrMax, int threshHoldValue)
        {
            int monitorTime;
            bool validResponse;
            string userResponse;
            DisplayScreenHeader("Set Time to Monitor");

            Console.WriteLine();
            Console.WriteLine($"\t Enter the time you wish to monitor the {minOrMax} threshold value of {threshHoldValue} in seconds");
            do
            {

                Console.WriteLine();
                Console.Write("\t");

                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out monitorTime);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter a whole integer number");
                }
            }
            while (!validResponse);
            Console.WriteLine($"\t User has chosen {monitorTime} seconds ");


            DisplayMenuPrompt("Alarm Menu");

            return monitorTime;
        }

        /// <summary>
        /// Gets the min/max threshold value set by the user
        /// </summary>
        /// <param name="minOrMax"></param>
        /// <param name="finchrobot"></param>
        /// <returns></returns>
        static int GetThreshHoldValue(string minOrMax, Finch finchrobot)
        {
            string userResponse;
            bool validResponse;
            int thresholdValue;
            DisplayScreenHeader("Threshold Value Selection");

            Console.WriteLine($"\t Current Ambient light values; Left - {finchrobot.getLeftLightSensor()} Right - {finchrobot.getRightLightSensor()}");
            Console.WriteLine();

            Console.WriteLine($"\t Choose the {minOrMax} sensor value");
            do
            {
               
                Console.WriteLine();
                Console.Write("\t");

                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out thresholdValue);
                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter an integer number");
                }
            }
            while (!validResponse);
            Console.WriteLine($"\t User has chosen a threshold value of {thresholdValue}");

            DisplayMenuPrompt("Alarm Menu");

            return thresholdValue;


        }


        /// <summary>
        /// Gets min or max
        /// </summary>
        /// <returns> minOrMax </returns>
        static string ChooseMinOrMax()
        {
            DisplayScreenHeader("\t Sensor Choice");
            bool validResponse = false;
            string minOrMax;

            do

            {
                Console.WriteLine("\t Set a minimum or maximum threshold to monitor");
                Console.WriteLine("\t You may choose \"minimum\" or \"maximum\"");
                Console.WriteLine();
                Console.Write("\t ");
                minOrMax = Console.ReadLine().ToLower();
                switch (minOrMax)

                {

                    case "minimum":
                        validResponse = true;

                        break;

                    case "maximum":
                        validResponse = true;

                        break;
                  
                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t Please enter a valid response");

                        break;

                }


            } while (!validResponse);


            Console.WriteLine($"\t User has chosen {minOrMax}");


            DisplayMenuPrompt("Alarm Menu");
            return minOrMax; 
        }
        /// <summary>
        /// Gets Sensor to monitor
        /// </summary>
        /// <returns>sensor choice</returns>
        static string ChooseSensorToMonitor()
        {
            DisplayScreenHeader("\t Sensor Choice");
            bool validResponse = false;
            string leftRightBoth;
            
            
            
            do

            {
                Console.WriteLine("\t Choose the sensor to monitor");
                Console.WriteLine("\t You may choose \"left\" \"right\" or \"both\"");
                Console.WriteLine();
                Console.Write("\t ");
                leftRightBoth = Console.ReadLine().ToLower();
                switch (leftRightBoth)

                {

                    case "left":
                        validResponse = true;
                         
                        break;

                    case "right":
                        validResponse = true;
                        
                        break;

                    case "both":
                        validResponse = true;
                        
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t Please enter a valid response");

                        break;

                }



            } while (!validResponse);



            Console.WriteLine($"\t User has chosen {leftRightBoth} sensor(s)");
            Console.WriteLine();
            DisplayMenuPrompt("Alarm Menu");
            return leftRightBoth;
        }
        #endregion

        #region User Command Menu
        //******************
        // User Command Menu
        //******************
        static void UserInputMenu(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitUserProgramming = false;
            string menuChoice;
            (int speed, int brightness, int tone, double secondsToWait) userCommands;
            userCommands.speed = 0;
            userCommands.brightness = 0;
            userCommands.tone = 0;
            userCommands.secondsToWait = 0;
            List<Command> commandsFromUser = new List<Command>();  

            do
            {
                DisplayScreenHeader("User Programming");

                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands From User");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                Console.CursorVisible = true;
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "a":
                        userCommands = GetUserCommands();
                        break;

                    case "b":
                        GetFinchCommands(commandsFromUser);
                        break;

                    case "c":
                        ShowUserCommands(commandsFromUser);
                        break;

                    case "d":
                        if (userCommands.speed == 0 || userCommands.brightness == 0 || userCommands.tone == 0 || userCommands.secondsToWait == 0)
                        {
                            DisplayEnterData(userCommands);
                        }
                        else
                        {
                            CommandExecution(finchRobot, commandsFromUser, userCommands);
                        }
                        break;

                    case "q":
                        quitUserProgramming = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitUserProgramming);
        }

        //*********************************
        //Displays Missing Variable Message
        //*********************************
        static void DisplayEnterData((int speed, int brightness, int tone, double secondsToWait) userCommands)
        {
            DisplayScreenHeader("\t Enter Missing Data");
            Console.WriteLine();
            Console.WriteLine("\t Please enter the missing values if they are zero");
            Console.WriteLine($"\t Current speed setting: {userCommands.speed}");
            Console.WriteLine($"\t Currnet led brightness settings: {userCommands.brightness}");
            Console.WriteLine($"\t Current tone settings: {userCommands.tone}");
            Console.WriteLine($"\t Current wait time in second(s): {userCommands.secondsToWait}");
            Console.WriteLine();
            DisplayMenuPrompt("User Programming");
        }

        //**********************
        //Executes User Commands
        //**********************
        private static void CommandExecution(Finch finchRobot, 
            List<Command> commandsFromUser, 
            (int speed, int brightness, int tone, double secondsToWait) userCommands)
        {
            int speed = userCommands.speed;
            int brightness = userCommands.brightness;
            int tone = userCommands.tone;
            double waitD = userCommands.secondsToWait * 1000;
            int waitI = Convert.ToInt32(waitD);
            int commandCount = 1;

            DisplayScreenHeader("Execution of Commands");

            Console.WriteLine("\t The Finch will now execute the commands you have given it");
            Console.WriteLine("\t Press any button when you are ready for the Finch to begin");
            Console.ReadKey();
            Console.WriteLine();

            foreach (Command command in commandsFromUser)
            {
                switch (command)
                {
                    case Command.FORWARD:
                        finchRobot.setMotors(speed, speed);
                        Console.WriteLine($"\t Command {commandCount++}: Forward");
                        break;

                    case Command.BACKWARD:
                        finchRobot.setMotors(-speed, -speed);
                        Console.WriteLine($"\t Command {commandCount++}: Backward");
                        break;

                    case Command.LEFTTURN:
                        finchRobot.setMotors(speed, 0);
                        Console.WriteLine($"\t Command {commandCount++}: left turn");
                        break;

                    case Command.RIGHTTURN:
                        finchRobot.setMotors(0, speed);
                        Console.WriteLine($"\t Command {commandCount++}: right turn");
                        break;

                    case Command.STOPMOVING:
                        finchRobot.setMotors(0, 0);
                        Console.WriteLine($"\t Command {commandCount++}: stop moving");
                        break;

                    case Command.TURNONLED:
                        finchRobot.setLED(brightness, brightness, brightness);
                        Console.WriteLine($"\t Command {commandCount++}: LED on");
                        break;

                    case Command.TURNOFFLED:
                        finchRobot.setLED(0, 0, 0);
                        Console.WriteLine($"\t Command {commandCount++}: LED off");
                        break;

                    case Command.PLAYNOTE:
                        finchRobot.noteOn(tone);
                        Console.WriteLine($"\t Command {commandCount++}: note on");
                        break;

                    case Command.NOTEOFF:
                        finchRobot.noteOff();
                        Console.WriteLine($"\t Command {commandCount++}: note off");
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitI);
                        Console.WriteLine($"\t Command {commandCount++}: wait");
                        break;

                    case Command.GETTEMPERATURE:
                        double temp = finchRobot.getTemperature();
                        double tempF = CelsiusToFahrenheit(temp);
                        Console.WriteLine($"\t Command {commandCount++}: get temperature: current temperature {tempF.ToString("n2")}");
                        break;

                    case Command.GETLIGHTDATA:
                        int light = finchRobot.getLeftLightSensor();
                        Console.WriteLine($"\t Command {commandCount++}: get light level: current light level {light}");
                        break;

                    case Command.ISFINCHLEVEL:
                        bool level = finchRobot.isFinchLevel();
                        if (level == true)
                        {
                            Console.WriteLine($"\t Command {commandCount++}: Finch is level");
                        }
                        else
                        {
                            Console.WriteLine($"\t Command {commandCount++}: Finch is not level");
                        }
                        break;

                    case Command.AMIUPSIDEDOWN:
                        bool turnOver = finchRobot.isFinchUpsideDown();
                        if (turnOver == true)
                        {
                            Console.WriteLine($"\t Command {commandCount++}: Finch is upside down, please flip over");
                        }
                        else
                        {
                            Console.WriteLine($"\t Command {commandCount++}: Finch is not upside down");
                        }
                        break;

                    case Command.SURPRISE:
                        finchRobot.setLED(brightness, brightness, brightness);
                        finchRobot.setMotors(speed, -speed);
                        finchRobot.noteOn(tone);
                        finchRobot.wait(waitI);
                        finchRobot.wait(waitI);
                        ResetFinch(finchRobot);
                        Console.WriteLine($"\t Command {commandCount++}: surprise");
                        break;
                        
                    default:
                        break;
                }
            }
            ResetFinch(finchRobot);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t Press any button to go back to the main menu");
            Console.ReadKey();
        }

        //*************************
        //Shows user their commands
        //*************************
        private static void ShowUserCommands(List<Command> commandsFromUser)
        {
            int commandCount = 1;
            int totalCommands = commandsFromUser.Count;

            Console.CursorVisible = false;

            DisplayScreenHeader("Review of User Commands");
            Console.WriteLine();
            Console.WriteLine($"\t Total number of commands for finch to execute: {totalCommands}");
            Console.WriteLine();

            foreach (Command command in commandsFromUser)
            {
                string commandS = command.ToString();
                string commandL = commandS.ToLower();
                Console.WriteLine($"\t Command {commandCount}: {commandL}");
                commandCount++; 
            }
            DisplayMenuPrompt("User Programming");
        }

        //******************
        //Gets user commands
        //******************
        static void GetFinchCommands(List<Command> commandsFromUser)
        {
            Command command = Command.NONE;
            int countCommand = 1;

            DisplayScreenHeader("Finch Robot Commands");
            Console.WriteLine("\t List of available commands");
            foreach (string commandOptions in Enum.GetNames(typeof(Command)))
            {
                Console.WriteLine($"\t - {commandOptions.ToLower()}");
            }
            Console.WriteLine("\t User may now enter commands");
            Console.WriteLine("\t Type done when finished entering commands");
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write($"\t Command {countCommand}: ");
                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commandsFromUser.Add(command);
                    countCommand++;
                }
                else
                {
                    Console.WriteLine("\t Please enter a command from the list of available commands");
                }
               
            }

            DisplayMenuPrompt("User Programming Menu");
        }

       //****************************
       // Menu to get user parameters
       //****************************
        static (int speed, int brightness, int tone, double secondsToWait) GetUserCommands()
        {
            DisplayScreenHeader("User Command Values");

            (int speed, int brightness, int tone, double secondsToWait) userCommands;
            userCommands.speed = 0;
            userCommands.brightness = 0;
            userCommands.tone = 0;
            userCommands.secondsToWait = 0;
            bool quitUserInput = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Set User Parameters");
               
                Console.WriteLine("\ta) Set Motor Speed");
                Console.WriteLine("\tb) Set LED Brightness");
                Console.WriteLine("\tc) Set Tone");
                Console.WriteLine("\td) Set Seconds for Finch to Wait");
                Console.WriteLine("\te) Review User Parameters");
                Console.WriteLine("\tq) User Programming Menu");
                Console.Write("\t\tEnter Choice:");
                Console.CursorVisible = true;
                menuChoice = Console.ReadLine().ToLower();
               
                switch (menuChoice)
                {
                    case "a":
                        userCommands.speed = GetValidSpeed();
                        break;

                    case "b":
                        userCommands.brightness = GetValidLED();
                        break;

                    case "c":
                        userCommands.tone = GetTone();
                        break;

                    case "d":
                        userCommands.secondsToWait = GetValidWaitTime();
                        break;

                    case "e":
                        if (userCommands.speed == 0 || userCommands.brightness == 0 || userCommands.secondsToWait == 0 || userCommands.tone == 0)
                        {
                            DisplayDataError(userCommands);
                        }
                        else
                        {
                            DisplayUserParameters(userCommands);
                        }
                        break;

                    case "q":
                        quitUserInput = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t Please enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitUserInput);

            return userCommands;
        }

        //*************************
        //Displays users parameters
        //*************************
        static void DisplayUserParameters((int speed, int brightness, int tone, double secondsToWait) userCommands)
        {
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine();
            Console.WriteLine($"\t User has set motors to {userCommands.speed}");
            Console.WriteLine($"\t User has set LED brightness to {userCommands.brightness}");
            Console.WriteLine($"\t User has set the wait time to {userCommands.secondsToWait} second(s)");
            Console.WriteLine($"\t User has set the tone to {userCommands.tone}");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to return to the user parameter menu");
            Console.ReadKey();
        }

        //********************************************************
        //Displays Message that user needs to enter more variables
        //********************************************************
        static void DisplayDataError((int speed, int brightness, int tone, double secondsToWait) userCommands)
        {
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine();
            Console.WriteLine("\t Please go back to the menu and re-enter the value(s) that is/are at 0");
            Console.WriteLine();
            Console.WriteLine($"\t Motor speed {userCommands.speed}");
            Console.WriteLine($"\t LED brightness {userCommands.brightness}");
            Console.WriteLine($"\t Seconds to Wait {userCommands.secondsToWait}");
            Console.WriteLine($"\t Tone {userCommands.tone}");
            Console.WriteLine("");
            Console.WriteLine("\t Press any key to return to the user parameter menu");
            Console.ReadKey();
        }

        //*******************
        //Validates User Tone
        //*******************
        static int GetTone()
        {
            Console.Clear();
            bool validResponse;
            string userResponse;
            int tone;
            Console.CursorVisible = true;

            DisplayScreenHeader("Choose Tone");
            Console.WriteLine();
            Console.WriteLine("\t User may now select tone for finch to play: " +
            "500 is low pitched and 10,000 is high pitched");
            Console.WriteLine("\t Please enter a number between 500 and 10,000");

            do
            {
                Console.WriteLine();
                Console.Write("\t");

                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out tone);
                if (tone > 10000 || tone < 500)
                {
                    validResponse = false;
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter a whole integer number between 500 and 10,000");
                }
            }
            while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\t User has chosen to set the tone to {tone}");
            Console.WriteLine();
            Console.WriteLine(" \t Press any key to return to the user parameter menu");
            Console.ReadKey();

            return tone;
        }

        //**************
        //Validate Speed
        //**************
        static int GetValidSpeed()
        {
            Console.Clear();
            bool validResponse;
            string userResponse;
            int motorSpeed;
            Console.CursorVisible = true;

            DisplayScreenHeader("Choose Motor Speed");
            Console.WriteLine();
            Console.WriteLine("\t User may now select motor speed: " +
            "1 is slow and 255 is fast");
            Console.WriteLine("\t Please enter a number between 1 and 255");

            do
            {
                Console.WriteLine();
                Console.Write("\t");
                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out motorSpeed);

                if (motorSpeed > 255 || motorSpeed < 1)
                {
                    validResponse = false;
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter a whole integer number between 1 and 255");
                }
               
            }
            while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\t User has chosen to set motor speed to {motorSpeed}");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to return to the user parameter menu");
            Console.ReadKey();

            return motorSpeed;
        }

        //************
        //Validate LED
        //************
        static int GetValidLED()
        {
            bool validResponse;
            string userResponse;
            int ledSettings;
            Console.CursorVisible = true;

            DisplayScreenHeader("Choose Led Brightness");
            Console.WriteLine();
            Console.WriteLine("\t User may now select LED brightness " +
            "1 is low and 255 is high");
            Console.WriteLine("\t Please enter a number between 1 and 255");

            do
            {
                Console.WriteLine();
                Console.Write("\t");
                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out ledSettings);

                if (ledSettings > 255 || ledSettings < 1)
                {
                    validResponse = false;
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter a whole integer number between 1 and 255");
                }
            }
            while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\t User has chosen set Led brightness to {ledSettings}");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to return to the user parameter menu");
            Console.ReadKey();

            return ledSettings;
        }
        
        //******************
        //Validate Wait Time
        //******************
        static double GetValidWaitTime()
        {
            bool validResponse;
            string userResponse;
            double waitTime;
            Console.CursorVisible = true;

            DisplayScreenHeader("Choose Time to Wait");
            Console.WriteLine();
            Console.WriteLine("\t Please enter wait time (in seconds)");

            do
            {
                Console.WriteLine();
                Console.Write("\t");
                userResponse = Console.ReadLine();
                validResponse = double.TryParse(userResponse, out waitTime);

                if (!validResponse)
                {
                    Console.WriteLine("");
                    Console.WriteLine("\t Please enter a time in seconds");
                }
            }
            while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\t User has chosen to set time to wait to {waitTime}");
            Console.WriteLine();
            Console.WriteLine("\t Press any key to return to the user parameter menu");
            Console.ReadKey();

            return waitTime;
        }
        #endregion

        #region Finch Settings
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
            finchRobot.noteOff();
            
            finchRobot.disConnect();
            Console.WriteLine("\t The Finch robot is now disconnected.");

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
