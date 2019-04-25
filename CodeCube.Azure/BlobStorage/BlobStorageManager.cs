using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using CodeCube.Azure.Constants;

namespace CodeCube.Azure.BlobStorage
{
    public sealed class BlobStorageManager
    {
        private readonly string _accountname;
        private readonly string _accessKey;

        /// <summary>
        /// Constructor for this class which requires the accountname and accesskey for the blobstorage.
        /// Container name is specific for each action on this blobstorage account.
        /// </summary>
        /// <param name="accountName">The accountname for the blobstorage</param>
        /// <param name="accessKey">The accesskey for the blobstorage</param>
        public BlobStorageManager(string accountName, string accessKey)
        {
            _accountname = accountName;
            _accessKey = accessKey;
        }

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="bytes">The content for the blob in bytes.</param>
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <returns>The URI for the blobfile.</returns>
        public async Task<string> StoreFile(string filename, byte[] bytes, string container)
        {
            try
            {
                //Get the credentials
                var storageCredentials = new StorageCredentials(_accountname, _accessKey);

                //Get a reference to the storage account.
                var storageAccount = new CloudStorageAccount(storageCredentials, true);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //Get a reference to the container
                CloudBlobContainer containerReference = blobClient.GetContainerReference(container);
                await containerReference.CreateIfNotExistsAsync();

                //Create a reference to the blob.
                CloudBlockBlob blockBlob = containerReference.GetBlockBlobReference(filename);

                //Update the blob.
                await blockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);

                //Return the url for the blob.
                return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
            }
            catch (Exception e)
            {
                throw new Exception(ErrorConstants.FileCouldNotBeStored, e);
            }

        }

        /// <summary>
        /// Download the specified file from the container.
        /// </summary>
        /// <param name="url">The full URL of the blob to download.</param>
        /// <param name="container">The name of the container where the blob is stored.</param>
        /// <returns>Forces an download for the file.</returns>
        public string DownloadFile(string url, string container)
        {
            var sc = new StorageCredentials(_accountname, _accessKey);
            var filename = Path.GetFileName(url);
            var storageAccount = new CloudStorageAccount(sc, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var containerReference = blobClient.GetContainerReference(container);
            var blockBlob = containerReference.GetBlockBlobReference(filename);

            //Create an ad-hoc Shared Access Policy with read permissions which will expire in 1 minute
            var policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1)
            };

            //Set content-disposition header for force download
            var headers = new SharedAccessBlobHeaders()
            {
                ContentDisposition = $"attachment;filename=\"{filename}\"",
            };

            var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);

            return blockBlob.Uri.AbsoluteUri.Replace("http://", "https://") + sasToken;

        }

        /// <summary>
        /// Get the file behind the specified URL as byte-array.
        /// </summary>
        /// <param name="url">The full URL for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <returns></returns>
        public async Task<byte[]> GetBytes(string url, string container)
        {
            var storageCredentials = new StorageCredentials(_accountname, _accessKey);
            var filename = Path.GetFileName(url.Split('?')[0]);

            if (!string.IsNullOrWhiteSpace(filename)) filename = HttpUtility.UrlDecode(filename);

            var storageAccount = new CloudStorageAccount(storageCredentials, false);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var containerReference = blobClient.GetContainerReference(container);
            var blockBlob = containerReference.GetBlockBlobReference(filename);

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(ms);
                bytes = ms.GetBuffer();
            }
            return bytes;

        }
    }
}
