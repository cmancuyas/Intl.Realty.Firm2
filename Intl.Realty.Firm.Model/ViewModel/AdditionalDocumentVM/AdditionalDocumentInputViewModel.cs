using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intl.Realty.Firm.Model.ViewModel.AdditionalDocumentVM
{
    public class AdditionalDocumentInputViewModel
    {
        public int? Id { get; set; }

        public string? DocumentName { get; set; }

        public IFormFile? File { get; set; }

        public string? ExistingFileName { get; set; }
        public string? ExistingFilePath { get; set; }
    }
}
