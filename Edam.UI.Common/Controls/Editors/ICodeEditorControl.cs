using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.UI.Common.Controls.Editors;

public interface ICodeEditorControl
{
    object Instance { get; }
    void SetText(string text, string extensionName);
    Task<string> GetTextAsync();
    NotificationEvent NotifyCodeEditorEvent { get; set; }
}
