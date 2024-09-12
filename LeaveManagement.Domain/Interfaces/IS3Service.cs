using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(IFormFile file);
        string GeneratePreSignedURL(string fileKey, double durationInMinutes = 60);
        Task<Stream> GetFileAsync(string fileKey);
        Task DeleteFileAsync(string fileKey);
    }
}
