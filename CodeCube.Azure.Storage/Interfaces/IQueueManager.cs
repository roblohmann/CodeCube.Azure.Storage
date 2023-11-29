using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace CodeCube.Azure.Storage.Interfaces
{
    public interface IQueueManager
    {
        /// <summary>
        /// Connect to the desired queue and return the client.
        /// </summary>
        /// <returns>The queueclient.</returns>
        Task<QueueClient> ConnectAsync(CancellationToken cancellationToken);
    }
}