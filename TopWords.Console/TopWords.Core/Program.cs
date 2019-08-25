using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopWords.Bs.Concretes;
using TopWords.Bs.Interfaces;

namespace TopWords.Core
{
    /// <summary>
    /// Program to display the Top words with its count 
    /// </summary>
    class Program
    {
        private static int _displayCount = 20; //As per user specification, the value has be defaulted to 20.
        private static string _filePath;
        private static IWordCounter WordCounter { get; set; }
        private static IFileValidator FileValidator { get; set; }

        static void Main(string[] args)
        {
            #region Initializing Objects

            //Introducing Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IWordCounter, WordCounter>()
                .AddSingleton<IFileValidator, FileValidator>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            WordCounter = serviceProvider.GetService<IWordCounter>();
            FileValidator = serviceProvider.GetService<IFileValidator>();

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            String root = Directory.GetCurrentDirectory();

            //File information has been hard coded to save time. But this could be made as user input.
            _filePath = root + @"\Files\mobydick.txt";

            #endregion

            //Read the display count from user.
            Console.Write("Enter the number of top words you would like to view (or press enter to skip and display top 20):");
            var userEntry = Console.ReadLine();

            #region Read User Input

            //Check if the user has entered any input
            if (!String.IsNullOrEmpty(userEntry))
            {
                //Check the user input is valid integer or not
                while (!Int32.TryParse(userEntry, out _displayCount))
                {
                    Console.WriteLine("Not a valid number, Please enter again.");
                    Console.Write("Enter the number of top words you would like to view:");

                    userEntry = Console.ReadLine();
                }
            }
            #endregion
            
            //Some Decoration for better UI
            Console.WriteLine("".PadRight(24, '-'));

            var isValidFile = Validator(FileValidator);

            if (isValidFile)
            {
                //Some Decoration for better UI
                Console.WriteLine("Output");
                Console.WriteLine("".PadRight(24, '-'));

                #region Counting the Top Words

                //Returns a dictionary of unique word and it's count
                var result = WordCounter.CountWords(_filePath);

                int rank = 1;

                //Sort the dictionary elements to display the top count.
                foreach (var word in result.OrderByDescending(x => x.Value).Take(_displayCount))
                {
                    //Print the result in diserable format.
                    Console.WriteLine("Rank: " + rank + " | " + "Word: " + word.Key + " | " + "Count: " + word.Value);
                    //increase the counter to display the rank.
                    rank++;
                }

                #endregion

                //Some Decoration for better UI
                Console.WriteLine("".PadRight(24, '-'));
                Console.WriteLine("Please enter key to exit the program.");
                logger.LogInformation("All done!");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Method to validate the text file
        /// </summary>
        /// <param name="fileValidator"></param>
        /// <returns>boolen</returns>
        static bool Validator(IFileValidator fileValidator)
        {
            //Validate if the file exist in the given filePath
            if (!fileValidator.IsFileExist(_filePath)) return false;
            
            //Validate if the file is not already open and ready to read the file stream.
            if (!fileValidator.IsFileOpenOrReadOnly(_filePath)) return false;

            //Validate if the given file is a valid text file with "*.txt" extension.
            if (!fileValidator.IsValidTxtFile(_filePath)) return false;

            //Validate if the given file is in UTF 8 Encoded
            if (!fileValidator.IsFileInUtf8(_filePath)) return false;

            return true;
        }
    }
}
