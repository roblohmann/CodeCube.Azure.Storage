namespace CodeCube.Azure.Storage
{
    public abstract class BaseManager
    {
        /// <summary>
        /// The connectionstring to the storage
        /// </summary>
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