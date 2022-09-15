

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

using CommonServiceLocator;

namespace CoreBase
{
    public class MefServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly CompositionContainer compositionContainer;

        public MefServiceLocatorAdapter(CompositionContainer compositionContainer)
        {
            this.compositionContainer = compositionContainer;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            List<object> instances = new List<object>();

            IEnumerable<Lazy<object, object>> exports = this.compositionContainer.GetExports(serviceType, null, null);
            if (exports != null)
            {
                instances.AddRange(exports.Select(export => export.Value));
            }

            return instances;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            IEnumerable<Lazy<object, object>> exports = this.compositionContainer.GetExports(serviceType, null, key);
            if ( exports != null  && exports.Count () > 0 )
            {
                return exports.Single().Value;
            }
                


            throw new ActivationException( this.FormatActivationExceptionMessage( new CompositionException("Export not found"), serviceType, key ) );
        }
    }
}