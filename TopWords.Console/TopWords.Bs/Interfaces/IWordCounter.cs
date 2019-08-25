using System.Collections.Generic;

namespace TopWords.Bs.Interfaces
{
    public interface IWordCounter
    {
        IDictionary<string, int> CountWords(string path);
    }
}