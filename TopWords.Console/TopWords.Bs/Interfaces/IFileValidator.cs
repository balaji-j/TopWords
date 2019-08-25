namespace TopWords.Bs.Interfaces
{
    public interface IFileValidator
    {
        bool IsFileExist(string filePath);
        bool IsValidTxtFile(string filePath);
        bool IsFileOpenOrReadOnly(string filePath);
        bool IsFileInUtf8(string filePath);
    }
}