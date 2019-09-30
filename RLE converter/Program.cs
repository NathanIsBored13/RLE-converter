using System;
using System.IO;
using System.Text;

namespace RLE_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists(Environment.CurrentDirectory + "\\input.txt"))
            {
                Console.WriteLine("input file does not exist, exiting");
                Environment.Exit(0);
            }
            FileStream fs = File.Open(Environment.CurrentDirectory + "\\input.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            fs.Close();
            string[] read = Encoding.Default.GetString(buffer).Split('\n');
            string input;
            File.Create(Environment.CurrentDirectory + "\\output.txt").Close();
            do
            {
                Console.Write("[1]converte from ASCII, [2]converte from RLE: ");
                input = Console.ReadLine();
                if (input == "1") foreach (string line in read) File.AppendAllText(Environment.CurrentDirectory + "\\output.txt", ToRLE(line));
                else if (input == "2")
                {

                }
                else Console.WriteLine("\nonly enter 1 or 2\n");
            } while (!"12".Contains(input));
        }
        static string ToRLE(string line)
        {
            int run = 0;
            string ret = "";
            char current = line[0];
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == current) run++;
                else //if (line[i] != current)
                {
                    ret += String.Format("{0}{1:00}", current, run);
                    run = 1;
                    current = line[i];
                }
            }
            return ret += String.Format("{0}{1:00}", current, run);
        }
        static string ToASCII(string line)
        {
            return "";
        }
    }
}
