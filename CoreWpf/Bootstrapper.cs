using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.Threading.Tasks;

using CommonServiceLocator;

using CoreBase;



namespace CoreWpf
{
    public class Bootstrapper : BootstrapperBase
    {
        #region Constructors

        public Bootstrapper () :
            base ()
        { }

        #endregion

        #region Support Methods


        #endregion




        #region Overrides


        public override void AddCatalogs ( ICollection<ComposablePartCatalog> catalogs )
        {
            if ( catalogs == null )
            {
                throw new ArgumentNullException ( "catalogs is not valid" );
            }


            LoadCatalog ( catalogs, "CoreBase.dll" );

            LoadCatalog ( catalogs, "CoreModel.dll" );

            // add our current assembly
            catalogs.Add ( new AssemblyCatalog ( Assembly.GetExecutingAssembly () ) );
        }



        #endregion


    }
}
