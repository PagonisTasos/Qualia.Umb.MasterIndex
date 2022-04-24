using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;

namespace Qualia.Umb.MasterIndex
{
    public abstract class AbstractMasterIndexComponent : IComponent
    {
        protected readonly IExamineManager _examineManager;

        protected AbstractMasterIndexComponent(IExamineManager examineManager)
        {
            _examineManager = examineManager;
        }

        public virtual void Initialize()
        {
            // get the custom index
            if (!_examineManager.TryGetIndex(MasterIndexComposer.MASTER_INDEX_NAME, out IIndex index))
                return;

            ((BaseIndexProvider)index).TransformingIndexValues += TransformingFieldValues;
        }
        public virtual void Terminate()
        { }

        public abstract FieldDefinition[] FieldDefinitions { get; }
        public abstract void TransformingFieldValues(object? sender, IndexingItemEventArgs e);
    }

}
