using System.Text.RegularExpressions;

namespace InformalPenguins {
    public class FileUtility {
        public static string[][] readFileAsArray(string file)
        {
            string text = System.IO.File.ReadAllText(file);
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
            string text = System.IO.File.ReadAllText(file);
            return text;
        }
    }
}