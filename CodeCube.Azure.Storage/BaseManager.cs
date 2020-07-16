using Azure.Storage.Blobs;
using System;

namespace CodeCube.Azure
{
    public abstract class BaseManager
    {
        protected readonly string Connectionstring;

        protected BaseManager()
        {
            
        }
        protected BaseManager(string connectionstring)
        {
            Connectionstring = connectionstring;
        }
    }
}