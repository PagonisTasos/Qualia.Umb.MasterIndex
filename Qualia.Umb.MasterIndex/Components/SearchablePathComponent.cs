using Examine;
using System.Linq;
using Umbraco.Cms.Infrastructure.Examine;

namespace Qualia.Umb.MasterIndex
{
    public class SearchablePathComponent : AbstractMasterIndexComponent
    {
        public const string SEARCHABLE_PATH_INDEX_FIELD = "searchablePath";

        public SearchablePathComponent(IExamineManager examineManager) : base(examineManager)
        { }

        public override FieldDefinition[] FieldDefinitions => new FieldDefinition[1]
        {
            new FieldDefinition(SEARCHABLE_PATH_INDEX_FIELD, FieldDefinitionTypes.FullText)
        };

        public override void TransformingFieldValues(object? sender, IndexingItemEventArgs e)
        {
            if (e?.ValueSet?.Category == IndexTypes.Content)
            {
                //get the path from umbraco's "path" field
                var path = e.ValueSet.Values.ToList().FirstOrDefault(v => v.Key == UmbracoExamineFieldNames.IndexPathFieldName).Value?.FirstOrDefault() as string;
                //store the path with spaces to make it searchable with index analyzers
                e.ValueSet.Set(SEARCHABLE_PATH_INDEX_FIELD, path?.Replace(",", " "));
            }
        }
    }
}
