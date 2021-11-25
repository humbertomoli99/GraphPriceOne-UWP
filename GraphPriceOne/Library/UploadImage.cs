using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphPriceOne.Library
{
    public class UploadImage
    {
        private BitmapImage _bitmapImage;
        private byte[] avatar;
        public static async Task<ImageSource> FromStorageFileAsync(StorageFile sf)
        {
            using (var randomAccessStream = await sf.OpenAsync(FileAccessMode.Read))
            {
                var result = new BitmapImage();
                await result.SetSourceAsync(randomAccessStream);
                return result;
            }
        }
        public static async Task<string> SaveImageAsync(StorageFile sourceFile, string folder, string nameFile)
        {
            string root = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

            string format = Path.GetExtension(sourceFile.Name);
            string OuputNamefileFull = nameFile + format;

            DirectoryInfo pathExit = Directory.CreateDirectory(root + folder);
            StorageFolder folderExit = await StorageFolder.GetFolderFromPathAsync(pathExit.ToString());

            string FileExit = Path.Combine(pathExit.ToString(), OuputNamefileFull);



            //if (sourceFile.Name != OuputNamefileFull)
            //{
            //    await sourceFile.RenameAsync(OuputNamefileFull);
            //}
            //detectar si hay un archivo con el nombre de ouputfilename en la carpeta stores
            if (File.Exists(FileExit))
            {
                StorageFile FileReplaceExit = await StorageFile.GetFileFromPathAsync(FileExit);
                await FileReplaceExit.DeleteAsync();

                await sourceFile.CopyAsync(folderExit);
            }
            else
            {
                //StorageFile FileReplaceExit = await StorageFile.GetFileFromPathAsync(FileExit);
                await sourceFile.CopyAsync(folderExit);

                StorageFile fileChangeName = await StorageFile.GetFileFromPathAsync(pathExit + sourceFile.Name);
                await fileChangeName.RenameAsync(OuputNamefileFull);

                //await folderExit.RenameAsync(OuputNamefileFull);
                //await FileReplaceExit.RenameAsync(OuputNamefileFull);
            }
            ////StorageFile filesaved = await StorageFile.GetFileFromPathAsync(Path.Combine(pathExit.ToString(), OuputNamefileFull));




            ////IStorageFile ouputFile = await StorageFile.GetFileFromPathAsync(filesaved);


            ////carpeta temporal
            //DirectoryInfo SFolderTemp = Directory.CreateDirectory(root + @"\temp\");
            //StorageFolder FolderTemp = await StorageFolder.GetFolderFromPathAsync(SFolderTemp.ToString());

            //StorageFile sourceFile2 = await sourceFile.CopyAsync(FolderTemp);

            //// mueve el archivo a la carpeta temporal
            //await sourceFile2.RenameAsync(OuputNamefileFull);

            //if (File.Exists(filesaved.ToString()))
            //{
            //    IStorageFile ouputFile = await StorageFile.GetFileFromPathAsync(filesaved);
            //    await sourceFile2.CopyAndReplaceAsync(ouputFile);
            //}
            //else
            //{
            //    StorageFile a = await sourceFile2.CopyAsync(folderExit);
            //}
            ////await sourceFile2.CopyAndReplaceAsync(ouputFile);
            //await FolderTemp.DeleteAsync();
            ////await sourceFile.RenameAsync(OuputNamefileFull);

            ////if (File.Exists(ouputFile))
            ////{
            ////    StorageFile SFfilesaved = await StorageFile.GetFileFromPathAsync(ouputFile);
            ////    await SFfilesaved.DeleteAsync();
            ////}
            //// 1 copiar el archivo en la carpeta temporal

            //// 2 renombrar el archivo en la carpeta temporal
            //// 3 mover el archivo a la carpeta final
            //// 4 eliminar la carpeta temporal
            //// 5 guardar el nombre del archivo final
            //////if (!File.Exists(ouputFile))
            //////{
            //////    StorageFile pathString = await StorageFile.GetFileFromPathAsync(ouputFile);
            //////    await sourceFile.CopyAsync(folderExit);
            //////    await sourceFile.RenameAsync(OuputNamefileFull);
            //////}
            //////else
            //////{
            //////    if (ouputFile != null)
            //////    {
            //////        StorageFile pathString = await StorageFile.GetFileFromPathAsync(ouputFile);
            //////        await pathString.DeleteAsync();
            //////        await sourceFile.RenameAsync(OuputNamefileFull);
            //////        await sourceFile.CopyAndReplaceAsync(pathString);
            //////    }
            //////    //await sourceFile.CopyAsync(folderExit);
            //////}

            ////cambiar este valor
            return OuputNamefileFull;
        }
        public async Task<object[]> LoadImageAsync()
        {
            avatar = null;
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".webp");// esta talvez no
            _bitmapImage = new BitmapImage();
            StorageFile file = await picker.PickSingleFileAsync();
            //await SaveImageAsync(file, @"/Stores/",file.Name);
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    _bitmapImage.SetSource(fileStream);
                }
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BinaryReader reader = new BinaryReader(fileStream.AsStream());
                    avatar = reader.ReadBytes((int)fileStream.Size);
                }
            }
            object[] objects = { avatar, _bitmapImage, file };
            return objects;
        }
        public async Task<byte[]> ImagebyteAsync(BitmapImage image)
        {
            RandomAccessStreamReference streamRef = RandomAccessStreamReference.CreateFromUri(image.UriSource);
            IRandomAccessStreamWithContentType streamWithContent = await streamRef.OpenReadAsync();
            BinaryReader reader = new BinaryReader(streamWithContent.AsStream());
            avatar = reader.ReadBytes((int)streamWithContent.Size);
            return avatar;
        }
        public async Task<BitmapImage> ImageFromBufferAsync(byte[] byteAvatar)
        {
            _bitmapImage = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(byteAvatar);
                    await writer.StoreAsync();
                }
                await _bitmapImage.SetSourceAsync(stream);
            }
            return _bitmapImage;
        }
    }
}
