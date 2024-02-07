using Google.Apis.Drive.v2;
using Google.Apis.Services;
using System.Text;

namespace BlockDownloadGoogleDrive
{
    internal class Program
    {
        private static string clientId = "597540699804-nd7rmi77jt0fjvmkh3pfhdrduv0ct7bv.apps.googleusercontent.com";
        private static string clientSecret = "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";

        private static string[] scopes = new[] { "https://www.googleapis.com/auth/drive", "https://www.googleapis.com/auth/drive.file" };
        static async Task Main(string[] args)
        {
            try
            {
                Console.InputEncoding = Console.OutputEncoding = Encoding.Unicode;
                var userCredential = await GoogleHelper.Login(clientId, clientSecret, scopes);
                using var driveService = new DriveService(
                    new BaseClientService.Initializer() { HttpClientInitializer = userCredential }
                    );

                await Console.Out.WriteAsync("FolderId: ");
                string folderId = Console.ReadLine() ?? String.Empty;
                await GoogleHelper.BlockDownloadWithFolderId(driveService, folderId);
                await Console.Out.WriteLineAsync("Hoàn tất");
            }
            catch (Exception e)
            {

                await Console.Out.WriteLineAsync(e.Message);
                Console.ReadLine();
            }

        }

    }
}
