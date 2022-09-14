using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using CoreBase.Enums;
using CoreBase.Interfaces;
using CoreBase.Utilities;


namespace CoreBase
{

    [Export ( typeof ( ILoggerConfig ) )]
    [PartCreationPolicy ( CreationPolicy.NonShared )]
    public class LoggerConfig : ILoggerConfig
    {
        #region Constants

        public const string LoggerFile = "gagetrak.log";

        public const string LoggerConfigFile = "LoggerConfig.xml";

        public const string LoggerConfigLabel = "LoggerConfig";

        public const string MaxLogFileSizeLabel = "MaxLogFileSize";
        public const string MaxArchiveFilesLabel = "MaxArchiveFiles";
        public const string ArchiveNumberingLabel = "ArchiveNumbering";
        public const string LogVerbosityLabel = "LogVerbosity";
        public const string ValueLabel = "value";

        public const string InfoLabel = "Info";
        public const string WarningLabel = "Warning";
        public const string ErrorLabel = "Error";

        public const int DefaultMaxLogFileSize = 5000000;
        public const int DefaultMaxArchiveFiles = 3;
        public const LogArchiveNumbering DefaultArchiveNumbering = LogArchiveNumbering.Rolling;
        public const LogVerbosity DefaultLogVerbosity = LogVerbosity.Error;


        #endregion



        #region Constructors

        public LoggerConfig ()
        {
            MaxLogFileSize = DefaultMaxLogFileSize;
            MaxArchiveFiles = DefaultMaxArchiveFiles;
            LogArchiveNumbering = DefaultArchiveNumbering;
            LogVerbosity = DefaultLogVerbosity;


            LoadConfig ();
        }


        #endregion


        #region Properties

        public int MaxLogFileSize
        { get; set; }

        public int MaxArchiveFiles
        { get; set; }


        public LogArchiveNumbering LogArchiveNumbering
        { get; set; }


        public LogVerbosity LogVerbosity
        { get; set; }

        public virtual string LogFile
        {
            get { return LoggerFile; }
        }

        public virtual string LogPath
        {
            get { return Path.Combine ( PathUtilities.BinPath, LoggerFile ); }
        }

        public virtual string ConfigFile
        {
            get { return LoggerConfigFile; }
        }


        public virtual string ConfigPath
        {
            get 
            { 
                var path = Path.Combine ( PathUtilities.BinPath, LoggerConfigFile );

                return path;
            }
        }


        #endregion

        #region Support Methods


        public bool LoadConfig ()
        {
            var success = true;

            var configPath = ConfigPath;

            if ( string.IsNullOrWhiteSpace ( configPath ) )
                throw new ArgumentNullException ( "ValidConfigPathRequired" );

            // write config file if it does not exist
            if ( !File.Exists ( configPath ) )
            {
                SaveConfig ();
            }

            success = LoadConfigInternal ();


            return success;
        }


        public bool LoadConfigInternal ()
        {
            var success = false;
            var configPath = ConfigPath;

            if ( string.IsNullOrWhiteSpace ( configPath ) )
                throw new ArgumentNullException ( "ValidConfigPathRequired" );

            if ( !File.Exists ( configPath ) )
                throw new ApplicationException ( "ValidConfigFileRequired" );

            try
            {
                var xmlDoc = XDocument.Load ( configPath );

                var descendants = xmlDoc.Descendants ( LoggerConfigLabel );
                var descendant = descendants.ElementAt ( 0 );

                var tmpStr = descendant.Element ( MaxLogFileSizeLabel ).Attribute ( ValueLabel ).Value;

                MaxLogFileSize = DefaultMaxLogFileSize;
                if ( !string.IsNullOrWhiteSpace ( tmpStr ) )
                {
                    int size;
                    if ( int.TryParse ( tmpStr, out size ) )
                    {
                        MaxLogFileSize = size;
                    }
                }

                tmpStr = descendant.Element ( MaxArchiveFilesLabel ).Attribute ( ValueLabel ).Value;

                MaxArchiveFiles = DefaultMaxArchiveFiles;
                if ( !string.IsNullOrWhiteSpace ( tmpStr ) )
                {
                    int size;
                    if ( int.TryParse ( tmpStr, out size ) )
                    {
                        MaxArchiveFiles = size;
                    }
                }

                tmpStr = descendant.Element ( ArchiveNumberingLabel ).Attribute ( ValueLabel ).Value;

                LogArchiveNumbering = DefaultArchiveNumbering;
                if ( !string.IsNullOrWhiteSpace ( tmpStr ) )
                {
                    if ( tmpStr == "Rolling" )
                        LogArchiveNumbering = LogArchiveNumbering.Rolling;
                    else if ( tmpStr == "Date" )
                        LogArchiveNumbering = LogArchiveNumbering.Date;
                }

                tmpStr = descendant.Element ( LogVerbosityLabel ).Attribute ( ValueLabel ).Value;

                LogVerbosity = DefaultLogVerbosity;
                if ( !string.IsNullOrWhiteSpace ( tmpStr ) )
                {
                    if ( tmpStr == InfoLabel )
                        LogVerbosity = LogVerbosity.Info;
                    else if ( tmpStr == WarningLabel )
                        LogVerbosity = LogVerbosity.Warning;
                    else if ( tmpStr == ErrorLabel )
                        LogVerbosity = LogVerbosity.Error;

                }


                success = true;
            }
            catch ( Exception ex )
            {
                LogProvider.Logger.LogException ( "Error reading " + configPath, ex );
            }

            return success;
        }


        public void SaveConfig ()
        {
            string filename = ConfigPath;
            if ( string.IsNullOrEmpty ( filename ) )
                throw new ArgumentNullException ( "filename required" );

            var settings = new XmlWriterSettings { Indent = true };
            if ( settings == null )
                throw new NullReferenceException ( "Failed to create xml writer settings" );

            using ( var writer = XmlWriter.Create ( filename, settings ) )
            {
                SaveConfig ( writer );
            }
        }


        protected void SaveConfig ( XmlWriter writer )
        {
            if ( writer == null )
                throw new ArgumentNullException ( "writer required" );

            try
            {
                writer.WriteStartElement ( LoggerConfigLabel );

                writer.WriteStartElement ( MaxLogFileSizeLabel );
                writer.WriteAttributeString ( ValueLabel, MaxLogFileSize.ToString () );
                writer.WriteEndElement ();

                writer.WriteStartElement ( MaxArchiveFilesLabel );
                writer.WriteAttributeString ( ValueLabel, MaxArchiveFiles.ToString () );
                writer.WriteEndElement ();

                writer.WriteComment ( "Rolling, Date" );
                writer.WriteStartElement ( ArchiveNumberingLabel );
                writer.WriteAttributeString ( ValueLabel, ArchiveNumberingLabel.ToString () );
                writer.WriteEndElement ();

                writer.WriteComment ( "Info, Warning, Error" );
                writer.WriteStartElement ( LogVerbosityLabel );
                writer.WriteAttributeString ( ValueLabel, LogVerbosity.ToString () );
                writer.WriteEndElement ();

                writer.WriteEndElement ();

            }
            catch ( Exception ex )
            {
                LogProvider.Logger.LogException ( "Failed to save logger config", ex );

                throw;
            }


            #endregion
        }
    }
}  