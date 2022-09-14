using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

using CommonServiceLocator;

using CoreModel.Interfaces;

namespace CoreWpf
{
    public class EntryPoint
    {
        // All WPF applications should execute on a single-threaded apartment (STA) thread
        [STAThread]
        public static void Main ( string [] args )
        {

            // intialize service locator
            var bootstrapper = new Bootstrapper ();
            if ( bootstrapper == null )
                throw new NullReferenceException ( "Failed to create bootstrapper" );

            bootstrapper.Run ();

            var app = new App ();
            if ( app == null )
                throw new NullReferenceException ( "Failed to create app" );

            app.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler ( Current_DispatcherUnhandledException );
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler ( CurrentDomain_UnhandledException );
            //AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            //TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException1; ;

            try
            {
                app.InitializeComponent ();

                // set main window gtapplication will show it
                app.MainWindow = new MainWindow ();


                app.Run ();
            }
            catch ( System.ComponentModel.Win32Exception e )
            {
                Debug.WriteLine ( "Win32 exception show message box" );
                MessageBox.Show ( "A Windows error occured. will need to shutdown. Please contact technical support error", "Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( "Exception show message box" );
                MessageBox.Show ( "An error occured. will need to shutdown. Please contact technical support error", "Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }


            Process.GetCurrentProcess ().Kill ();
        }

        private static void CurrentDomain_FirstChanceException ( object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e )
        {
            Exception ex = e.Exception;
        }

        private static void TaskScheduler_UnobservedTaskException1 ( object sender, UnobservedTaskExceptionEventArgs e )
        {
            Exception ex = e.Exception;

        }


        #region Internal Exception Handlers


        static object _classLock = new object ();
        static object ClassLock
        {
            get { return _classLock; }
        }




        private static void Current_DispatcherUnhandledException ( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
        {
            e.Handled = true;

            var ex = e.Exception;

            if ( ex.InnerException != null )
                ex = ex.InnerException;


            Application.Current.Shutdown ();
        }

        public static void CurrentDomain_UnhandledException ( object sender, UnhandledExceptionEventArgs e )
        {
            var ex = e.ExceptionObject as Exception;

            if ( ex.InnerException != null )
                ex = ex.InnerException;

            Application.Current.Shutdown ();
        }

        #endregion
    }

}
