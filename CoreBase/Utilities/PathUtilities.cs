using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CommonServiceLocator;

using CoreBase.Interfaces;

namespace CoreBase.Utilities
{
    public static class PathUtilities
    {
        #region Constants

        public const string AppFiles = "AppFiles";
        public const string ReportError = "ReportError";
        public const string ReportTemp = "ReportTemp";
        public const string ReportScript = "ReportScript";
        public const string Temp = "Temp";
        public const string Bin = "bin";
        public const string ViewSettings = "ViewSettings";
        public const string Reports = "Reports";

        #endregion

        #region Properties

        public static string InstallPath
        {
            get
            {
                return GetInstallPath();
            }
        }

        public static string AppFilesPath
        {
            get
            {
                return GetAppFilesPath();
            }
        }

        public static string BinPath
        {
            get
            {
                return GetBinPath();
            }
        }

        public static string ViewSettingsPath
        {
            get
            {
                return GetViewSettingsPath ();
            }
        }



        public static string TempPath
        {
            get
            {
                return GetPath( InstallPath, Temp );
            }
        }

        static object _classLock = new object();
        static object ClassLock
        {
            get { return _classLock;  }
        }

        #endregion

        #region Methods
        

        private static string GetPath ( string basePath, string extPath )
        {
            string path = Path.Combine( basePath, extPath );

            if ( !Directory.Exists( path ) )
                Directory.CreateDirectory( path );

            return path;
        }

        public static string GetLogPath ()
        {
            return GetInstallPath();
        }

        public static string GetAppFilesPath ()
        {
            string installPath = GetBasePath();
            string path = Path.Combine( installPath, AppFiles );
            return path;
        }



        public static string GetViewSettingsPath ()
        {
            string installPath = GetInstallPath ();

            string filterPath = null;
            if ( installPath.EndsWith ( Bin ) || installPath.EndsWith ( Bin + @"\Debug" ) || installPath.EndsWith ( Bin + @"\Release" ) )
                filterPath = Path.Combine ( installPath, ViewSettings );
            else
                filterPath = Path.Combine ( installPath, ViewSettings );

            return filterPath;
        }


    public static string GetBinPath()
        {
            string installPath = GetInstallPath();

            if ( installPath.Contains( "Bin" ) )
            { 
                installPath.Replace( "Bin", Bin );
            }

            string binPath = null;
            if ( installPath.EndsWith( Bin ) || installPath.EndsWith( Bin + @"\Debug" ) || installPath.EndsWith( Bin + @"\Release" ) )
            {
                binPath = installPath;
            }
            else
            { 
                binPath = Path.Combine(installPath, Bin);
            }

            return binPath;
        }

        public static string GetReportPath ()
        {
            string installPath = GetInstallPath();

            string reportPath = null;
            if ( installPath.EndsWith( Bin ) || installPath.EndsWith( Bin + @"\Debug" ) || installPath.EndsWith( Bin + @"\Release" ) )
                reportPath = installPath;
            else
                reportPath = Path.Combine( installPath, Reports );

            return reportPath;
        }


        static bool showedInstallPath = false;
        public static string GetInstallPath ()
        {
            string basePath = null;

            basePath = Directory.GetCurrentDirectory();

            if ( string.IsNullOrEmpty( basePath ) || !Directory.Exists( basePath ) )
            { 
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                if ( assembly != null )
                {
                    basePath = assembly.Location.Replace( assembly.ManifestModule.Name, "" );
                }
            }

            int pos = -1;
            if ( basePath.EndsWith( "\\" ) )
            {
                pos = basePath.LastIndexOf( "\\" );
                if ( pos > -1 )
                {
                    basePath = basePath.Substring( 0, pos );
                }
            }

            // we are case sensitive on directory so just in case
            // use all lower
            if ( basePath.Contains("Bin") )
                basePath.Replace( "Bin", "bin" );


            return basePath;
        }

        public static string GetBasePath ()
        {
            // get path to database            
            string dir = GetInstallPath();
            string [] endings = { @"bin\Debug", @"bin\Release", @"bin" };

            foreach ( var ending in endings )
            {
                if ( dir.EndsWith( ending ) )
                {
                    dir = dir.Replace( ending, null );
                    break;
                }
            }

            return dir;
        }

        #endregion
    }
}
