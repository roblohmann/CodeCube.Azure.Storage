using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CodeCube.Azure.Storage.Constants;
using CodeCube.Azure.Storage.Interfaces;

namespace CodeCube.Azure.Storage
{
    /// <summary>
    /// Manager class to connect to a blob storage and upload or download blobs.
    /// </summary>
    public sealed class BlobStorageManager : BaseManager, IBlobStorageManager
    {
        private readonly BlobServiceClient _blobServiceClient;

        /// <summary>
        /// Constructor for this class which requires the accountname and accesskey for the blobstorage.
        /// Container name is specific for each action on this blobstorage account.
        /// </summary>
        /// <param name="url">The URL to the BLOB-storage.</param>
        /// <param name="accountName">The accountname for the blobstorage</param>
        /// <param name="accessKey">The accesskey for the blobstorage</param>
        internal BlobStorageManager(string url, string accountName, string accessKey)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url), ErrorConstants.Blob.BlobUriRequired);
            }
            if (string.IsNullOrEmpty(accountName))
            {
                throw new ArgumentNullException(nameof(accountName), ErrorConstants.Blob.BlobAccountRequired);
            }
            if (string.IsNullOrEmpty(accessKey))
            {
                throw new ArgumentNullException(nameof(accessKey), ErrorConstants.Blob.BlobAccesskeyRequired);
            }

            ConnectBlobServiceClient(new Uri(url), accountName, accessKey);
        }

        /// <summary>
        /// Constructor for this class which requires the accountname and accesskey for the blobstorage.
        /// Container name is specific for each action on this blobstorage account.
        /// </summary>
        /// <param name="url">The URL to the BLOB-storage.</param>
        /// <param name="accountName">The accountname for the blobstorage</param>
        /// <param name="accessKey">The accesskey for the blobstorage</param>
        internal BlobStorageManager(Uri url, string accountName, string accessKey)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url), ErrorConstants.Blob.BlobUriRequired);
            }
            if (string.IsNullOrEmpty(accountName))
            {
                throw new ArgumentNullException(nameof(accountName), ErrorConstants.Blob.BlobAccountRequired);
            }
            if (string.IsNullOrEmpty(accessKey))
            {
                throw new ArgumentNullException(nameof(accessKey), ErrorConstants.Blob.BlobAccesskeyRequired);
            }

            _blobServiceClient = ConnectBlobServiceClient(url, accountName, accessKey);
        }

        /// <summary>
        /// Constructor for this class which requires an connectionstring for the blobstorage.
        /// Container name is specific for each action on this blobstorage account.
        /// </summary>
        /// <param name="connectionstring"></param>
        internal BlobStorageManager(string connectionstring) : base(connectionstring)
        {
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring), ErrorConstants.Blob.BlobConnectionstringRequired);
            }

            _blobServiceClient = ConnectBlobServiceClient();
        }

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="fileContent">The content for the blob as a stream.</param>
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        public async Task<string> StoreFile(string filename, Stream fileContent, string container, CancellationToken cancellationToken = default)
        {
            try
            {
                //Get a reference to the container
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
                await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                //Create a reference to the blob.
                var blobClient = blobContainerClient.GetBlobClient(filename);

                //Update the blob.
                await blobClient.UploadAsync(fileContent, cancellationToken).ConfigureAwait(false);

                //Return the url for the blob.
                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(ErrorConstants.Blob.FileCouldNotBeStored, e);
            }

        }

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="bytes">The content for the blob in bytes.</param>
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        public async Task<string> StoreFile(string filename, byte[] bytes, string container, CancellationToken cancellationToken = default)
        {
            try
            {
                //Get a reference to the container
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
                await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                //Create a reference to the blob.
                var blobClient = blobContainerClient.GetBlobClient(filename);

                //Update the blob.
                using (var memoryStream = new MemoryStream(bytes))
                {
                    await blobClient.UploadAsync(memoryStream, cancellationToken);
                }

                //Return the url for the blob.
                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(ErrorConstants.Blob.FileCouldNotBeStored, e);
            }
        }

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="bytes">The content for the blob in bytes.</param>
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="overwriteExistingFile">Boolean indicating wether existing blob with this filename should be overwritten.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        public async Task<string> StoreFile(string filename, byte[] bytes, string container, bool overwriteExistingFile, CancellationToken cancellationToken = default)
        {
            return await StoreFile(filename, new BinaryData(bytes), container, overwriteExistingFile, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="binaryData">A <see cref="BinaryData"/>object</param> holding the contents of the file to store.
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="overwriteExistingFile">Boolean indicating wether existing blob with this filename should be overwritten.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        public async Task<string> StoreFile(string filename, BinaryData binaryData, string container, bool overwriteExistingFile, CancellationToken cancellationToken = default)
        {
            try
            {
                //Get a reference to the container
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
                await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                //Create a reference to the blob.
                var blobClient = blobContainerClient.GetBlobClient(filename);

                //Update the blob.
                await blobClient.UploadAsync(binaryData, overwriteExistingFile, cancellationToken).ConfigureAwait(false);

                //Return the url for the blob.
                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(ErrorConstants.Blob.FileCouldNotBeStored, e);
            }
        }

        /// <summary>
        /// Deletes the specified file from the container.
        /// </summary>
        /// <param name="filename">The filename of the blob to delete.</param>
        /// <param name="container">The containername where the blob to be deleted is stored.</param>
        /// <param name="cancellationToken">The cancelaltiontoken.</param>
        /// <returns></returns>
        public async Task<bool> DeleteFile(string filename, string container, CancellationToken cancellationToken = default)
        {
            try
            {
                //Get a reference to the container
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
                await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                //Create a reference to the blob.
                var blobClient = blobContainerClient.GetBlobClient(filename);

                //Update the blob.
                return await blobClient.DeleteIfExistsAsync(cancellationToken:cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(ErrorConstants.Blob.FileCouldNotBeStored, e);
            }
        }

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="binaryData">A <see cref="BinaryData"/>object</param> holding the contents of the file to store.
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        public async Task<string> StoreFile(string filename, BinaryData binaryData, string container, CancellationToken cancellationToken = default)
        {
            return await StoreFile(filename, binaryData.ToArray(), container, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the specified file from the BLOB-storage.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The bytearray for the specified blob.</returns>
        public async Task<byte[]> GetBytes(string filename, string container, CancellationToken cancellationToken = default)
        {
            //Get a reference to the container
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
            await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.None, cancellationToken: cancellationToken).ConfigureAwait(false);

            var blobClient = blobContainerClient.GetBlobClient(filename);

            BlobDownloadInfo downloadInfo = await blobClient.DownloadAsync(cancellationToken).ConfigureAwait(false);

            byte[] bytes;
            await using (var memoryStream = new MemoryStream())
            {
                await downloadInfo.Content.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
                bytes = memoryStream.ToArray();
            }

            return bytes;
        }

        /// <summary>
        /// Retrieves the specified file as string.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The specified file as string.</returns>
        public async Task<string> GetString(string filename, string container, CancellationToken cancellationToken = default)
        {
            return Encoding.UTF8.GetString(await GetBytes(filename, container, cancellationToken));

            ////Get a reference to the container
            //var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);
            //await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            //await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.None, cancellationToken: cancellationToken).ConfigureAwait(false);

            //var blobClient = blobContainerClient.GetBlobClient(filename);

            //BlobDownloadInfo downloadInfo = await blobClient.DownloadAsync(cancellationToken).ConfigureAwait(false);

            //string returnvalue;
            //using (var memoryStream = new MemoryStream())
            //{
            //    await downloadInfo.Content.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            //    using (var streamReader = new StreamReader(memoryStream))
            //    {
            //        returnvalue = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            //    }
            //}

            //return returnvalue;
        }

        #region privates
        private BlobServiceClient ConnectBlobServiceClient()
        {
            return ConnectCloudStorageAccountWithConnectionString();
        }

        private BlobServiceClient ConnectBlobServiceClient(Uri uri, string accountname, string accessKey)
        {
            //Get the credentials
            var sharedkeyCredential = new StorageSharedKeyCredential(accountname, accessKey);

            return new BlobServiceClient(uri, sharedkeyCredential);
        }

        private BlobServiceClient ConnectCloudStorageAccountWithConnectionString()
        {
            if (string.IsNullOrWhiteSpace(Connectionstring))
            {
                throw new InvalidOperationException("No connectionstring available!");
            }

            return new BlobServiceClient(Connectionstring);
        }
        #endregion
    }
}
