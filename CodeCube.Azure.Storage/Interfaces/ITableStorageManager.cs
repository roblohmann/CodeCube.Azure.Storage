using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace CodeCube.Azure.Storage.Interfaces
{
    public interface ITableStorageManager
    {
        /// <summary>
        /// Insert the specified entity to the table.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to insertin the tablestorage</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        Task<Response> Insert<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new();

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
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <param name="propertiesToSelect"></param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        AsyncPageable<T> Retrieve<T>(string query, IEnumerable<string> propertiesToSelect = null, int pageSize = 25, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

        /// <summary>
        /// Retrieve an entity from the tablestorage
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="partitionKey">The partitionkey of the entity to retrieve.</param>
        /// <param name="rowKey">The rowkey of the entity to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Response<T>> Retrieve<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

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