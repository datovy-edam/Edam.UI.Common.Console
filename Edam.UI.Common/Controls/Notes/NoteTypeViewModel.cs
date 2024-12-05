using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.DataObjects.DataCodes;
using resource = Edam.Application.ApplicationHelper;
using Edam.Application;
using Edam.DataObjects.References;
using Edam.DataObjects;
using Edam.DataObjects.Notes;

namespace Edam.UI.Common.Models.Notes;


public class NoteTypeViewModel : ObservableObject
{

    #region -- 1.00 - Properties and definitions...

    protected ObservableCollection<DataCodeInfo> m_NoteTypes =
       new ObservableCollection<DataCodeInfo>();

    public ObservableCollection<DataCodeInfo> NoteTypes
    {
        get { return m_NoteTypes; }
        set
        {
            if (m_NoteTypes != value)
            {
                m_NoteTypes = value;
                OnPropertyChanged("NoteTypes");
            }
        }
    }

    private Int32 m_SelectedNoteTypeIndex;
    public Int32 SelectedNoteTypeIndex
    {
        get { return m_SelectedNoteTypeIndex; }
        set
        {
            if (m_SelectedNoteTypeIndex != value)
            {
                m_SelectedNoteTypeIndex = value;
                OnPropertyChanged(DataElementName.SelectedNoteTypeIndex);

                //if (SelectedItem != null)
                //{
                //   var i = NoteTypes[value];
                //   SelectedItem.TypeCode = i;
                //   SelectedItem.TypeDescription = i.Description;
                //}
            }
        }
    }

    #endregion
    #region -- 1.50 - Initialize Resources

    public NoteTypeViewModel()
    {
        var l = CacheHelper.GetNoteTypes((Int16)ReferenceType.Person);
        foreach (var i in l)
        {
            m_NoteTypes.Add(i);
        }
    }

    #endregion
    #region -- 4.00 - Manage Note Type

    public DataCodeInfo FindNoteType(String codeId)
    {
        DataCodeInfo c = null;
        foreach (var i in m_NoteTypes)
        {
            if (i.CodeId == codeId)
            {
                c = i;
                break;
            }
        }
        return c;
    }

    public Int32 FindNoteType(Int16 typeNo)
    {
        Int32 index = -1;
        if (NoteTypes == null)
            return index;

        String typeText = typeNo.ToString();
        for (var i = 0; i < NoteTypes.Count; i++)
        {
            if (NoteTypes[i].CodeId == typeText)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public void SetNoteTypeByNo(Int16 typeNo)
    {
        var i = FindNoteType(typeNo);
        if (i < 0)
            return;
        SelectedNoteTypeIndex = i;
    }

    public void SetNoteType(NoteModel note, NoteModel selectedItem)
    {
        if (note.TypeNo == (Int16)NoteType.Unknown)
            note.TypeNo = (Int16)NoteType.FollowUpNote;
        note.TypeCode = FindNoteType(note.TypeNo.ToString());
        note.TypeDescription = note.TypeCode == null ?
           resource.GetLocalString("Unknown") : note.TypeCode.Description;
        if (selectedItem != null && note == selectedItem)
            SetNoteTypeByNo(note.TypeNo);
    }

    #endregion

}
