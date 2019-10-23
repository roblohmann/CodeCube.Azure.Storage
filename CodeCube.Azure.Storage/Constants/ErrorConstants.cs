namespace CodeCube.Azure.Constants
{
    internal static class ErrorConstants
    {
        internal const string InvalidConnectionstring = "An invalid connectionstring was provided!";

        internal static class Blob
        {
            internal const string BlobAccountRequired = "Accountname for BLOB-storage cannot be empty!";
            internal const string BlobAccesskeyRequired = "Accesskey for BLOB-storage cannot be empty!";
            internal const string BlobConnectionstringRequired = "A valid connectionstring for the BLOB-storage is required!";
            internal const string FileCouldNotBeStored = "File could not be stored in the cloud!";
        }

        internal static class Table
        {
            internal const string TableConnectionstringRequired = "Connectionstring for TABLE-storage cannot be empty!";
            internal const string TableNameRequired = "Tablename for TABLE-storage cannot be empty!";

            internal const string RowKeyIsRequired = "RowKey is required!";
            internal const string PartitionKeyIsRequired = "PartitionKey is required!";
        }

        internal static class Queue
        {
            internal const string QueueNameRequired = "Name for the queue is required!";
            internal const string QueueConnectionstringRequired = "Connectionstring for the queue is required!";
        }
    }
}
