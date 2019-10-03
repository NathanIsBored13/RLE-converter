using System;
using System.IO;
using System.Text;
using System.Linq;

namespace RLE_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }
        static void Run()
        {
            string input;
            bool exit = false;
            // make the output file, removing it if it currently exists
            File.Create(Environment.CurrentDirectory + "\\output.txt").Close();
            do
            {
                Console.Write("[1]Enter RLE, [2]Display ASCII art file, [3]Convert to ASCII art [4] Convert to RLE [5]exit: ");
                input = Console.ReadLine();
                // if the user entered a valid input
                if (input == "1")
                {
                    do
                    {
                        Console.Write("how many lines of RLE do you wish to enter: ");
                        input = Console.ReadLine();
                        if (!input.All(Char.IsDigit))
                        {
                            Console.WriteLine("only enter digits");
                            input = "0";
                        }
                        else if (Convert.ToInt32(input) < 2) Console.WriteLine("the number of lines must be greater then 2");
                    } while (Convert.ToInt32(input) < 2);
                    string[] ASCII = new string[Convert.ToInt32(input)];
                    for (int i = 0; i < Convert.ToInt32(input); i++) ASCII[i] = Converter(true, EnterRLE());
                    foreach (string line in ASCII) Console.WriteLine(line);
                }
                else if (input == "2")
                {
                    Console.WriteLine("enter the ASCII file name to be displayed");
                    string[] file = ReadFile(string.Format("{0}\\{1}.txt", Environment.CurrentDirectory, Console.ReadLine()));
                    foreach (string line in file) Console.WriteLine(file);
                }
                else if ("34".Contains(input))
                {
                    Console.Write("enter the {0} file name: ", input == "2" ? "ASCII" : "RLE");
                    string[] file = ReadFile(string.Format("{0}\\{1}.txt",Environment.CurrentDirectory, Console.ReadLine()));
                    for (int i = 0; i < file.Length; i++)
                    {
                        // removes any newline and carrage return charecters then passes it to the Converter function alongside the input
                        string print = Converter(input == "3", System.Text.RegularExpressions.Regex.Replace(file[i], "\r|\n", ""));
                        // writes it to the output file, adding newline charecters if it is not the last line
                        File.AppendAllText(Environment.CurrentDirectory + "\\output.txt", string.Format("{0}{1}", print, i < file.Length - 1 ? "" : "\n"));
                        // prints what was written to the file
                        Console.WriteLine(print);
                    }
                }
                else if (input == "5") Console.WriteLine("\nExiting...\n");
                // if the input was not valid, say so
                else
                {
                    Console.WriteLine("\nplease only enter 1, 2, 3, 4 or 5\n");
                    exit = true;
                }
            } while (!exit);
        }
        static string EnterRLE()
        {
            bool valid = true;
            string input = "";
            do
            {
                Console.Write("enter a line of RLE: ");
                input = Console.ReadLine();
                if (input.Length % 3 != 0) valid = false;
                else
                {
                    for (int i = 0; i < input.Length; i += 3)
                    {
                        if (!input.Substring(i, 2).All(char.IsDigit))
                        {
                            Console.WriteLine("this line of RLE is invalid");
                            valid = false;
                        }
                    }
                }
            } while (!valid);
            return input;
        }
        // passes the line to the appropriate function then passes the output back up
        static string Converter(bool option, string line) => option? ToASCII(line) : ToRLE(line);
        static string ToRLE(string line)
        {
            int run = 1;
            string ret = "";
            // for each charecter
            for (int i = 1; i < line.Length; i++)
            {
                // if the charecter differes from the previous charecter or if it is the last charecter
                if (line[i] != line[i - 1] || i == line.Length - 1)
                {
                    // append it to return in the required format
                    ret += string.Format("{0:00}{1}", run, line[i - 1]);
                    // reset the run counter
                    run = 1;
                }
                // if the charecter is the same as the previous, append the run counter
                else run++;
            }
            // if the line contains more then one charecter, return ret if not return the single charecter marked with the counter
            return line.Length > 1? ret : string.Format("{0}01", line[0]);
        }
        static string ToASCII(string line)
        {
            string ret = "";
            // for each set of three carecters, append the third charecter to ret repeated the amount of times signified by the first and second charecters
            for (int i = 0; i < line.Length; i += 3) ret += new string(line[i + 2], Convert.ToInt32(line.Substring(i, 2)));
            return ret;
        }
        static string[] ReadFile(string path)
        {
            string[] ret;
            try
            {
                // open the input file
                FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
                // creat a buffer to hold it
                byte[] buffer = new byte[fs.Length];
                // read the file
                fs.Read(buffer, 0, (int)fs.Length);
                fs.Close();
                //convert the buffer to a string and split it at each new line charecter
                ret = Encoding.Default.GetString(buffer).Split('\n');
            }
            catch (Exception)
            {
                // if the file was not readable, say so and return nothing
                Console.WriteLine("the file is unreadable or non-existant");
                ret = null;
            }
            return ret;
        }
    }
}