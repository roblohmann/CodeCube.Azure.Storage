using System;
using Xunit;

namespace CodeCube.Azure.Storage.Tests.Factory
{
    public class BlobStorageManager
    {
        // [Fact]
        // public void Get()
        // {
        //     //Setup            
        //     const string mockUri = "http://google.com";
        //     const string mockStorageAccountName = "mockStorageAccountName";
        //     const string mockAccessKey = "mockAccessKey";
        //     var factory = new StorageFactory();
        //
        //     //Act
        //     var manager = factory.GetBlobStorageManager(mockUri, mockStorageAccountName, mockAccessKey);
        //
        //     //Assert
        //     Assert.NotNull(manager);
        // }

        [Fact]
        public void Get_WithoutStorageAccountNameAndKey_Throws_Exception()
        {
            //Setup            
            const string mockUri = "mockUri";
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(mockUri,null,null));
            Assert.Equal("Accountname for BLOB-storage cannot be empty! (Parameter 'blobStorageAccountname')", exception.GetBaseException().Message);
        }


        [Fact]
        public void Get_WithoutUri_ThrowsException()
        {
            //Setup            
            const string mockStorageAccountName = "mockStorageAccountName";
            const string mockAccessKey = "mockAccessKey";
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(null, mockStorageAccountName,mockAccessKey));
            Assert.Equal("URI for BLOB-storage cannot be empty! (Parameter 'uri')", exception.GetBaseException().Message);
        }
        
        [Fact]
        public void Get_WithoutAccessKey_ThrowsException()
        {
            //Setup            
            const string mockUri = "http://google.com";
            const string mockStorageAccountName = "mockStorageAccountName";
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(mockUri,mockStorageAccountName,null));
            Assert.Equal("Accesskey for BLOB-storage cannot be empty! (Parameter 'blobAccesskey')", exception.GetBaseException().Message);
        }
    }
}
