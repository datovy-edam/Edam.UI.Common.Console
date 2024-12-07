using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Application;

namespace Edam.DataObjects.Models;

/// <summary>
/// An Application Element is represented by a node that contains details of a
/// complex element that in turn can have child elements.  Those are generally
/// provided in a JSON document that list data elements.
/// 
/// For example:
///
/// [
///    {
///       "Title": "Connection String",
///       "Name": "ConnectionString",
///       "MinLength": "0",
///       "MaxLength": "1024",
///       "SerialNo": "1"
///    }
/// ]
/// 
/// The previous describes a single data element.  The supported attributes are 
/// listed in the "DataTemplates/TemplateItem.txt".
/// </summary>
public class ApplicationElementInfo : ElementInfo
{

    private const string JSON_EXT = "json";
    private const string DATA_TEMPLATES_FOLDER = "DataTemplates";

    /// <summary>
    /// Read a template file that describe a list of elements.
    /// </summary>
    /// <param name="fileName">file path name.</param>
    /// <param name="name"></param>
    /// <param name="description">description</param>
    /// <returns>an element node is returned</returns>
    public static ElementNodeInfo GetElementNode(
       string fileName, string name, string description)
    {
        string filePath = Session.GetApplicationFullPath(
           DATA_TEMPLATES_FOLDER, fileName, JSON_EXT);
        ElementNodeInfo info = ElementNodeInfo.FromJsonFile(filePath, name,
           description);
        return info;
    }

}


