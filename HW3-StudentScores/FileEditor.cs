using System;
using System.IO;
using System.Threading;

namespace HW3_StudentScores
{
    /// <summary>
    /// FileEditor class that handles all stream writings
    /// </summary>
    public static class FileEditor
    {
 
        static FileEditor() { }

        /// <summary>
        /// Verifies that the file doesn't already exists then creates and outputs the
        /// passed in data to that file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool OutputToFile(string path, string data)
        {
            try
            {
                string filepath = @"C:\Users\Public\" + path + ".txt";
                if (!File.Exists(filepath))
                {
                    using (StreamWriter writer = File.AppendText(filepath))
                    {
                        writer.WriteLine(data);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
