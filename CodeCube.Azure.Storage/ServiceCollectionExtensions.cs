using CodeCube.Azure.Storage;
using CodeCube.Azure.Storage.Interfaces;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class with extenion methods specific for usage with CodeCube.Azure.Storage
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds an singleton instance of <see cref="IStorageFactory"/> to the DI-container.
    /// The <see cref="IStorageFactory"/> can be used to connect to BlobStorage, TableStorage or Azure Queues.
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddStorageFactory(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IStorageFactory, StorageFactory>();
    }

    /// <summary>
    /// Adds an instance of <see cref="IBlobStorageManager"/> to the DI-container.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="storageUrl">The URL for the storage account to connect to.</param>
    /// <param name="accountName">The name of the storage account to connect to.</param>
    /// <param name="accessKey">The AccessKey to use when connection.</param>
    public static void AddBlobStorageManager(this IServiceCollection serviceCollection, string storageUrl, string accountName, string accessKey)
    {
        serviceCollection.AddSingleton<IStorageFactory, StorageFactory>();

        serviceCollection.AddScoped<IBlobStorageManager>(s =>
        {
            var storageFactory = s.GetRequiredService<IStorageFactory>();
            return storageFactory.GetBlobStorageManager(storageUrl, accountName, accessKey);
        });
    }

    /// <summary>
    /// Adds an instance of <see cref="IBlobStorageManager"/> to the DI-container.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="connectionString">The connectionstring of the BLOB-storage to connect to.</param>
    public static void AddBlobStorageManager(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddSingleton<IStorageFactory, StorageFactory>();

        serviceCollection.AddScoped<IBlobStorageManager>(s =>
        {
            var storageFactory = s.GetRequiredService<IStorageFactory>();
            return storageFactory.GetBlobStorageManager(connectionString);
        });
    }

    /// <summary>
    /// Adds an instance of the <see cref="ITableStorageManager"/> to the DI-container.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="tableConnectionstring">The connectionstring of the tablestorage to connect to.</param>
    /// <param name="tableName">The name of the table to connect to.</param>
    public static void AddTableStorageManager(this IServiceCollection serviceCollection, string tableConnectionstring, string tableName)
    {
        serviceCollection.AddSingleton<IStorageFactory, StorageFactory>();

        serviceCollection.AddScoped<ITableStorageManager>(s =>
        {
            var storageFactory = s.GetRequiredService<IStorageFactory>();
            return storageFactory.GetTableStorageManager(tableConnectionstring, tableName);
        });
    }

    /// <summary>
    /// Adds an instance of the <see cref="IQueueManager"/> to the DI-container.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="connectionstring">The connectionstring of the queue to connect to.</param>
    /// <param name="queueName">The name of the queue to connect to.</param>
    public static void AddQueueManager(this IServiceCollection serviceCollection, string connectionstring, string queueName)
    {
        serviceCollection.AddSingleton<IStorageFactory, StorageFactory>();

        serviceCollection.AddScoped<IQueueManager>(s =>
        {
            var storageFactory = s.GetRequiredService<IStorageFactory>();
            return storageFactory.GetQueueManager(connectionstring, queueName);
        });
    }
}