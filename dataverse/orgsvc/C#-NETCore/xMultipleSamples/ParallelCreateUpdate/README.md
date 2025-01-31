﻿# ParallelCreateUpdate README

This project uses the 
[System.Threading.Tasks.Parallel.ForEachAsync Method](https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.foreachasync?view=net-6.0) 
together with the 
[Microsoft.PowerPlatform.Dataverse.Client.ServiceClient.ServiceClient.CreateAsync Method](https://learn.microsoft.com/dotnet/api/microsoft.powerplatform.dataverse.client.serviceclient.createasync?view=dataverse-sdk-latest) 
and [Microsoft.PowerPlatform.Dataverse.Client.ServiceClient.ServiceClient.UpdateAsync Method](https://learn.microsoft.com/dotnet/api/microsoft.powerplatform.dataverse.client.serviceclient.updateasync?view=dataverse-sdk-latest) 
to perform individual create and update operations using multiple threads.

It depends on the common structure for other projects in this solution that is described in [CreateUpdateMultiple/README.md](../README.md).

The number of threads used will depend on the [ServiceClient.RecommendedDegreesOfParallelism Property](https://learn.microsoft.com/dotnet/api/microsoft.powerplatform.dataverse.client.serviceclient.recommendeddegreesofparallelism?view=dataverse-sdk-latest), which is based on the value of the `x-ms-dop-hint` response header. The `x-ms-dop-hint` response header provides a hint for the Degree Of Parallelism (DOP) that represents a number of threads that should provide good results for a given environment.

The output of this project will look like this:

```
Creating sample_Example table...
        sample_Example table created.
Adding 'sample_Description' column to sample_Example table...
        'sample_Description' column created.
Preparing 100 records to create..

RecommendedDegreesOfParallelism:24

Sending create requests in parallel...
        Created 100 records in 1 seconds.

Preparing 100 records to update..
Sending update requests in parallel...
        Updated 100 records in 1 seconds.

Starting asynchronous bulk delete of 100 created records...
        Asynchronous job to delete 100 records completed in 21 seconds.
        Bulk Delete status: Succeeded
Deleting sample_Example table...
        sample_Example table deleted.
```

