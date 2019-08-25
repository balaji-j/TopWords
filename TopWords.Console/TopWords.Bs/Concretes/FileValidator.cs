using System;
using System.IO;
using System.Text;
using TopWords.Bs.Interfaces;

namespace TopWords.Bs.Concretes
{
    public class FileValidator : IFileValidator
    {
        /// <summary>
        /// method to check if file exist or not.
        /// </summary>
        /// <param name="filePath">the file we wish to check</param>
        /// <returns>boolen</returns>
        public bool IsFileExist(string filePath)
        {
            //Check the file is not null or empty
            if (!String.IsNullOrEmpty(filePath))
            {
                try
                {
                    //Check the file exist in the given file path
                    return System.IO.File.Exists(filePath);
                }
                catch (IOException ex)//Exception thrown if the file could not be opened for some reason.
                {
                    Console.WriteLine("File does not exist. Reason: " + ex.Message);
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Method to check if the file provided is Text file with .txt extension.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>boolen</returns>
        public bool IsValidTxtFile(string filePath)
        {
            //read the file extension from file path
            string ext = Path.GetExtension(filePath);

            //check if the file extension is not null and equals to .txt.
            //transforming the extension to lower case to compare the extension.
            if (ext != null && ext.ToLower() == ".txt")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Un-Supported file type. Please try using Text File(*.txt)");
                return false;
            }
        }


        /// <summary>
        /// method to check not only if a file is already open, but if the
        /// but also if read and write permissions exist
        /// </summary>
        /// <param name="filePath">the file we wish to check</param>
        /// <returns>boolen</returns>
        public bool IsFileOpenOrReadOnly(string filePath)
        {
            try
            {
                //first make sure it's not a read only file
                if ((File.GetAttributes(filePath) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                {
                    //first we open the file with a FileStream
                    using (FileStream stream =
                        new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                    {
                        try
                        {
                            stream.ReadByte();
                            return true;
                        }
                        catch (IOException) //Exception thrown if the file could not be opened for some reason.
                        {
                            return false;
                        }
                        finally
                        {
                            //Close the file stream and dispose the memory stream to release the managed and unmanaged resources.
                            stream.Close();
                            stream.Dispose();
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (IOException ex)//Exception thrown if the file could not be opened for some reason.
            {
                Console.WriteLine("File could not be opened. Reason: " + ex.Message);
                return true;
            }
        }

        /// <summary>
        /// Method to check if the given file is in UTF-8 format
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>boolen</returns>
        public bool IsFileInUtf8(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath, true))
            {
                while (sr.Peek() >= 0)
                {
                    var contents = (char)sr.Read();
                }

                //Test for the encoding after reading, or at least
                //after the first read.
                if (Equals(sr.CurrentEncoding, Encoding.UTF8))
                {
                    return true;
                }

                Console.WriteLine("Warning: Invalid file format. Please provide a valid UTF8 Encoded file");
                return false;
            }
        }
    }
}
