using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LeaveManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Services
{
    public class S3Service : IS3Service
    {
        private readonly string _bucketName = "filestorage2910"; 
        private readonly IAmazonS3 _s3Client;

        public S3Service()
        {
            
            _s3Client = new AmazonS3Client(Amazon.RegionEndpoint.EUNorth1); 
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
