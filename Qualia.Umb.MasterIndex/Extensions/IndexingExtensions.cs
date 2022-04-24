using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examine;
using Examine.Search;

namespace Qualia.Umb.MasterIndex
{
    public static class IndexingExtensions
    {
        public static IBooleanOperation IsDocumentType(this IQuery q, string docTypeAlias)
        {
            var newQ = q.Field(Umbraco.Cms.Infrastructure.Examine.UmbracoExamineFieldNames.ItemTypeFieldName, docTypeAlias);
            return newQ;
        }

        public static IBooleanOperation IsDocumentType(this IQuery q, IEnumerable<string> docTypeAlias)
        {
            var newQ = q.GroupedOr(new string[] { Umbraco.Cms.Infrastructure.Examine.UmbracoExamineFieldNames.ItemTypeFieldName }, docTypeAlias.ToArray());
            return newQ;
        }

        public static IBooleanOperation HasAncestor(this IQuery q, int ancestorId)
        {
            var newQ = q.Field(SearchablePathComponent.SEARCHABLE_PATH_INDEX_FIELD, ancestorId.ToString("d"));
            return newQ;
        }

        public static IBooleanOperation IsPublishedInLanguage(this IQuery q, string cultureIso)
        {
            var newQ = q.Field($"{Umbraco.Cms.Infrastructure.Examine.UmbracoExamineFieldNames.PublishedFieldName}_{cultureIso.ToLower()}", "y");
            return newQ;
        }

        public static IBooleanOperation ExcludeNodesByIds(this IQuery q, IEnumerable<int> excludedIds)
        {
            var newQ = q.GroupedNot(new string[] { Umbraco.Cms.Infrastructure.Examine.UmbracoExamineFieldNames.ItemIdFieldName }, excludedIds?.Select(x => x.ToString()).ToArray());
            return newQ;
        }


    }
}
