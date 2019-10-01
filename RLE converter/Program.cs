using System;
using System.IO;
using System.Text;

namespace RLE_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(Environment.CurrentDirectory + "\\input.txt")) Select();
            else Console.WriteLine("input file does not exist");
        }
        static void Select()
        {
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
                if ("12".Contains(input)) for (int i = 0; i < read.Length; i++) File.AppendAllText(Environment.CurrentDirectory + "\\output.txt", String.Format("{0}{1}", Converter(input, read[i].Trim()), i == read.Length - 1? "" : "\n"));
                else Console.WriteLine("\nonly enter 1 or 2\n");
            } while (!"12".Contains(input));
        }
        static string Converter(string option, string line) => option == "1"? ToRLE(line) : ToASCII(line);
        static string ToRLE(string line)
        {
            int run = 1;
            string ret = "";
            for (int i = 1; i < line.Length; i++)
            {
                if (line[i] != line[i - 1] || i == line.Length - 1)
                {
                    ret += String.Format("{0}{1:00}", line[i - 1], run);
                    run = 1;
                }
                else run++;
            }
            return line.Length == 1? String.Format("{0}01", line[0]) : ret;
        }
        static string ToASCII(string line)
        {
            string ret = "";
            for (int i = 0; i < line.Length; i += 3) ret += new String(line[i], Convert.ToInt32(line.Substring(i + 1, 2)));
            return ret;
        }
    }
}
