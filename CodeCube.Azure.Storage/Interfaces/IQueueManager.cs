using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace CodeCube.Azure.Storage.Interfaces
{
    /// <summary>
    /// Manager to connect to one single Azure Storage Queue.
    /// </summary>
    public interface IQueueManager
    {
        /// <summary>
        /// Connect to the desired queue and return the client.
        /// </summary>
        /// <returns>The queueclient.</returns>
        Task<QueueClient> ConnectAsync(CancellationToken cancellationToken);
    }
}