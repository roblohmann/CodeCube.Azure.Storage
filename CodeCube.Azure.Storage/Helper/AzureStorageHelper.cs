using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public static class AzureStorageHelper
{
    private static readonly HashSet<string> _createdResources = [];
    private static readonly object _lock = new();

    /// <summary>
    /// Ensures a blob container exists, using thread-safe creation to avoid 409 Conflict errors.
    /// Only attempts creation once per container name across all threads.
    /// </summary>
    /// <param name="container">The blob container client</param>
    /// <param name="containerName">The name of the container (used as a unique key for tracking)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task EnsureBlobContainerExistsAsync(
        BlobContainerClient container,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        var key = $"blob:{containerName}";

        if (_createdResources.Contains(key))
            return;

        bool shouldCreate;
        lock (_lock)
        {
            shouldCreate = !_createdResources.Contains(key);
            if (shouldCreate)
                _createdResources.Add(key);
        }

        if (shouldCreate)
        {
            try
            {
                await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            }
            catch (RequestFailedException ex) when (ex.Status == 409)
            {
                // Container already exists, ignore the conflict
            }
        }
    }

    /// <summary>
    /// Ensures a blob container exists (synchronous version), using thread-safe creation to avoid 409 Conflict errors.
    /// Only attempts creation once per container name across all threads.
    /// </summary>
    /// <param name="container">The blob container client</param>
    /// <param name="containerName">The name of the container (used as a unique key for tracking)</param>
    public static async Task EnsureBlobContainerExists(
        BlobContainerClient container,
        string containerName,
        CancellationToken cancellationToken)
    {
        var key = $"blob:{containerName}";

        if (_createdResources.Contains(key))
            return;

        bool shouldCreate;
        lock (_lock)
        {
            shouldCreate = !_createdResources.Contains(key);
            if (shouldCreate)
                _createdResources.Add(key);
        }

        if (shouldCreate)
        {
            try
            {
                await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);
            }
            catch (RequestFailedException ex) when (ex.Status == 409)
            {
                // Container already exists, ignore the conflict
            }
        }
    }

    /// <summary>
    /// Ensures a table exists, using thread-safe creation to avoid 409 Conflict errors.
    /// Only attempts creation once per table name across all threads.
    /// </summary>
    /// <param name="tableClient">The table client</param>
    /// <param name="tableName">The name of the table (used as a unique key for tracking)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task EnsureTableExistsAsync(
        TableClient tableClient,
        string tableName,
        CancellationToken cancellationToken = default)
    {
        var key = $"table:{tableName}";

        if (_createdResources.Contains(key))
            return;

        bool shouldCreate;
        lock (_lock)
        {
            shouldCreate = !_createdResources.Contains(key);
            if (shouldCreate)
                _createdResources.Add(key);
        }

        if (shouldCreate)
        {
            try
            {
                await tableClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            }
            catch (RequestFailedException ex) when (ex.Status == 409)
            {
                // Table already exists, ignore the conflict
            }
        }
    }

    /// <summary>
    /// Ensures a table exists (synchronous version), using thread-safe creation to avoid 409 Conflict errors.
    /// Only attempts creation once per table name across all threads.
    /// </summary>
    /// <param name="tableClient">The table client</param>
    /// <param name="tableName">The name of the table (used as a unique key for tracking)</param>
    public static void EnsureTableExists(
        TableClient tableClient,
        string tableName)
    {
        var key = $"table:{tableName}";

        if (_createdResources.Contains(key))
            return;

        bool shouldCreate;
        lock (_lock)
        {
            shouldCreate = !_createdResources.Contains(key);
            if (shouldCreate)
                _createdResources.Add(key);
        }

        if (shouldCreate)
        {
            try
            {
                tableClient.CreateIfNotExists();
            }
            catch (RequestFailedException ex) when (ex.Status == 409)
            {
                // Table already exists, ignore the conflict
            }
        }
    }

    /// <summary>
    /// Clears the tracking of created resources. Useful for testing purposes.
    /// </summary>
    public static void ResetCreatedResources()
    {
        lock (_lock)
        {
            _createdResources.Clear();
        }
    }
}