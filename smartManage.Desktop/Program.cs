using System;
using System.Threading;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Subscribe to thread (unhandled) exception events
            ThreadExceptionHandler handler = new ThreadExceptionHandler();
            Application.ThreadException += new ThreadExceptionEventHandler(handler.Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmPrincipal());
            Application.Run(new frmSplash());
            //Application.Run(new TestQRCode());
        }
    }

    /// 
    /// Handles a thread (unhandled) exception.
    /// 
    internal class ThreadExceptionHandler
    {
        static System.Resources.ResourceManager stringManager = null;

        internal ThreadExceptionHandler()
        {
            //Initialisation des Resources
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.Load("ResourcesData");
            stringManager = new System.Resources.ResourceManager("ResourcesData.Resource", _assembly);
        }
        /// 
        /// Handles the thread exception.
        /// 
        public void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                // Exit the program if the user clicks Abort.
                DialogResult result = ShowThreadExceptionDialog(e.Exception);

                if (result == DialogResult.Abort)
                    Application.Exit();
            }
            catch
            {
                // Fatal error, terminate program
                try
                {
                    MessageBox.Show(stringManager.GetString("StringSystemErrorMessage", System.Globalization.CultureInfo.CurrentUICulture), stringManager.GetString("StringSystemErrorCaption", System.Globalization.CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                finally
                {
                    Application.Exit();
                }
                throw;
            }
        }

        /// 
        /// Creates and displays the error message.
        /// 
        private static DialogResult ShowThreadExceptionDialog(Exception ex)
        {
            string errorMessage = stringManager.GetString("StringSystemErrorMessage", System.Globalization.CultureInfo.CurrentUICulture);

            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur inattendue : smartManage.Desktop.Program : " + ex.GetType().ToString() + " : " + ex.Message;
            ManageUtilities.ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);

            return MessageBox.Show(errorMessage, stringManager.GetString("StringSystemErrorCaption", System.Globalization.CultureInfo.CurrentUICulture), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }
    } // End ThreadExceptionHandler
}