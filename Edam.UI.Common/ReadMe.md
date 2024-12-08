# Common UI Resources

This project contains reusable resources on common UI required User Controls including:

*Diagnostics Support*
- DiagnosticsLogControl.xmls - control that uses existing Edam.Diagnositcs resources to display logged exceptions and issues.
- DiagnosticsLogSidePanel.xamls - hosts the Diagnostics Log control with a drawer that expands upon request to display the log.

*Identity Login - Logout* (see ""Identity Management and Services"" ahead)
- KeyPadControl.xamls - display a key-pad that allows to enter codes.
- AccountLoginControl.xamls - gather an organization id, user id and password.
- AccountPinLoginControl.xamls - using the KeyPad allow user to enter a pin number.

Visible UI Resources within these libraries are targeting the Uno Platform.  In
many instances it will be found that those have been written to support any kind
of platform and eventually may be supporting Maui and Blazor.

Other Common resources and functionality are documented ahead.

## Main Menu and Navigation

Each UI platform offers a collection of classes and resources to support navigation.
To be agnostic to the specifics of an underlying UI some classes were created to
do generic Menus and Navigation support.

In this (Common) library those will be found under the `Edam.UI.Common.Controls.Navigation`
namespace. The `MenuController` manage expected common Menus functionality that
include instantiating panels or controls and navigating through them.

The `NavigationControl.xaml` user control implements a general layout for the
instanced pages and controls to be visible within a named UI Frame called `PanelControl`.
The Main Menu doesn't need to be visible but managed and the `MenuController` just do that.
Therefore, setup new menu items by calling the `MenuController.SetupInstance` method.
Take a look at the `NavigationControl` constructure:

```
public NavigationControl()
{
    this.InitializeComponent();
    this.DataContext = m_ViewModel;

    VisibleMenuItems.Source = m_ViewModel.VisibleItems;
    ApplicationHelper.SetApplicationMenuControl(this);

    m_MenuController = new MenuController(PanelContent, ViewModel.Items);

    // add/update a menu option
    ProjectViewerControl projectViewer = new ProjectViewerControl();
    m_MenuController.SetupInstance(MenuOption.Projects, projectViewer);

    ApplicationHelper.SetMenuOption(MenuOption.Login);
}
```

Note that upon calling the `NavigationControl.xaml.cs` constructure the instance
of `MenuController` is created.  In the above example the navigation get started
by calling the `ApplicationHelper.SetMenuOption` that will trigger the initiation
of the Login process.

## Identity Management and Services

As shown in the previous section the Login process gets initiated as the last step
of the `NavigationControl` constructure by calling the `ApplicationHelper.SetMenuOption`
method.  The identity and management services are assumed to allow fully disconnected
applications and therefore as a minimum a local Sqlite database is used to store
organization and user information that gets initiated upon the first use of the
application.  Through the process a pin number is asked to be defined so end-users
don't need to provide the credentials repeatedly and just provide the already
registered pin-number.

Login details and registration is managed through the related process within the
`LoginViewModel` code while using the `AccountLoginControl` and `AccountPinLoginControl`
User Control to gather the required information and execution of necessary functionality
to register the data into the target repository (default: Sqlite DB).

## Data Templates
Trying to find a way to quickly generate even simple forms to gather some basic
information without writing code and get bothered by some technical UI issues
the "Data Templates" got born.

Just describe the data entity(es) that are needed and translate the description
into a form.  For example, the following JSON document describe a form to get
details about the domain of an entity:

```
[
   {
      "Title": "Asset Id",
      "Name": "DomainId",
      "MinLength": "0",
      "MaxLength": "20",
      "Required": "0",
      "SerialNo": "1"
   },
   {
      "Title": "Type",
      "Name": "TypeNo",
      "MinLength": "0",
      "MaxLength": "20",
      "ValueType": "6",
      "SerialNo": "2",
      "LinkCodes": [
         {
            "CodeId": "1",
            "Description": "Asset"
         },
         {
            "CodeId": "2",
            "Description": "Instance"
         },
         {
            "CodeId": "10",
            "Description": "Information Exchange"
         },
         {
            "CodeId": "20",
            "Description": "Form"
         },
         {
            "CodeId": "30",
            "Description": "Use Case"
         },
         {
            "CodeId": "40",
            "Description": "Code Set"
         },
         {
            "CodeId": "30",
            "Description": "Use Case"
         },
         {
            "CodeId": "50",
            "Description": "Document"
         },
         {
            "CodeId": "60",
            "Description": "Report"
         },
         {
            "CodeId": "70",
            "Description": "Table"
         },
         {
            "CodeId": "80",
            "Description": "Schema"
         }
      ]
   },
   {
      "Title": "Name",
      "Name": "DomainName",
      "MinLength": "0",
      "MaxLength": "80",
      "SerialNo": "3"
   },
   {
      "Title": "URI Prefix",
      "Name": "Prefix",
      "Description": "Short identifier for the URI.  Try to make it unique for each URI.",
      "MinLength": "0",
      "MaxLength": "20",
      "SerialNo": "4"
   },
   {
      "Title": "URI",
      "Name": "DomainUri",
      "Description": "Unique Resource ID formatted as (http://<Organization URI>.<Business Entity ID>.<Resource ID>) for example http://datovy.com/encounter/schedule",
      "MinLength": "0",
      "MaxLength": "200",
      "SerialNo": "5"
   },
   {
      "Title": "Description",
      "Name": "Description",
      "Description": "Provide details about the domain purpose and possible use",
      "MinLength": "0",
      "MaxLength": "1024",
      "SerialNo": "8",
      "Discernible": "1"
   }

]
```

Note that while describing the data entity you are not concerned and shouldn't be
concerned about how it will look to the end-user and concentrate on the task of
having a well describe data entity element and associated meta data.

The previous will create a form that has the "Titles" and their values being
displayed one after the other in the following order:

```
Asset ID    [           ]
Type        [           ] (will be displayed as a DropDown list)
Name        [           ]
URI Prefix  [           ]
URI         [           ]
Description [           ]
```

Also understand that you are describing each data element in detail and therefore
the provided information can be used to manage the data entry such as min and max
string length, available codes to pick from a drop-down list, and tooltips
to show when the user hover in any of the text editor boxes.

Each of the described elements are defaulted to a base "element type" that since
it was not specified will be defaulted to "10" (Column).  To specify a different
element type, add the "Type" attribute with the required value, available options
follow:

```
 Unknown         = 0
[Column          = 10]
 Parameter       = 11
 Variable        = 12
 Constant        = 13
 OutputValue     = 14
 ReturnValue     = 15
 Group           = 91
 Link            = 51
 Map             = 52
 Section         = 16
 View            = 20
 Function        = 21
 Dynamic         = 100
 StoredProcedure = 30
```

### Data Template Interpreters

Given a Data Template an interpreter will go through the JSON document and derive
a Form out of the description, here is where the metadata is used to generate
each of the requested UI elements apply visuals and manage its instancing,
dialog lifecycle.  The related service will open the UI dialog, allow the user
to enter the information, submit or cancel/close the dialog session.  Based on
the gathered information a C# dynamic object is returned for the developer to
get the info of each element attribute and do something with it.

For more details go to the `Edam.Data.ReferenceData` (dll) library that supports
the implementation and interpretation of the supported entity-elements.  Within
`Edam.UI.Common` `Controls.Helpers` folder the default Data Template Interpreter
is available.

To open the form you could use the `Edam.UI.Common` - `Controls.Dialogs.DialogBox`
- `ShowDialog`

```
  /// <summary>
  /// Using information in a Reference Data Template prepare a dialog box
  /// and show it.
  /// </summary>
  /// <param name="dataTemplateFilePath">data template file path</param>
  /// <param name="dataEntityName">data entity name</param>
  /// <param name="dataEntityDescription">data entity description</param>
  /// <param name="callBack">(optional) call back function</param>
  /// <param name="PrimaryButtonText">primary button text</param>
  public static void ShowDialog(string dataTemplateFilePath, 
     string dataEntityName, string dataEntityDescription,
     Action<IDialogObjectInfo> callBack, string PrimaryButtonText)
  {
     ElementNodeInfo node = ApplicationElementInfo.GetElementNode(
        dataTemplateFilePath, dataEntityName, dataEntityDescription);

     Dialogs.DialogInfo d = new Dialogs.DialogInfo
     {
        Title = dataEntityDescription,
        ItemName = node.Name,
        ItemType = Dialogs.DialogInfo.ITEM_TYPE_FILE,
        CallBack = callBack,
        DataObject = node,
        PrimaryText = PrimaryButtonText,
        ModelData = node.GetModelData()
     };

     DialogBox.ShowDialog(d);
  }
```

You can follow the code to figure out how it is implemented by the default
interpreter.
