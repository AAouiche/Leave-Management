using Google.Apis.Drive.v3;
using Google.Apis.Upload;
using DriveFile = Google.Apis.Drive.v3.Data.File;
using LeaveManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Services
{
    public class GoogleDriveService : ICloudStorageService
    {
        private readonly DriveService _driveService;
        private readonly string _leaveManagementFolderId = "1G3dT8WLD0a2yvBaLZJeOUgc-wCgfCZc4";

        public GoogleDriveService(DriveService driveService)
        {
            _driveService = driveService;
        }

        public async Task UploadFileAsync(string fileName, Stream fileStream)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
                Parents = new List<string> { _leaveManagementFolderId } 
            };

            FilesResource.CreateMediaUpload request = _driveService.Files.Create(
                fileMetadata, fileStream, "application/octet-stream");
            request.Fields = "id";

            var result = await request.UploadAsync();
            if (result.Status != Google.Apis.Upload.UploadStatus.Completed)
            {
                throw new Exception($"Failed to upload file: {result.Exception?.Message}");
            }

            
            // var file = request.ResponseBody;
            // Console.WriteLine("File ID: " + file.Id);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var request = _driveService.Files.List();
            request.Q = $"name='{fileName}' and '{_leaveManagementFolderId}' in parents and trashed=false"; 
            request.Fields = "files(id, name)";

            var files = await request.ExecuteAsync();

            if (files.Files == null || !files.Files.Any())
                throw new FileNotFoundException($"File '{fileName}' not found in the Leave Management folder.");

            var fileId = files.Files.First().Id;

            var stream = new MemoryStream();
            var getRequest = _driveService.Files.Get(fileId);
            await getRequest.DownloadAsync(stream);

            stream.Position = 0; 
            return stream;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var request = _driveService.Files.List();
            request.Q = $"name='{fileName}' and '{_leaveManagementFolderId}' in parents and trashed=false";
            request.Fields = "files(id, name)";

            var files = await request.ExecuteAsync();

            if (files.Files == null || !files.Files.Any())
                throw new FileNotFoundException($"File '{fileName}' not found in the Leave Management folder.");

            var fileId = files.Files.First().Id;

            var deleteRequest = _driveService.Files.Delete(fileId);
            await deleteRequest.ExecuteAsync();
        }
    }
}
