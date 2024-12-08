using System;

using Microsoft.Extensions.DependencyInjection;

// -----------------------------------------------------------------------------
using Edam.Application;
using helper = Edam.UI.Helpers;
using Edam.InOut;
using languages = Edam.Language;

namespace Edam.UI.Helpers;


public class DependencyInjectionHelper
{

    public static void InitializeDependencyInjectionService()
    {
        /*
        DependencyService.Collection.Add(
           new ServiceDescriptor(
              typeof(IFileHelper), (IFileHelper)(new helper.FileHelper())));

        DependencyService.Collection.Add(
           new ServiceDescriptor(
              typeof(IReferenceDataTemplateReader),
                 (IReferenceDataTemplateReader)
                    (new ReferenceDataTemplateFileReader())));
        DependencyService.Collection.Add(
           new ServiceDescriptor(
              typeof(IApplicationResource),
                 (IApplicationResource)
                    (new ApplicationResource())));
        DependencyService.Compile();

        AppAssembly.RegisterType(
           AssetResourceHelper.ASSET_B2B_EDI_FILE_READER,
           typeof(EdiFileReader),
           "EdiToAssets");
        AppAssembly.RegisterType(
           AssetResourceHelper.ASSET_APP_SETTINGS,
           typeof(UIApp.AppSettings),
           AppSettings.APP_SETTINGS_SECTION_KEY);
        AppAssembly.RegisterType(
           AssetResourceHelper.ASSET_DDL_IMPORT_FILE_READER,
           typeof(ImportReader),
           AssetConsoleProcedure.DdlImportToAssets.ToString());

        AppAssembly.RegisterType(AssetResourceHelper.ASSET_MAPPING_LANGUAGE,
           typeof(MapLanguageInfo),
           AssetResourceHelper.ASSET_MAPPING_LANGUAGE);
        AppAssembly.RegisterType(
           AssetResourceHelper.ASSET_ROW_BUILDER_NAME,
           typeof(Edam.Xml.OpenXml.ExcelRowBuilder));

        // TODO: Implement it using the interface !!!
        AppAssembly.RegisterType(
           AssetResourceHelper.ASSET_LEXICON,
           typeof(Edam.Data.Lexicon.LexiconData));
        AppAssembly.RegisterType(
           languages.LanguageHelper.LANGUAGE_PHYTHON,
           typeof(Edam.Language.Python.Interpreter));
        AppAssembly.RegisterType(
           TextSimilarityService.SEMANTIC_TEXT_SIMILARITY_INSTANCE,
           typeof(TextSimilarityInstance));

        AppAssembly.RegisterType(
           AssetResourceHelper.ASSET_DICTIONARY_API,
           typeof(Edam.Data.Dictionary.Api.FreeDictionaryApi));
        */

    }

}

