using System;
using Xunit;

namespace CodeCube.Azure.Storage.Tests.Factory;

public class QueueManager
{
    [Fact]
    public void Get()
    {
        //Setup            
        const string mockConnectionstring = "MockConnectionstring";
        const string mockQueueName = "MockQueueName";
        var factory = new StorageFactory();

        //Act
        var manager = factory.GetQueueManager(mockConnectionstring, mockQueueName);

        //Assert
        Assert.NotNull(manager);
    }

    [Fact]
    public void Get_WithoutConnectionstring_ThrowsException()
    {
        //Setup            
        const string mockQueueName = "MockQueueName";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetQueueManager(null, mockQueueName));
        Assert.Equal("Name for the queue is required! (Parameter 'connectionstring')", exception.GetBaseException().Message);
    }


    [Fact]
    public void Get_WithoutQueueName_ThrowsException()
    {
        //Setup            
        const string mockConnectionstring = "MockConnectionstring";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetQueueManager(mockConnectionstring, null));
        Assert.Equal("Connectionstring for the queue is required! (Parameter 'queueName')", exception.GetBaseException().Message);
    }
}