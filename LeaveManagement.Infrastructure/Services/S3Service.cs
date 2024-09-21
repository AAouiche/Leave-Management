using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LeaveManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Services
{
    public class S3Service : IS3Service
    {
        private readonly string _bucketName; 
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;

        public S3Service(IConfiguration configuration, IAmazonS3 s3Client)
        {
            _configuration = configuration;
            _bucketName = _configuration["AWS:BucketName"]!;
            _s3Client = s3Client;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            

            var fileTransferUtility = new TransferUtility(_s3Client);

            var key = $"LeaveRequests/{Guid.NewGuid()}_{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = key,
                    BucketName = _bucketName,
                    CannedACL = S3CannedACL.Private 
                };

                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            return key;
        }
        public string GeneratePreSignedURL(string fileKey, double durationInMinutes = 60)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.AddMinutes(durationInMinutes)
            };

            return _s3Client.GetPreSignedURL(request);
        }
        public async Task<Stream> GetFileAsync(string fileKey)
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, fileKey);
            return response.ResponseStream; 
        }
        public async Task DeleteFileAsync(string fileKey)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }
    }
}
