#define LOGGING

using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using IndianaPark.Tools.Logging;
using System.Security.Permissions;

namespace IndianaPark
{
    /// <summary>
    /// Entry point
    /// </summary>
    static class Program
    {
        private static System.Threading.Mutex mc_mutex = null;

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        [SecurityPermission( SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain )]
        private static void Main()
        {
            var text = String.Format(
                "PROGRAM INITIALIZATION\nStart time: {0}\nExecutable Info: {1} - {2}\nProcess Info: {3} - {4} - {5}",
                DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(),
                Path.GetDirectoryName(Application.ExecutablePath),
                Path.GetFileName(Application.ExecutablePath),
                Process.GetCurrentProcess().ProcessName,
                Process.GetCurrentProcess().Id,
                Process.GetCurrentProcess().BasePriority
            );
            Logger.Default.Write( text );

            // Intercetto la chiusura del processo e le eccezioni non gestite per effettuare una chiusura pulita
            /* Attenzione!! Bug con i sistemi a 64 bit che non catturano ThreadException. Per info vedere:
             * - http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=325742
             * - http://stackoverflow.com/questions/1583351/silent-failures-in-c-seemingly-unhandled-exceptions-that-does-not-crash-the-pr */
            Application.SetUnhandledExceptionMode( UnhandledExceptionMode.ThrowException );
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            AppDomain.CurrentDomain.ProcessExit += ProcessExitHandler;
            
            // A causa di concorrenze sul database e visto che è un programma single-user, prevengo la presenza
            // di istanze multiple in esecuzione del programma con l'utilizzo di un mutex
            bool instantiated;
            Program.mc_mutex = new System.Threading.Mutex( false, @"Global\IndianaPark.Mutex", out instantiated );
            if( instantiated )
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
                Application.Run( new IndianaPark.Forms.MainWindow() );
            }
            else
            {
                Logger.Default.Write( "It's not possible to run more than once this program. If you do not see any open windows check the task list in Windows Task Manager", Verbosity.WarningDebug | Verbosity.User );
            }
            
            GC.KeepAlive( Program.mc_mutex );
        }

        /// <summary>
        /// Gestore dell'evento di terminazione del programma
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void ProcessExitHandler( object sender, EventArgs e )
        {
            Logger.Default.Write( "Program terminated" );
        }

        /// <summary>
        /// Gestore delle eccezioni non gestite su tutti i thread
        /// </summary>
        /// <remarks>
        /// Attenzione!! Bug con i sistemi a 64 bit che non catturano ThreadException. Per info vedere:
        /// http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=325742
        /// http://stackoverflow.com/questions/1583351/silent-failures-in-c-seemingly-unhandled-exceptions-that-does-not-crash-the-pr */
        /// </remarks>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        static void UnhandledExceptionHandler( object sender, UnhandledExceptionEventArgs e )
        {
            // Scrittura sul log del programma
            Logger.Default.Write( (Exception)e.ExceptionObject, "Unhandled Exception: program crash", Verbosity.ErrorDebug | Verbosity.User );

            // Scrittura sul log di sistema e termine forzato del programma
            var sysLogger = Logger.Default.Clone( new Tools.Logging.Writers.OSEventLogger() );
            sysLogger.Write( (Exception)e.ExceptionObject, "Unhandled Exception: program crash", Verbosity.ErrorDebug );

            Application.Exit();
        }
    }
}
