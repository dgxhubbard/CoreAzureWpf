using CoreBase.Enums;

namespace CoreBase.Interfaces
{
    public interface ILoggerConfig
    {
        #region Properties

        LogArchiveNumbering LogArchiveNumbering 
        { get; set; }

        LogVerbosity LogVerbosity 
        { get; set; }

        int MaxArchiveFiles 
        { get; set; }

        int MaxLogFileSize 
        { get; set; }

        string LogFile
        { get; }

        string LogPath
        { get; }

        string ConfigFile
        { get; }


        string ConfigPath
        { get; }

        #endregion

        #region Methods

        bool LoadConfig ();
        void SaveConfig ();

        #endregion
    }
}