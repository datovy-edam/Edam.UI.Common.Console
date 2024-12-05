using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.Application.Settings
{

   public class EdamSettings
   {
      public class SchemaInfo
      {
         public class DefaultsInfo
         {
            public string TypePostfix { get; set; }
            public string DocumentPostfix { get; set; }
         }

         public DefaultsInfo Defaults = new DefaultsInfo();
      }

      public class AppInfo
      {
         public class DefaultsInfo
         {
            public string OrganizationId { get; set; }
         }

         public DefaultsInfo Defaults = new DefaultsInfo();
         public string ConsolePath { get; set; }
         public List<UriItemInfo> UriList { get; set; } = 
            new List<UriItemInfo>();
      }

      public class DataSourceInfo
      {
         public string DefaultConnectionString { get; set; }
      }

      public AppInfo App = new AppInfo();
      public SchemaInfo Schema = new SchemaInfo();
      public DataSourceInfo DataSource = new DataSourceInfo();

   }

}
