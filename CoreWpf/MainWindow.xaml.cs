using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


using Microsoft.EntityFrameworkCore;

using CoreModel;

namespace CoreWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow ()
        {
            InitializeComponent ();

            DataContext = this;
        }

        #region Properties


        string _databaseName;
        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }
            set
            {
                _databaseName = value;
                NotifyPropertyChanged ( "DatabaseName" );
            }
        }




        string _serverName;
        public string ServerName
        {
            get
            {
                return _serverName;
            }
            set
            {
                _serverName = value;
                NotifyPropertyChanged ( "ServerName" );
            }
        }


        string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                NotifyPropertyChanged ( "UserName" );
            }
        }


        string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                NotifyPropertyChanged ( "Password" );
            }
        }



        #endregion

        #region Handlers


        private void Button_Click ( object sender, RoutedEventArgs e )
        {

            var bldr = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder ();

            bldr.DataSource = ServerName;
            bldr.InitialCatalog = DatabaseName;


            if ( !ServerName.EndsWith ( "database.windows.net" ) )
                bldr.IntegratedSecurity = true;

            if ( !string.IsNullOrEmpty( UserName ) && !string.IsNullOrEmpty( Password ) )
            {
                bldr.UserID = UserName;
                bldr.Password = Password;
            }


            var connectionString = bldr.ToString ();

            connectionString += ";TrustServerCertificate=True;";


            var context = MsSqlContextFactory.Create ( connectionString );
            if ( context == null )
                throw new NullReferenceException ( "FAILED to create mssql context" );

            // expand timeout for azure db create
            context.Database.SetCommandTimeout ( 600 );


            try
            {
                context.Database.Migrate ();
            }
            catch ( Exception ex )
            {
                var msg = ex.Message;
            }











        }


        #endregion


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;



        public void NotifyPropertyChanged ( string propertyName )
        {

            if ( string.IsNullOrWhiteSpace ( propertyName ) )
            {
                return;
            }

            Action notifyAction =
            ( Action )
            (
                () =>
                {
                    try
                    {

                        PropertyChangedEventHandler handler = this.PropertyChanged;
                        if ( handler != null )
                        {
                            var e = new PropertyChangedEventArgs ( propertyName );
                            if ( e == null )
                                throw new NullReferenceException ( "Unable to  create PropertyChangedEventArgs for " + propertyName );

                            handler ( this, e );
                        }
                    }
                    catch ( Exception ex )
                    {
                    }
                }
            );

            if ( System.Windows.Application.Current != null && System.Windows.Application.Current.Dispatcher.Thread != Thread.CurrentThread )
                System.Windows.Application.Current.Dispatcher.Invoke ( notifyAction, System.Windows.Threading.DispatcherPriority.DataBind );
            else
                notifyAction ();
        }


        public void OnPropertyChanged ( string info )
        {
            NotifyPropertyChanged ( info );
        }





        #endregion

    }
}
