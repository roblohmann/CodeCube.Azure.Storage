using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace CodeCube.Azure.Storage.Interfaces
{
    public interface ITableStorageManager
    {
        Task<Response> Insert<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new();
        Task<Response<IReadOnlyList<Response>>> InsertBatch<T>(List<T> entities, CancellationToken cancellationToken = default) where T : ITableEntity;
        Task<Response> Update<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new();
        AsyncPageable<T> Retrieve<T>(string query, IEnumerable<string> propertiesToSelect = null, int pageSize = 25, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task<Response<T>> Retrieve<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task<Response> Delete<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : ITableEntity, new();
    }
}