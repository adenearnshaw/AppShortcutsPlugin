using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;

namespace Plugin.AppShortcuts
{
    public class EmbeddedImageHelper
    {
        private StorageFolder _appDataAssetsFolder;

        public EmbeddedImageHelper()
        {
            Initialize();
        }

        private async void Initialize()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            _appDataAssetsFolder = await localFolder.CreateFolderAsync("assets", CreationCollisionOption.OpenIfExists);
        }

        public async Task<Uri> CopyEmbeddedImageToAppData(string fileName)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var fullyQualifiedName = $"Plugin.AppShortcuts.Assets.{fileName}";
            var imageStream = assembly.GetManifestResourceStream(fullyQualifiedName);

            var filePath = await SaveStreamToAppData(fileName, imageStream);

            return new Uri(filePath);
        }

        private async Task<string> SaveStreamToAppData(string fileName, Stream fileStream)
        {
            var existingFile = await _appDataAssetsFolder.TryGetItemAsync(fileName);
            if (existingFile != null)
                return ConvertToAppDataPath(fileName);

            var file = await _appDataAssetsFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream.AsRandomAccessStream());
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();


                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream.AsRandomAccessStream());
                encoder.SetSoftwareBitmap(softwareBitmap);
                encoder.IsThumbnailGenerated = true;

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    switch (err.HResult)
                    {
                        case unchecked((int)0x88982F81): //WINCODEC_ERR_UNSUPPORTEDOPERATION
                                                         // If the encoder does not support writing a thumbnail, then try again
                                                         // but disable thumbnail generation.
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw err;
                    }
                }

                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }
            }

            return ConvertToAppDataPath(fileName);
        }

        private string ConvertToAppDataPath(string fileName)
        {
            return $"ms-appdata:///local/assets/{fileName}";
        }
    }
}
