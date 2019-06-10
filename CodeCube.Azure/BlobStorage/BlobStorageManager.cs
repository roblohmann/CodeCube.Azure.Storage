using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using CodeCube.Azure.Constants;

namespace CodeCube.Azure.BlobStorage
{
    public sealed class BlobStorageManager
    {
        private readonly string _accountname;
        private readonly string _accessKey;
        private readonly string _connectionstring;

        /// <summary>
        /// Constructor for this class which requires the accountname and accesskey for the blobstorage.
        /// Container name is specific for each action on this blobstorage account.
        /// </summary>
        /// <param name="accountName">The accountname for the blobstorage</param>
        /// <param name="accessKey">The accesskey for the blobstorage</param>
        internal BlobStorageManager(string accountName, string accessKey)
        {
            _accountname = accountName;
            _accessKey = accessKey;
        }

        /// <summary>
        /// Constructor for this class which requires an connectionstring for the blobstorage.
        /// Container name is specific for each action on this blobstorage account.
        /// </summary>
        /// <param name="connectionstring"></param>
        internal BlobStorageManager(string connectionstring)
        {
            _connectionstring = connectionstring;
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
                //Get a reference to the storage account.
                CloudStorageAccount storageAccount = GetCloudStoragaAccount();
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //Get a reference to the container
                CloudBlobContainer containerReference = blobClient.GetContainerReference(container);
                await containerReference.CreateIfNotExistsAsync().ConfigureAwait(false);

                //Create a reference to the blob.
                CloudBlockBlob blockBlob = containerReference.GetBlockBlobReference(filename);

                //Update the blob.
                await blockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length).ConfigureAwait(false);

                //Return the url for the blob.
                return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
            }
            catch (Exception e)
            {
                throw new Exception(ErrorConstants.Blob.FileCouldNotBeStored, e);
            }

        }

        /// <summary>
        /// Download the specified file from the container.
        /// </summary>
        /// <param name="path">The full URL of the blob to download. This is used to get the filename for the blob.</param>
        /// <param name="container">The name of the container where the blob is stored.</param>
        /// <returns>Uri to download the blob including sharedaccess-token</returns>
        public string GetUrlForDownload(string path, string container)
        {
            var filename = Path.GetFileName(path);

            //Get a reference to the storage account.
            var storageAccount = GetCloudStoragaAccount();
            var blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to the container
            var containerReference = blobClient.GetContainerReference(container);

            //Get a reference to the blob.
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
        /// Get the specified file from the BLOB-storage.
        /// The BLOB-container permissions are set to private by default.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <returns>The bytearray for the specified blob.</returns>
        public async Task<byte[]> GetBytes(string filename, string container)
        {
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            };

            return await GetBytes(filename, container, permissions);
        }

        /// <summary>
        /// Get the specified file from the BLOB-storage.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <param name="containerPermissions">The object with container permissions. The enabled or restricts public access to the BLOB-container.</param>
        /// <returns>The bytearray for the specified blob.</returns>
        public async Task<byte[]> GetBytes(string filename, string container, BlobContainerPermissions containerPermissions)
        {
            //Get a reference to the storage account.
            CloudStorageAccount storageAccount = GetCloudStoragaAccount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to the container
            CloudBlobContainer containerReference = blobClient.GetContainerReference(container);
            await containerReference.CreateIfNotExistsAsync().ConfigureAwait(false);
            await containerReference.SetPermissionsAsync(containerPermissions);


            //Create a reference to the blob.
            CloudBlockBlob blockBlob = containerReference.GetBlockBlobReference(filename);

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(ms).ConfigureAwait(false);
                bytes = ms.GetBuffer();
            }

            return bytes;
        }

        /// <summary>
        /// Retrieves the specified file as string.
        /// The BLOB-container permissions are set to private by default.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <returns>The specified file as string.</returns>
        public async Task<string> GetString(string filename, string container)
        {
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                    PublicAccess = BlobContainerPublicAccessType.Off
            };

            return await GetString(filename, container, permissions);
        }

        /// <summary>
        /// Retrieves the specified file as string.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <param name="containerPermissions">The object with container permissions. The enabled or restricts public access to the BLOB-container.</param>
        /// <returns>The specified file as string.</returns>
        public async Task<string> GetString(string filename, string container, BlobContainerPermissions containerPermissions)
        {
            //Get a reference to the storage account.
            CloudStorageAccount storageAccount = GetCloudStoragaAccount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to the container
            CloudBlobContainer containerReference = blobClient.GetContainerReference(container);
            await containerReference.CreateIfNotExistsAsync().ConfigureAwait(false);
            await containerReference.SetPermissionsAsync(containerPermissions);


            //Create a reference to the blob.
            CloudBlockBlob blockBlob = containerReference.GetBlockBlobReference(filename);

            return await blockBlob.DownloadTextAsync().ConfigureAwait(false);
        }

        #region privates
        private CloudStorageAccount GetCloudStoragaAccount()
        {
            if (!string.IsNullOrWhiteSpace(_connectionstring))
            {
                if (!CloudStorageAccount.TryParse(_connectionstring, out CloudStorageAccount storageAccount))
                {
                    throw new InvalidOperationException(ErrorConstants.InvalidConnectionstring);
                }

                return storageAccount;
            }

            //Get the credentials
            var storageCredentials = new StorageCredentials(_accountname, _accessKey);

            return new CloudStorageAccount(storageCredentials, true);
        }
        #endregion
    }
}
