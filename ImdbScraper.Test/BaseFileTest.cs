using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ImdbScraper.Test
{
    public class BaseFileTest
    {
        protected string GetTestFile(string filename)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string solutionPath = string.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 6));
            var filePathItems = filename.Split('/');
            string filePath = string.Join(Path.DirectorySeparatorChar.ToString(), filePathItems);
            string completePath = Path.Combine(solutionPath, "TestData", filePath);

            string readContents;
            using (StreamReader streamReader = new StreamReader(completePath, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }
            return readContents;
        }
    }
}
