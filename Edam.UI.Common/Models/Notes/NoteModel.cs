using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.DataObjects.Notes;
using Edam.DataObjects.DataCodes;
using Edam.DataObjects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Edam.UI.Common.Models.Notes;


public class NoteModel : ObservableObject
{

    private NoteInfo m_Note = new NoteInfo();

    public String NoteText
    {
        get { return m_Note.NoteText; }
        set
        {
            if (m_Note.NoteText != value)
            {
                m_Note.NoteText = value;
                OnPropertyChanged(nameof(NoteText));
            }
        }
    }

    public NoteInfo Note
    {
        get { return m_Note; }
        set
        {
            if (m_Note != value)
            {
                m_Note = value;
                OnPropertyChanged(nameof(Note));
            }
        }
    }
    public String OrganizationId
    {
        get { return m_Note.OrganizationId; }
        set
        {
            if (m_Note.OrganizationId != value)
            {
                m_Note.OrganizationId = value;
                OnPropertyChanged(nameof(OrganizationId));
            }
        }
    }

    public Int16 TypeNo
    {
        get { return m_Note.TypeNo; }
        set
        {
            if (m_Note.TypeNo != value)
            {
                m_Note.TypeNo = value;
                OnPropertyChanged(nameof(TypeNo));
            }
        }
    }

    public DataCodeInfo TypeCode
    {
        get { return m_Note.TypeCode; }
        set
        {
            if (m_Note.TypeCode != value)
            {
                m_Note.TypeCode = value;
                OnPropertyChanged("TypeCode");
            }
        }
    }

    private String m_TypeDescription;
    public String TypeDescription
    {
        get { return m_TypeDescription; }
        set
        {
            if (m_TypeDescription != value)
            {
                m_TypeDescription = value;
                OnPropertyChanged(nameof(TypeDescription));
            }
        }
    }

    public String ReferenceId
    {
        get { return m_Note.ReferenceId; }
        set
        {
            if (m_Note.ReferenceId != value)
            {
                m_Note.ReferenceId = value;
                OnPropertyChanged(nameof(ReferenceId));
            }
        }
    }

    public DateTime? ReferenceDate
    {
        get { return m_Note.ReferenceDate; }
        set
        {
            if (m_Note.ReferenceDate != value)
            {
                m_Note.ReferenceDate = value;
                OnPropertyChanged(nameof(ReferenceDate));
            }
        }
    }

    public DateTimeOffset ReferenceDateOffset
    {
        get
        {
            return m_Note.ReferenceDate.HasValue ? m_Note.ReferenceDate.Value :
               new DateTime();
        }
        set
        {
            if (m_Note.ReferenceDate != value.Date)
            {
                m_Note.ReferenceDate = value.Date;
                OnPropertyChanged("ReferenceDateOffset");
            }
        }
    }

    private String m_ReferenceDateText;
    public String ReferenceDateText
    {
        get
        {
            m_ReferenceDateText = m_Note.ReferenceDate.HasValue ?
               m_Note.ReferenceDate.Value.ToString("g") : String.Empty;
            if (m_Note.ReferenceDate.HasValue)
            {
                Edam.DateDiff d =
                   new DateDiff(DateTime.Now, m_Note.ReferenceDate.Value);
                m_ReferenceDateText += " ( " + d.HowOldText() + " )";
            }
            return m_ReferenceDateText;
        }
        set
        {
            if (m_ReferenceDateText != value)
            {
                m_ReferenceDateText = value;
                OnPropertyChanged("ReferenceDateText");
            }
        }
    }

    public String NoteId
    {
        get { return m_Note.NoteId; }
        set
        {
            if (m_Note.NoteId != value)
            {
                m_Note.NoteId = value;
                OnPropertyChanged(nameof(NoteId));
            }
        }
    }

    public Boolean IsValid
    {
        get { return m_Note.Validate(); }
    }

    public NoteModel(NoteInfo note = null)
    {
        m_Note = note ?? new NoteInfo();
    }

    public void ClearFields()
    {
        m_Note.ClearFields();
    }

}
