using Azure.Storage.Blobs;
using Azure.Storage;

namespace HamroFashion.Api.V1.Services
{
    public class ImageService
    {
        private readonly string storageAccount = "hamrofashion";
        private readonly string accessKey = "";
        private readonly BlobServiceClient blobServiceClient;

        public ImageService()
        {
            var credential = new StorageSharedKeyCredential(storageAccount, accessKey);
            var blobUri = $"https://{storageAccount}.blob.core.windows.net";
            blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        }

        public async Task<string> UploadImageAsync(string path, string name, string base64, CancellationToken cancellationToken = default)
        {
            string filepath = $"{path}/{name}";
            var blobContainer = blobServiceClient.GetBlobContainerClient("cdn");

            var blob = blobContainer.GetBlobClient(filepath);

            var bytes = Convert.FromBase64String(base64);
            var fileStream = new MemoryStream(bytes);

            try
            {
                await blob.UploadAsync(fileStream, false, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Blob with same name already exists");
            }

            return blob.Uri.AbsoluteUri;

        }

        public async Task<string> EditImageAsync(string path, string name, string base64, CancellationToken cancellationToken = default)
        {
            string filepath = $"{path}/{name}";
            var blobContainer = blobServiceClient.GetBlobContainerClient("cdn");

            var blob = blobContainer.GetBlobClient(filepath);

            var bytes = Convert.FromBase64String(base64);
            var fileStream = new MemoryStream(bytes);

            try
            {
                await blob.UploadAsync(fileStream, true, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.ToString());
            }

            return blob.Uri.AbsoluteUri;

        }

    }
}
