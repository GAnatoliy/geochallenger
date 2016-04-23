using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Domains.Media;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Media;
using GeoChallenger.Services.Settings.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GeoChallenger.Services
{
    public class AzureBlobMediaService : IMediaService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IMapper _mapper;

        public AzureBlobMediaService(AzureStorageSettings azureStorageSettings, IMapper mapper)
        {
            _azureStorageSettings = azureStorageSettings;
            _mapper = mapper;
        }

        public async Task<MediaUploadResultDto> UploadAsync(Stream stream, MediaTypeDto mediaType)
        {
            var mediaDescriptor = _azureStorageSettings.MediaContainers[_mapper.Map<MediaType>(mediaType)];
            var blobContainer = await GetContainerAsync(_azureStorageSettings.AzureStorageConnectionString, mediaDescriptor.ContainerName);

            var blobName = $"{Guid.NewGuid()}.{mediaDescriptor.FileExtension}";
            var blockBlob = blobContainer.GetBlockBlobReference(blobName);

            blockBlob.Properties.ContentType = mediaDescriptor.ContentType;

            await blockBlob.UploadFromStreamAsync(stream);

            return new MediaUploadResultDto {
                Name = blobName,
                ContentType = blockBlob.Properties.ContentType,
                Uri = blockBlob.Uri.AbsoluteUri
            };
        }

        /// <summary>
        ///     Get Azure blob storage container
        /// </summary>
        /// <param name="storageConnectionString">Azure storage connection string</param>
        /// <param name="containerName">Container name</param>
        /// <returns></returns>
        private async Task<CloudBlobContainer> GetContainerAsync(string storageConnectionString, string containerName)
        {
            // Create storage account
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create azure blob client
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Getting container reference
            var container = blobClient.GetContainerReference(containerName);

            // Create container if not exist
            await container.CreateIfNotExistsAsync();

            // Set container permission
            await container.SetPermissionsAsync(new BlobContainerPermissions {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container;
        }

        private async Task ValidateMediaType()
        {
            
        }
    }
}
