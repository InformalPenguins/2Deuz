using System.Text.RegularExpressions;
using System.IO;
namespace InformalPenguins {
    public class FileUtility {
        public static string[][] readFileAsArray(string file)
        {
            string text = File.ReadAllText(file);
            string[] lines = Regex.Split(text, "\r\n");
            int rows = lines.Length;

            string[][] levelBase = new string[rows][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] stringsOfLine = Regex.Split(lines[i], " ");
                levelBase[i] = stringsOfLine;
            }
            return levelBase;
        }
        public static string readFile(string file)
        {
            string text = File.ReadAllText(file);
            return text;
        }
        public static void writeFile(string file, string content)
        {
            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(file, false);
            writer.WriteLine(content);
            writer.Close();
        }

    }
}