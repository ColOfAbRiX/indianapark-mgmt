using System;
using System.Diagnostics;

#pragma warning disable 1591

namespace System
{
    /// <summary>
    /// Reference: http://www.codeproject.com/KB/cs/Execute_Command_in_CSharp.aspx
    /// </summary>
    public static class SystemCommands
    {
        public static string ReadLine( string message )
        {
            Console.Write( message );
            return Console.ReadLine();
        }

        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        public static string ExecuteCommand( object command )
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                var procStartInfo = new ProcessStartInfo( "cmd", "/c " + command );
                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;

                // Now we create a process, assign its ProcessStartInfo and start it
                var proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                return result;
            }
            catch( Exception )
            {
                // Log the exception
            }

            return "";
        }
    }
}
