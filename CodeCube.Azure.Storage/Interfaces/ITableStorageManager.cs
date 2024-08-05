using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace CodeCube.Azure.Storage.Interfaces
{
    public interface ITableStorageManager
    {
        /// <summary>
        /// Retrieve an entity from the tablestorage
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="partitionKey">The partitionkey of the entity to retrieve.</param>
        /// <param name="rowKey">The rowkey of the entity to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Response<T>> GetSingle<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve an entities of the given type.
        /// </summary>
        /// <param name="query">The query to use for filtering entites.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>        
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        Task<T?> GetSingle<T>(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve all entities of the given type.
        /// Pagesize is required to be able to handle large tables.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="continuationToken">The continuationToken. Should be null for the first request. For the following request the identifier (eg item1) from the Tuple should be used.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>Tuple with the results. Item1 is should be used as continuationtoken for the next request. Item2 is a strong type collection with an max number of items matching the pageSize.</returns>
        Task<Tuple<string, IEnumerable<T>>?> GetAll<T>(string continuationToken, int pageSize, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve all entities of the given type based on the provided filter.
        /// </summary>
        /// <param name="rangeQuery">The query to use for filtering entites.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        Task<List<T>> Query<T>(string rangeQuery, int pageSize = 25, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve all entities of the given type based on the provided filter.
        /// </summary>
        /// <param name="rangeQuery">The query to use for filtering entites.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="propertiesToSelect">The properties to select for the result.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        Task<List<T>> Query<T>(string rangeQuery, int pageSize = 25, IEnumerable<string> propertiesToSelect = null, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve all entities of the given type based on the provided filter.
        /// </summary>
        /// <param name="query">The query to use for filtering entites.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        Task<List<T>> Query<T>(Expression<Func<T, bool>> query, int pageSize = 25, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve all entities of the given type based on the provided filter.
        /// </summary>
        /// <param name="query">The query to use for filtering entites.</param>
        /// <param name="propertiesToSelect">The properties eg coluns to select from your tableEntity.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        Task<List<T>> Query<T>(Expression<Func<T, bool>> query, IEnumerable<string> propertiesToSelect = null, int pageSize = 25, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve all entities of the given type based on the filter.
        /// Pagesize is required to be able to handle large tables.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="continuationToken">The continuationToken. Should be null for the first request. For the following request the identifier (eg item1) from the Tuple should be used.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns>Tuple with the results. Item1 is should be used as continuationtoken for the next request. Item2 is a strong type collection with an max number of items matching the pageSize.</returns>
        Task<Tuple<string, IEnumerable<T>>?> QueryAll<T>(Expression<Func<T, bool>> query, string continuationToken, int pageSize, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        

        /// <summary>
        /// Insert the specified entity to the table.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to insertin the tablestorage</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        Task<Response> Insert<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new();

        /// <summary>
        /// Insert the specified entity to the table.
        /// Entity will be replaced if already exists.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to insertin the tablestorage</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        Task<Response> InsertOrReplace<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new();

        /// <summary>
        /// Insert a batch of entities.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entities">The batch of entities to insert.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        Task<Response<IReadOnlyList<Response>>> InsertBatch<T>(List<T> entities, CancellationToken cancellationToken = default) where T : ITableEntity;

        /// <summary>
        /// Update the specified entity in the table storage.
        /// </summary>
        /// <typeparam name="T">The type for the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        Task<Response> Update<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new();

        /// <summary>
        /// Replace the specified entity in the table storage.
        /// </summary>
        /// <typeparam name="T">The type for the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to replace.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        Task<Response> Replace<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new();

        /// <summary>
        /// Replace a batch of entities via the UpdateReplace method.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entities">The batch of entities to replace.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        Task<Response<IReadOnlyList<Response>>> ReplaceBatch<T>(List<T> entities, CancellationToken cancellationToken = default) where T : ITableEntity;

        /*
        /// <summary>
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query">The query to use for filtering entites. Eg: PartitionKey eq 'myPartitionKey' </param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>        
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        [Obsolete("Will be removed in a future version. Please use overload which returns List<T>")]
        AsyncPageable<T> Query<T>(string query, int pageSize = 25, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <param name="propertiesToSelect"></param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        [Obsolete("Will be removed in a future version. Please use overload which returns List<T>")]
        AsyncPageable<T> Query<T>(string query, IEnumerable<string> propertiesToSelect = null, int pageSize = 25, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        */

        /// <summary>
        /// Delete the specified entity from the tablestorage.
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="rowKey"></param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        Task<Response> Delete<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : ITableEntity, new();
    }
}