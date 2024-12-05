using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

// -----------------------------------------------------------------------------

namespace Edam.UI.Common.InOut
{

   public class StorageFolderFileHelper
   {

      public static StorageFolder GetFolder(string folderPath)
      {
         var l = Windows.ApplicationModel.Package.Current.InstalledLocation;
         var folder = l.GetFolderAsync(folderPath).GetAwaiter();
         return folder.GetResult();
      }

      public static List<StorageFile> GetFolderFiles(StorageFolder folder)
      {
         var files = folder.GetFilesAsync().GetAwaiter();
         return files.GetResult().ToList();
      }

      public static string ReadText(StorageFile file)
      {
         var data = Windows.Storage.FileIO.ReadTextAsync(file).GetAwaiter();
         return data.GetResult();
      }
   }

}
