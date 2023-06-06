# CodeCube.Azure
Helper classes to connect to Microsoft Azure Blob- and Table-Storage

![Nuget](https://img.shields.io/nuget/dt/CodeCube.Azure.Storage?style=for-the-badge)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/CodeCube.Azure.Storage?style=for-the-badge)
![GitHub](https://img.shields.io/github/license/roblohmann/CodeCube.Azure.Storage?style=for-the-badge)

## Version 1.x
Version 1.x of this package is supporting Windows.Azure.Storage untill version 9.3.3. As of version 9.4.0 that particular package has been split into multiple smaller packages.

This version won't be maintained anymore!

## Version 2.x 
Version 2.x of this package supports the following microsoft packages;

- Microsoft.Azure.Storage.Blob
- Microsoft.Azure.Storage.Queue
- Microsoft.Azure.Cosmos.Table

# How to use
1. In your startup.cs add the following line of code;

    service.AddScoped<IStorageFactory, StorageFactory>();

2.