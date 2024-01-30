using System;
using Xunit;

namespace CodeCube.Azure.Storage.Tests.Factory;

public class TableStorageManager
{
    [Fact]
    public void Get_With_Invalid_Connectionstring_Throws_Exception()
    {
        //Setup            
        const string mockTableName = "MockTableName";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => factory.GetTableStorageManager("MockConnectionString", mockTableName));
        Assert.Equal("Connection string doesn't have value for keyword 'MockConnectionString'.", exception.GetBaseException().Message);
    }

    [Fact]
    public void Get_WithoutConnectionstring_ThrowsException()
    {
        //Setup            
        const string mockTableName = "MockTableName";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetTableStorageManager(null, mockTableName));
        Assert.Equal("Connectionstring for tablestorage cannot be empty! (Parameter 'tableConnectionstring')", exception.GetBaseException().Message);
    }


    [Fact]
    public void Get_WithoutTableName_ThrowsException()
    {
        //Setup            
        const string mockConnectionstring = "MockConnectionstring";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetTableStorageManager(mockConnectionstring, null));
        Assert.Equal("Tablename for tablestorage cannot be empty! (Parameter 'tableName')", exception.GetBaseException().Message);
    }
}