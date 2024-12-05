using Edam.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.DataObjects.ReferenceData;
using Edam.UI.Common.InOut;

namespace Edam.UI.Common.Models.ReferenceData
{

   public class ReferenceDataTemplateFileReader : IReferenceDataTemplateReader
   {

      public static ReferenceDataTemplateInfo
         ToTemplateInfo(String jsonText)
      {
         ReferenceDataTemplateInfo template =
            ReferenceDataTemplateInfo.FromJson(jsonText);
         return template;
      }

      /// <summary>
      /// Read given folder looking for template files...
      /// </summary>
      /// <param name="folderPath">non-empty folder path</param>
      /// <returns>list of templates is returned</returns>
      public List<ReferenceDataTemplateInfo> 
         FromFolder(string folderPath)
      {
         Windows.Storage.StorageFolder folder =
            StorageFolderFileHelper.GetFolder(folderPath);
         List<Windows.Storage.StorageFile> files = 
            StorageFolderFileHelper.GetFolderFiles(folder);

         List<ReferenceDataTemplateInfo> list =
            new List<ReferenceDataTemplateInfo>();

         foreach (var i in files)
         {
            string data = StorageFolderFileHelper.ReadText(i);
            ReferenceDataTemplateInfo l = ToTemplateInfo(data);
            list.Add(l);
         }
         return list;
      }

   }

}
