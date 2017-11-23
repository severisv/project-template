namespace HappyBever.Files

open System
open Microsoft.Extensions.Options
open Microsoft.WindowsAzure.Storage

[<CLIMutable>]
type FileStorageOptions = {
    ConnectionString: string
}

module FileStorageOptions = 
    let Url = ""


type FileStorage (options: IOptions<FileStorageOptions>) =
    let settings = options.Value


    member this.Save (fileName, containerName, fileStream) =
        let storageAccount = CloudStorageAccount.Parse(settings.ConnectionString)
        let blobClient = storageAccount.CreateCloudBlobClient()
        let container = blobClient.GetContainerReference(containerName)

        let blockBlob = container.GetBlockBlobReference(fileName)


        blockBlob.UploadFromStreamAsync(fileStream) 
                |> Async.AwaitTask 
                |> Async.RunSynchronously

        blockBlob.Uri                 
     

