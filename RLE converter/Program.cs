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
            string input;
            bool exit = false;
            do
            {
                Console.Write("[1]Enter RLE, [2]Display ASCII art file, [3]Convert to ASCII art [4] Convert to RLE [5]exit: ");
                input = Console.ReadLine();
                if (input == "1")
                {
                    do
                    {
                        Console.Write("\nhow many lines of RLE do you wish to enter: ");
                        input = Console.ReadLine();
                        if (!input.All(char.IsDigit))
                        {
                            Console.WriteLine("\nplease only enter digits");
                            input = "0";
                        }
                        else if (Convert.ToInt32(input) <= 2) Console.WriteLine("\nthe number of lines must be greater then 2");
                    } while (Convert.ToInt32(input) <= 2);
                    string[] ASCII = new string[Convert.ToInt32(input)];
                    for (int i = 0; i < Convert.ToInt32(input); i++) ASCII[i] = Converter(true, EnterRLE());
                    foreach (string line in ASCII) Console.WriteLine(line.ToString());
                }
                else if (input == "2")
                {
                    string[] file = ReadFileFromName("enter the name of the .txt ASCII file to be displayed");
                    foreach (string line in file) Console.WriteLine(line.ToString());
                }
                else if ("34".Contains(input))
                {
                    string[] file = ReadFileFromName("enter the name of the .txt file to be converted");
                    File.Create(Environment.CurrentDirectory + "\\output.txt").Close();
                    for (int i = 0; i < file.Length; i++)
                    {
                        string print = Converter(input == "3", System.Text.RegularExpressions.Regex.Replace(file[i], "\r|\n", ""));
                        File.AppendAllText(Environment.CurrentDirectory + "\\output.txt", string.Format("{0}{1}", print, i < file.Length - 1 ? "\n" : ""));
                        Console.WriteLine(print);
                    }
                    Console.WriteLine("\nthe output was saved in the\"output\" file");
                }
                else if (input == "5")
                {
                    Console.WriteLine("\nExiting...\n");
                    exit = true;
                }
                else Console.WriteLine("\nplease only enter 1, 2, 3, 4 or 5\n");
            } while (!exit);
        }
        static string EnterRLE()
        {
            bool valid;
            string input = "";
            do
            {
                valid = true;
                Console.Write("\nenter a line of RLE: ");
                input = Console.ReadLine();
                if (input.Length % 3 != 0) valid = false;
                for (int i = 0; i < input.Length && valid; i += 3)
                {
                    if (!input.Substring(i, 2).All(char.IsDigit)) valid = false;
                }
                if (!valid) Console.WriteLine("\nthis line of RLE is invalid");
            } while (!valid);
            return input;
        }
        static string Converter(bool option, string line) => option? ToASCII(line) : ToRLE(line);
        static string ToRLE(string line)
        {
            int run = 1;
            string ret = "";
            for (int i = 1; i <= line.Length; i++)
            {
                if (line[i < line.Length? i : i - 1] != line[i - 1] || i == line.Length)
                {
                    ret += string.Format("{0:00}{1}", run, line[i - 1]);
                    run = 1;
                }
                else run++;
            }
            return line.Length > 1? ret : string.Format("{0}01", line[0]);
        }
        static string ToASCII(string line)
        {
            string ret = "";
            for (int i = 0; i < line.Length; i += 3) ret += new string(line[i + 2], Convert.ToInt32(line.Substring(i, 2)));
            return ret;
        }
        static string[] ReadFileFromName(string text)
        {
            string input;
            do
            {
                Console.Write(string.Format("\n{0}: ", text));
                input = Console.ReadLine();
                if (!File.Exists(string.Format("{0}\\{1}.txt", Environment.CurrentDirectory, input))) Console.WriteLine("\nthe file does not exist, try again");
            } while (!File.Exists(string.Format("{0}\\{1}.txt", Environment.CurrentDirectory, input)));
            return ReadFile(string.Format("{0}\\{1}.txt", Environment.CurrentDirectory, input));
        }
        static string[] ReadFile(string path)
        {
            string[] ret;
            try
            {
                FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                fs.Close();
                ret = Encoding.Default.GetString(buffer).Split('\n');
            }
            catch (Exception)
            {
                Console.WriteLine("Error, the file is unreadable or non-existant");
                ret = null;
            }
            return ret;
        }
    }
}