using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace diploma1
{
  public static class Helpers
  {
    public static async Task<byte[]> ToByteArray(this StorageFile file)
    {
      using(MemoryStream ms = new MemoryStream())
      {
        var stream = await file.OpenStreamForReadAsync();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
      }
    }

//     public static byte[] ToByteArray(this BitmapImage image)
//     {
//       byte[] data;
//       var encoder = new GifBitmapEncoder();
//       encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
//       using (MemoryStream ms = new MemoryStream())
//       {
//         encoder.Save(ms);
//         data = ms.ToArray();
//       }
// 
// 
// 
//       MemoryStream ms = new MemoryStream();
//       image.Save(ms, Imaging.ImageFormat.Gif);
//       //Windows.UI.Xaml.Media.Imaging.
//       return ms.ToArray();
//     }
// 
//     public Image byteArrayToImage(byte[] byteArrayIn)
//     {
//       MemoryStream ms = new MemoryStream(byteArrayIn);
//       Image returnImage = Image.FromStream(ms);
//       return returnImage;
//     }


    static async public Task<StorageFile> LoadImage()
    {
      FileOpenPicker openPicker = new FileOpenPicker();

      openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
      openPicker.ViewMode = PickerViewMode.Thumbnail;

      // Filter to include a sample subset of file types.
      openPicker.FileTypeFilter.Clear();
      openPicker.FileTypeFilter.Add(".bmp");
      openPicker.FileTypeFilter.Add(".png");
      openPicker.FileTypeFilter.Add(".jpeg");
      openPicker.FileTypeFilter.Add(".jpg");

      // Open the file picker.
      return await openPicker.PickSingleFileAsync();
    }


    static async public Task<BitmapImage> LoadImage(StorageFile file)
    {
      // file is null if user cancels the file picker.
      if (file == null)
        return null;

      // Open a stream for the selected file.
      IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);

      // Set the image source to the selected bitmap.
      BitmapImage bitmapImage = new BitmapImage();

      bitmapImage.SetSource(fileStream);
      return bitmapImage;
    }


    static public async Task<BitmapImage> LoadImageAsync(byte[] array)
    {
      BitmapImage bitmapImage = new BitmapImage();
      using (var stream = new InMemoryRandomAccessStream())
      {
        stream.AsStreamForWrite().Write(array, 0, array.Length);
        stream.Seek(0);
        await bitmapImage.SetSourceAsync(stream);
      }
      return bitmapImage;
    }

    static public BitmapImage LoadImage(byte[] array)
    {
      BitmapImage bitmapImage = new BitmapImage();
      using (var stream = new InMemoryRandomAccessStream())
      {
        stream.AsStreamForWrite().Write(array, 0, array.Length);
        stream.Seek(0);
        bitmapImage.SetSource(stream);
      }
      return bitmapImage;
    }
  }
}
