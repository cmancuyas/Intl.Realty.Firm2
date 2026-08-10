using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Intl.Realty.Firm.Utilities
{
    public static class TransactionHelper
    {
        public const string ActionSave = "save";
        public const string ActionSubmit = "submit";

        public static string BuildDealTransactionNumber(int transactionTypeId, int nextNumber)
        {
            string type = transactionTypeId.ToString().PadLeft(2, '0');
            string date = DateTime.Now.ToString("yyyyMMdd");
            string seq = nextNumber.ToString().PadLeft(5, '0');
            return $"{date}-{type}-{seq}";
        }

        /// <summary>
        /// Normalizes Save/Submit form action. Defaults to "save" when missing/blank.
        /// </summary>
        public static string NormalizeActionType(string? actionType)
        {
            if (string.IsNullOrWhiteSpace(actionType))
                return ActionSave;

            return actionType.Trim().ToLowerInvariant();
        }

        public static bool IsSubmitAction(string? actionType)
        {
            return string.Equals(
                NormalizeActionType(actionType),
                ActionSubmit,
                StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsSaveAction(string? actionType)
        {
            return string.Equals(
                NormalizeActionType(actionType),
                ActionSave,
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Removes characters that are invalid in Windows/Linux path segments.
        /// </summary>
        public static string SanitizePathSegment(string? value, string fallback = "Unknown")
        {
            if (string.IsNullOrWhiteSpace(value))
                return fallback;

            var invalidChars = Path.GetInvalidFileNameChars();
            var cleaned = string.Concat(value.Trim().Select(ch => invalidChars.Contains(ch) ? '_' : ch));
            cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

            return string.IsNullOrWhiteSpace(cleaned) ? fallback : cleaned;
        }

        public static void EnsureDirectoryExists(string absoluteOrRelativeDirectory)
        {
            if (string.IsNullOrWhiteSpace(absoluteOrRelativeDirectory))
                return;

            var absolutePath = Path.IsPathRooted(absoluteOrRelativeDirectory)
                ? absoluteOrRelativeDirectory
                : Path.Combine(Directory.GetCurrentDirectory(), absoluteOrRelativeDirectory);

            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);
        }

        public static bool HasUploadableFiles(IEnumerable<IFormFile>? files)
        {
            return files != null && files.Any(f => f != null && f.Length > 0);
        }
    }
}
