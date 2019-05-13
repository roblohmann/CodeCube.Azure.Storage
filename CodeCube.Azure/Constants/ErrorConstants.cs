namespace CodeCube.Azure.Constants
{
    internal sealed class ErrorConstants
    {
        internal const string FileCouldNotBeStored = "File could not be stored in the cloud!";

        internal sealed class Blob
        {
            internal const string BlobAccountRequired = "Accountname for BLOB-storage cannot be empty!";
            internal const string BlobAccesskeyRequired = "Accesskey for BLOB-storage cannot be empty!";
            internal const string BlobConnectionstringRequired = "A valid connectionstring for the BLOB-storage is required!";
        }

        internal sealed class Table
        {
            internal const string TableConnectionstringRequired = "Connectionstring for TABLE-storage cannot be empty!";
            internal const string TableNameRequired = "Tablename for TABLE-storage cannot be empty!";
        }
    }
}
