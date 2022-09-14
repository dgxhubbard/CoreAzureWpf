using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using CommonServiceLocator;


using CoreBase;
using CoreBase.Constants;
using CoreBase.Enums;
using CoreBase.Interfaces;
using CoreBase.Utilities;

namespace CoreBase
{
    public abstract class BootstrapperBase
    {

        #region 


        public BootstrapperBase ()
        {

            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve ( object sender, ResolveEventArgs args )
        {
            Assembly assembly = null;

            var name = args.Name;

            var pos = name.IndexOf ( "," );
            if ( pos != -1 )
            {
                name = name.Substring ( 0, pos );
            }

            name += ".dll";

            var fullExeNameAndPath = Assembly.GetEntryAssembly ().Location;
            var path = Path.GetDirectoryName ( fullExeNameAndPath );

            var filepath = Path.Combine ( path, name );

            try
            {
                assembly = Assembly.LoadFrom ( filepath );
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( "Failed to resolve " + filepath, ex );
                LogProvider.Logger.LogException ( "Failed to resolve " + filepath, ex );
            }

            return assembly;
        }





        #endregion 

        #region Properties


        CompositionContainer _container;
        public CompositionContainer Container
        {
            get
            {
                CompositionContainer container = null;
                lock ( SyncLock )
                {
                    container = _container;
                }

                return container;
            }
            set
            {
                lock ( SyncLock )
                {
                    _container = value;
                }
            }
        }

        AggregateCatalog _catalog;
        public AggregateCatalog Catalog
        {
            get
            {
                AggregateCatalog catalog = null;
                lock ( SyncLock )
                {
                    catalog = _catalog;
                }

                return catalog;
            }
            set
            {
                lock ( SyncLock )
                {
                    _catalog = value;
                }
            }
        }


        object _syncLock = new object ();
        private object SyncLock
        {
            get { return _syncLock; }
        }

        #endregion

        #region New Methods

        public virtual void Run ()
        {

            Initialize ();

            // Because we created the LogProvider.Logger and it needs to be used immediately, we compose it to satisfy any imports it has.
            Container.ComposeExportedValue<ICompositionService> ( Container );
            Container.ComposeExportedValue<CompositionContainer> ( Container );


        }


        #endregion

        #region Virtual Methods

        public abstract void AddCatalogs ( ICollection<ComposablePartCatalog> catalogs );


        #endregion


        #region Support Methods


        public void Initialize ()
        {

            // intialize mef and service locator
            var aggregateCatalog = new AggregateCatalog ();
            var container = new CompositionContainer ( aggregateCatalog, CompositionOptions.DisableSilentRejection );

            var catalogs = aggregateCatalog.Catalogs;

            Container = container;
            Catalog = aggregateCatalog;

            AddCatalogs ( catalogs );


            ServiceLocator.SetLocatorProvider ( () => new MefServiceLocatorAdapter ( container ) );

            ShowExports ();


        }


        public static bool LoadCatalog ( ICollection<ComposablePartCatalog> catalogs, string assemblyName )
        {
            bool success = true;

            if ( catalogs == null )
            {
                var msg = "Bootstrapper LoadCatalog catalogs required " + assemblyName + Environment.NewLine;
                if ( LogProvider.Logger != null )
                    LogProvider.Logger.LogError ( msg );
                else
                    File.AppendAllText ( "gagetrak_error.log", msg );


                throw new ArgumentNullException ( "catalogs is not valid" );
            }

            if ( string.IsNullOrEmpty ( assemblyName ) )
            {
                var msg = "Bootstrapper LoadCatalog assemblyName required " + assemblyName + Environment.NewLine;

                if ( LogProvider.Logger != null )
                    LogProvider.Logger.LogError ( msg );
                else
                    File.AppendAllText ( "gagetrak_error.log", msg );


                throw new ArgumentNullException ( "assemblyName is not valid" );
            }

            var assemblyCatalog = GetCatalog ( assemblyName );
            if ( assemblyCatalog == null )
            {
                var msg = "Bootstrapper LoadCatalog assembly catalog required " + assemblyName  + Environment.NewLine;

                if ( LogProvider.Logger != null )
                    LogProvider.Logger.LogError ( msg );
                else
                    File.AppendAllText ( "gagetrak_error.log", msg );

                throw new NullReferenceException ( "Failed to get assembly catalog " + assemblyName );
            }



            try
            {
                var parts = assemblyCatalog.Parts;

                if ( parts != null )
                {
                    var definitions = new List<ExportDefinition> ();
                    if ( definitions == null )
                        throw new NullReferenceException ( "FAILED to create export list" );

                    foreach ( var part in parts )
                        definitions.AddRange ( part.ExportDefinitions );

                    definitions.Sort ( ( a, b ) => string.Compare ( a.ContractName, b.ContractName ) );
                }
                else
                {
                    if ( LogProvider.Logger != null )
                        LogProvider.Logger.LogInfo ( "No exports for " + assemblyName );
                    else
                        File.AppendAllText ( "gagetrak_error.log", "No exports for " + assemblyName );
                }

                catalogs.Add ( assemblyCatalog );
            }
            catch ( Exception ex )
            {
                if ( LogProvider.Logger != null )
                    LogProvider.Logger.LogException ( "Failed to load assembly catalog " + assemblyName, ex );
                else
                {
                    File.AppendAllText ( "gagetrak_error.log", "Failed to load assembly catalog " + assemblyName );
                    File.AppendAllText ( "gagetrak_error.log", ex.Message + Environment.NewLine + ex.StackTrace.ToString () + Environment.NewLine );
                }

                throw new InvalidOperationException ( "Failed to load assembly catalog " + assemblyName );
            }


            return success;
        }

        public static AssemblyCatalog GetCatalog ( string assemblyName )
        {
            AssemblyCatalog assemblyCatalog = null;

            var assembly = System.Reflection.Assembly.GetExecutingAssembly ();
            var codebase = assembly.GetName ().CodeBase;

            var path = codebase.Replace ( @"file:///", string.Empty );
            var basePath = System.IO.Path.GetDirectoryName ( path );
            var assemblyPath = Path.Combine ( basePath, assemblyName );

            try
            {
                assemblyCatalog = new AssemblyCatalog ( assemblyPath );
            }
            catch ( Exception ex )
            {
                LogProvider.Logger.LogException ( "Failed to load assembly catalog " + assemblyPath, ex );
            }

            return assemblyCatalog;
        }



        public void ShowExports ()
        {
            try
            {
                var parts = Catalog.Parts;

                var definitions = new List<ExportDefinition> ();
                foreach ( var part in parts )
                {
                    definitions.AddRange ( part.ExportDefinitions );
                }

                definitions.Sort ( ( a, b ) => string.Compare ( a.ContractName, b.ContractName ) );


                var names = new List<string> ();
                foreach ( var expDefinition in definitions )
                {
                    string name = expDefinition.ContractName;
                    if ( string.IsNullOrEmpty ( name ) )
                        continue;

                    names.Add ( name );
                }

                names.ForEach ( name => Debug.WriteLine ( "Name " + name ) );

            }
            catch ( Exception ex )
            {
                LogProvider.Logger.LogException ( "Failed to ShowExports", ex );
                throw;
            }
        }

        #endregion


    }
}
