using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;


namespace BlockDownloadGoogleDrive
{
    internal class GoogleHelper
    {
        internal static async Task<UserCredential> Login(string clientId, string clientSecret, string[] scopes)
        {
            ClientSecrets clientSecrets = new ClientSecrets()
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "user", CancellationToken.None);
        }
        internal static async Task BlockDownloadWithFolderId(DriveService driveService, string folderId)
        {
            string folderMimeType = "application/vnd.google-apps.folder";

            try
            {
                var request = driveService.Files.List();
                request.Q = $"parents in '{folderId}'";

                var fileRequest = await request.ExecuteAsync();
                foreach (var file in fileRequest.Items)
                {
                    if (!file.MimeType.Equals(folderMimeType))
                    {
                        Console.WriteLine($"Đã đọc được file {file.Title}, chuẩn bị chặn donwload");
                        file.Labels.Restricted = true;
                        file.CopyRequiresWriterPermission = true;
                        driveService.Files.Update(file, file.Id).Execute();
                        await Console.Out.WriteLineAsync($"Chặn thành công id : {file.Id}");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Đã đọc được folder {file.Id}");
                        await BlockDownloadWithFolderId(driveService, file.Id);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
