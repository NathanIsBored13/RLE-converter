using System;
using System.IO;
using System.Text;

namespace RLE_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            // reads the file
            string[] file = ReadFile(Environment.CurrentDirectory + "\\input.txt");
            // if the file was readable, run the main program with the file as an input
            if (file != null) Run(file);
        }
        static void Run(string[] file)
        {
            string input;
            // make the output file, removing it if it currently exists
            File.Create(Environment.CurrentDirectory + "\\output.txt").Close();
            do
            {
                Console.Write("[1]converte from ASCII, [2]converte from RLE: ");
                input = Console.ReadLine();
                // if the user entered a valid input
                if ("12".Contains(input))
                {
                    for (int i = 0; i < file.Length; i++)
                    {
                        // removes any newline and carrage return charecters then passes it to the Converter function alongside the input
                        string print = Converter(input, System.Text.RegularExpressions.Regex.Replace(file[i], "\r|\n", ""));
                        // writes it to the output file, adding newline charecters if it is not the last line
                        File.AppendAllText(Environment.CurrentDirectory + "\\output.txt", string.Format("{0}{1}", print, i < file.Length - 1? "" : "\n"));
                        // prints what was written to the file
                        Console.WriteLine(print);
                    }
                }
                // if the input was not valid, say so
                else Console.WriteLine("\nonly enter 1 or 2\n");
            } while (!"12".Contains(input));
        }
        // passes the line to the appropriate function then passes the output back up
        static string Converter(string option, string line) => option == "1"? ToRLE(line) : ToASCII(line);
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