using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using Edam.Data.Lexicon.Semantics;
using Edam.Data.AssetSchema;
using Edam.Data.Books;
using Edam.Data.Lexicon;
using Edam.UI.Common;

namespace Edam.UI.Controls.DataModels;


public delegate void TextSimilarityNotification(
  object source, NotificationArgs args);

/// <summary>
/// Lexicon Data Model.
/// </summary>
public class LexiconDataModel : ObservableObject
{

    public const string TEXT_SIMILARITY = "TextSimilarities";

    private ITextSimilarityInstance m_TextSimilarityInstance = null;

    public DataMapContext DataMapContext { get; set; }
    public AssetDataLexiconContext Context { get; set; }
    public ObservableCollection<ITextSimilarityScore> SimilarityScores
    { get; set; } = new ObservableCollection<ITextSimilarityScore>();

    public TextSimilarityNotification ManageNotification { get; set; }

    public LexiconDataModel()
    {

    }

    /// <summary>
    /// Set the Context based on given data-map context that may be of a
    /// different project or map-item.
    /// </summary>
    /// <param name="context">Mapping context to use</param>
    public void SetContext(DataMapContext context)
    {
        if (context == null)
            return;

        if (Context == null || Context.ContextId != context.ContextId)
        {
            Context = new AssetDataLexiconContext();
        }

        DataMapContext = context as DataMapContext;
        Context.ContextId = context.ContextId;
        Context.UseCase = context.UseCase;
        Context.Instance = context;
        Context.SetLexicon(ProjectContext.Arguments);
    }

    /// <summary>
    /// Compare elements of given context.
    /// </summary>
    /// <param name="booklet"></param>
    public void Compare(BookletInfo booklet)
    {
        SimilarityScores.Clear();

        var instance = m_TextSimilarityInstance ??
           LexiconHelper.GetTextSimilarityInstance();

        foreach (var sitem in DataMapContext.SourceItems)
        {
            foreach (var titem in DataMapContext.TargetItems)
            {
                string sourceText = sitem.GetAnnotation().Description;
                string targetText = titem.GetAnnotation().Description;

                ITextSimilarityScore result = TextSimilarityService.
                   GetSimilarityScore(sourceText, targetText);

                if (result != null)
                {
                    if (result.Results != null && !result.Results.Success)
                    {
                        continue;
                    }
                    SimilarityScores.Add(result);
                }
            }
        }

        // notify about found similarities...
        if (ManageNotification != null)
        {
            NotificationArgs args = new NotificationArgs();
            args.Type = NotificationType.ExecuteItem;
            args.EventData = SimilarityScores;
            args.MessageText = TEXT_SIMILARITY;
            ManageNotification(this, args);
        }
    }

}
