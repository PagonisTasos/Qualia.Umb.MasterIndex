using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Examine;
using Umbraco.Cms.Infrastructure.Examine;
using Lucene.Net.Analysis.El;

namespace Qualia.Umb.MasterIndex
{
    public class MasterIndexComposer : IComposer
    {
        public const string MASTER_INDEX_NAME = "Master_Index";

        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services
                .AddExamineLuceneIndex<UmbracoContentIndex, ConfigurationEnabledDirectoryFactory>(
                    name: MASTER_INDEX_NAME,
                    fieldDefinitions: new Extended_Umbraco_FieldDefinition(),
                    analyzer: new GreekAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48),
                    validator: new ContentValueSetValidator(true)
                );

            builder.Components().Append(FieldSetHelper.Instance().MasterIndex_Component_Types);
        }

        private class Extended_Umbraco_FieldDefinition : FieldDefinitionCollection
        {
            public static FieldDefinition[] ExtendedFieldDefinitions =>
                UmbracoFieldDefinitionCollection.UmbracoIndexFieldDefinitions.Concat(FieldSetHelper.Instance().CustomFieldDefinitions).ToArray();

            public Extended_Umbraco_FieldDefinition() : base(ExtendedFieldDefinitions)
            { }
        }


        /// <summary>
        /// Gathers (reflection) all field definitions declared within MasterIndex Components
        /// </summary>
        private class FieldSetHelper
        {
            private FieldSetHelper() { }
            public static FieldSetHelper Instance() => new FieldSetHelper();
            public FieldDefinition[] CustomFieldDefinitions => Components.SelectMany(x => x.FieldDefinitions).ToArray();
            public List<Type> MasterIndex_Component_Types => 
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(Is_MasterIndexComponent)
                .ToList();
            private IEnumerable<AbstractMasterIndexComponent> Components =>
                MasterIndex_Component_Types
                .Select(x => System.Runtime.Serialization.FormatterServices.GetUninitializedObject(x))
                .Cast<AbstractMasterIndexComponent>()
                .ToList();

            private bool Is_MasterIndexComponent(Type type) => typeof(AbstractMasterIndexComponent).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract;
            
        }
    }
}
