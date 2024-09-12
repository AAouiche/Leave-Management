using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Interfaces
{
    public interface ICloudStorageService
    {
        Task UploadFileAsync(string fileName, Stream fileStream);
        Task<Stream> DownloadFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
