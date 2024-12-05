using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Application;

namespace Edam.UI.Application
{

   public class ApplicationCode
   {

      public const string KEY_STATUS = "STATUS";
      public const string KEY_RATING_RESULT_TYPE = "RATING_RESULT_TYPE";
      public const string KEY_RATING_TYPE = "RATING_TYPE";
      public const string KEY_PARTICIPANT_ROLE = "PARTICIPANT_ROLE";

      private static Cache ApplicationCache { get; set; } = new Cache();

      public static void CacheSet<T>(string key, T item)
      {
         ApplicationCache.Set(key, item);
      }
      public static T CacheGet<T>(string key)
      {
         return ApplicationCache.Get<T>(key);
      }

   }

}
