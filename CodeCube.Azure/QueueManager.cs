using System;
using CodeCube.Azure.Constants;

namespace CodeCube.Azure
{
    public sealed class QueueManager : BaseManager
    {
        private readonly string _queuename;

        internal QueueManager(string connectionstring, string queuename) : base(connectionstring)
        {
            //Validate parameters
            if (string.IsNullOrEmpty(queuename))
            {
                throw new ArgumentNullException(nameof(queuename), ErrorConstants.Queue.QueueNameRequired);
            }

            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring), ErrorConstants.Queue.QueueConnectionstringRequired);
            }

            _queuename = queuename;
        }
    }
}
