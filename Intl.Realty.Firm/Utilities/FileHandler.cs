namespace Intl.Realty.Firm.Utilities
{
    public class FileHandler
    {
        public static void DeleteFile(string filePathFull)
        {
            // filePathFull - File Path Containing the filename (Ex. "C:\Users\Documents\MyFile.docx")

            // Delete Old Profile Pic on Folder
            if (System.IO.File.Exists(filePathFull))
            {
                System.IO.File.Delete(filePathFull);
            }
        }

        public static string GetCurrentDirectory()
        {
            var result = Directory.GetCurrentDirectory();
            return result;
        }

        public static string GetStaticContentDirectory(string directory)
        {
            if (directory == null)
            {
                directory = "Uploads\\StaticContent\\"; //Set Default upload path if directory is empty
            }
            var result = Path.Combine(Directory.GetCurrentDirectory(), directory);
            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }
            return result;
        }
        public static string GetFilePath(string directory, string fileName)
        {
            var staticContentDirectory = GetStaticContentDirectory(directory);
            var result = Path.Combine(staticContentDirectory, fileName);
            return result;
        }
        public static string GetFileNameWithoutExtension(string fileName)
        {
            var result = Path.GetFileNameWithoutExtension(fileName);
            return result;
        }
    }
}
