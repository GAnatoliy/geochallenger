using System;
using System.Data;
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

        public async Task<MediaReadDto> GetBlobUrl(string filename, MediaTypeDto mediaType)
        {
            var mediaDescriptor = _azureStorageSettings.MediaContainers[_mapper.Map<MediaType>(mediaType)];
            var blobContainer = await GetContainerAsync(_azureStorageSettings.AzureStorageConnectionString, mediaDescriptor.ContainerName, false);

            var blob = blobContainer.GetBlobReference(filename);
            if (blob == null) {
                return null;
            }

            return new MediaReadDto {
                ContentType = blob.Properties.ContentType,
                Url = blob.Uri.AbsoluteUri
            };
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
                MediaType = mediaType,
                Url = blockBlob.Uri.AbsoluteUri
            };
        }

        /// <summary>
        ///     Get Azure blob storage container
        /// </summary>
        /// <param name="storageConnectionString">Azure storage connection string</param>
        /// <param name="containerName">Container name</param>
        /// <param name="createContainerIfNotExist">Create container if not exist</param>
        /// <returns></returns>
        private async Task<CloudBlobContainer> GetContainerAsync(string storageConnectionString, string containerName, bool createContainerIfNotExist = true)
        {
            // Create storage account
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create azure blob client
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Getting container reference
            var container = blobClient.GetContainerReference(containerName);

            if (createContainerIfNotExist) {
                // Create container if not exist
                await container.CreateIfNotExistsAsync();
            }
            else if (!await container.ExistsAsync()) {
                throw new ObjectNotFoundException($"{containerName} not found.");
                
            }


            // Set container permission
            await container.SetPermissionsAsync(new BlobContainerPermissions {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container;
        }
    }
}
