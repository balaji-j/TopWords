using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TopWords.Bs.Interfaces;

namespace TopWords.Bs.Concretes
{
    public class WordCounter : IWordCounter
    {
        private readonly ILogger<WordCounter> _logger;

        //Inject the Logger Factory thru DI
        public WordCounter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WordCounter>();
        }

        /// <summary>
        /// This method returns number of unique words with this number of occurances/count
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Dictionary</returns>
        public IDictionary<string, int> CountWords(string path)
        {
            //Throw exception incase of file path is null or empty
            if (String.IsNullOrEmpty(path))
            {
                throw new FileNotFoundException();
            }

            //Stop watch to record the process timespan
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _logger.LogInformation("Reading file information started");

            //Text Case is ignored to count the words even if its in upper or lower case.
            var result = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Parallel.ForEach(File.ReadLines(path, Encoding.UTF8), line =>
                {
                    //words are splitted base with space and other special chars. 
                    var words = line.Split(
                        new[]
                        {
                            ' ', ',', '*', '.', '\'', '\"', '?', ';', ':', '!', '-', '(', ')', '[', ']', '{', '}', '_',
                            '~', '#', '<', '>'
                        }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        //Concurrent Dictionary allows us to upsert the count.
                        result.AddOrUpdate(word, 1, (_, x) => x + 1);
                    }
                });

            _logger.LogInformation("Reading file information completed");
            _logger.LogInformation("Time taken to read the file " + stopwatch.Elapsed);
            return result;
        }
    }
}
