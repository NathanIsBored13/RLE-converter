using System;
using System.IO;
using System.Text;

namespace RLE_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "";
            if (File.Exists(Environment.CurrentDirectory + "\\ascii.txt")) file = "\\ascii.txt";
            else if (File.Exists(Environment.CurrentDirectory + "\\RLE.txt")) file = "\\RLE.txt";
            FileStream fs = File.Open(Environment.CurrentDirectory + file, FileMode.Open, FileAccess.Read, FileShare.None);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            fs.Close();
            string[] read = Encoding.Default.GetString(buffer).Split('\n');
            foreach (string line in read) File.AppendAllText(Environment.CurrentDirectory + file, file == "\\ascii.txt"? ToRLE(line): ToASCII(line));
        }
        static string ToRLE(string line)
        {
            int run = 0;
            string ret = "";
            for (int i = 0; i < line.Length-1; i++)
            {
                char current = line[i];
                if (line[i] == line[i + 1]) run++;
                else
                {
                    //ret += String.Format("{0}{1}", line[i], );
                }
            }
            return ret;
        }
        static string ToASCII(string line)
        {
            return "";
        }
    }
}
