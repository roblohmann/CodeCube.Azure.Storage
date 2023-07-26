using Azure;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeCube.Azure.Storage.Interfaces
{
    public interface IBlobStorageManager
    {
        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="fileContent">The content for the blob as a stream.</param>
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        Task<string> StoreFile(string filename, Stream fileContent, string container, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="bytes">The content for the blob in bytes.</param>
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        Task<string> StoreFile(string filename, byte[] bytes, string container, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a file in the blob-storage.
        /// </summary>
        /// <param name="filename">The filename of the blob.</param>
        /// <param name="binaryData">A <see cref="BinaryData"/>object</param> holding the contents of the file to store.
        /// <param name="container">The containername where to store the blob. If the container doesn't exist it will be created.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The URI for the blobfile.</returns>
        Task<string> StoreFile(string filename, BinaryData binaryData, string container, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the specified file from the BLOB-storage.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The bytearray for the specified blob.</returns>
        Task<byte[]> GetBytes(string filename, string container, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the specified file as string.
        /// </summary>
        /// <param name="filename">The full filename for the blob to retrieve.</param>
        /// <param name="container">The name of the conatiner where the blob is stored.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>The specified file as string.</returns>
        Task<string> GetString(string filename, string container, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the specified blob in the container for deletion.
        /// <remarks>Snapshots are included in deletion!</remarks>
        /// </summary>
        /// <param name="filename">The name of the blob to delete.</param>
        /// <param name="container">The container.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Response"/>Response</returns> returns true if the file has succesfully been marked for deletion.
        Task<Response<bool>> DeleteFile(string filename, string container, CancellationToken cancellationToken = default);
    }
}