using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TopWords.Bs.Concretes;
using TopWords.Bs.Interfaces;


namespace TopWords.Test
{
    [TestClass]
    public class TestCases
    {
        private string _filePath;
        private IFileValidator _fileValidator;

        //Arrange
        //Initialize the default file path value.
        [TestInitialize]
        public void Initialize()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            String root = Directory.GetCurrentDirectory();
            //File information has been hard coded to save time. But this could be made as user input.
            _filePath = root + @"\Files\mobydick.txt";
            _fileValidator = new FileValidator();
        }

        /// <summary>
        /// Test Case to check if the filepath is empty or not
        /// </summary>
        [TestMethod]
        public void IsFilePath_Empty_ReturnsBoolen()
        {
            //Arrange
            var emptyFilePath = string.Empty;
            //Act
            var filePathEmpty = _fileValidator.IsFileExist(emptyFilePath);
            //Assert
            Assert.AreEqual(false, filePathEmpty);
        }

        /// <summary>
        /// Test Case to check if the file exist in the given path
        /// </summary>
        [TestMethod]
        public void IsFilePath_Do_Exist_ReturnsBoolen()
        {
            //Act
            var fileExist = _fileValidator.IsFileExist(_filePath);
            //Assert
            Assert.AreEqual(true, fileExist);
        }

        /// <summary>
        /// Test case to check if the given file is a valid text file with *.txt extension.
        /// </summary>
        [TestMethod]
        public void IsValid_TextFile_ReturnsBoolen()
        {
            //Act
            var isValidExt = _fileValidator.IsValidTxtFile(_filePath);
            //Assert
            Assert.AreEqual(true, isValidExt);
        }

        /// <summary>
        /// Test case to check if the given file is ready to read the contents.
        /// </summary>
        [TestMethod]
        public void IsFile_ReadOnly_Or_Open_ReturnsBoolen()
        {
            //Act
            var fileOpenOrReadOnly = _fileValidator.IsFileOpenOrReadOnly(_filePath);
            //Assert
            Assert.AreEqual(true, fileOpenOrReadOnly);
        }

        /// <summary>
        /// Test case to check if the given file is in right UTF8 format.
        /// </summary>
        [TestMethod]
        public void IsFile_In_Utf8_EncodedFormat_ReturnsBoolen()
        {
            //Act
            var fileFileInUtf8 = _fileValidator.IsFileInUtf8(_filePath);
            //Assert
            Assert.AreEqual(true, fileFileInUtf8);
        }

        /// <summary>
        /// Test case to check if invalid file path is provided and exception is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void IsInvalidFilePath_ThrowException()
        {
            //Creating a MOQ object of ILogger interface to instantiate the WordCounter
            //Arrange
            var logger = new Mock<ILoggerFactory>().Object;
            //Act
            var wordCounter = new WordCounter(logger);
            //Assert
            wordCounter.CountWords(String.Empty);
        }
    }
}
