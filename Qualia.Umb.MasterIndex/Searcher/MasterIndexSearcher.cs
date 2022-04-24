using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Infrastructure.Examine;

namespace Qualia.Umb.MasterIndex
{
    public abstract class MasterIndexSearcher
    {
        private readonly IExamineManager _examineManager;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private string CULTURE_ISO => _variationContextAccessor.VariationContext.Culture;


        public MasterIndexSearcher(IExamineManager examineManager, IVariationContextAccessor variationContextAccessor)
        {
            this._examineManager = examineManager;
            this._variationContextAccessor = variationContextAccessor;
        }

        /// <summary>
        /// Initialize query only for current language content
        /// </summary>
        /// <returns></returns>
        protected virtual Examine.Search.IQuery InitializeMasterIndexQuery()
        {
            var index = _examineManager.Indexes.FirstOrDefault(f => f.Name == MasterIndexComposer.MASTER_INDEX_NAME)
                ?? throw new ArgumentNullException("index", "Master Index was not found.");
            var searcher = index?.Searcher as Examine.Lucene.Providers.BaseLuceneSearcher
                ?? throw new ArgumentNullException("searcher", "Can't cast master index's searcher.");

            var query = searcher.CreateQuery(IndexTypes.Content
                                                , Examine.Search.BooleanOperation.And
                                                , searcher.LuceneAnalyzer
                                                , new Examine.Lucene.Search.LuceneSearchOptions() { AllowLeadingWildcard = true }
                                                )
                                .IsPublishedInLanguage(CULTURE_ISO)
                                .And();

            return query;
        }
    }
}
