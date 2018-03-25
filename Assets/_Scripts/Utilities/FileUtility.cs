using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

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
        public static string ReadFile(string file)
        {
            string text = File.ReadAllText(file);
            return text;
        }
        public static string LoadResource(string filePath)
        {
            string cleanFilePath = sanitizeStringForResource(filePath);
            TextAsset targetFile = Resources.Load<TextAsset>(cleanFilePath);
            return targetFile.text;
        }
        private static string sanitizeStringForResource(string filePath) {
            if (filePath == null) {
                return "";
            }

            return filePath.Replace(Constants.RESOURCES_PATH, "").Replace(Constants.RESOURCES_EXT, "");
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