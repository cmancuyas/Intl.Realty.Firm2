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
            // Prefer the application content root (set at startup) over the process
            // working directory, which points to C:\Windows\System32\inetsrv under IIS.
            var result = TransactionHelper.ContentRootPath ?? Directory.GetCurrentDirectory();
            return result;
        }

        public static string GetStaticContentDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Path.Combine("Uploads", "StaticContent");
            }

            var result = Path.IsPathRooted(directory)
                ? directory
                : Path.Combine(TransactionHelper.ContentRootPath ?? AppContext.BaseDirectory, directory);

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

        /// <summary>
        /// Resolves a stored relative/absolute path to an existing file on disk.
        /// Prefers Directory + FileName + Extension (same as uploads), then FullPath variants.
        /// </summary>
        public static string? ResolveExistingFilePath(
            string? fullPath,
            string? directory,
            string? fileName,
            string? fileExtension)
        {
            var candidates = new List<string>();

            if (!string.IsNullOrWhiteSpace(directory) && !string.IsNullOrWhiteSpace(fileName))
            {
                var combinedName = fileName + (fileExtension ?? string.Empty);
                candidates.Add(GetFilePath(directory, combinedName));
            }

            if (!string.IsNullOrWhiteSpace(fullPath))
            {
                candidates.Add(fullPath);
                if (!Path.IsPathRooted(fullPath))
                    candidates.Add(Path.Combine(TransactionHelper.ContentRootPath ?? AppContext.BaseDirectory, fullPath));
            }

            foreach (var candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (File.Exists(candidate))
                    return candidate;
            }

            return null;
        }
    }
}
