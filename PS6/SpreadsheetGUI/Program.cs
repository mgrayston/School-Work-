using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SSApplicationContext appContext = SSApplicationContext.getAppContext();
            appContext.RunWindow(new SpreadsheetGUI());
            Application.Run(appContext);
        }
    }

    /// <summary>
    /// Helper class to track the number of open GUI windows
    /// </summary>
    class SSApplicationContext : ApplicationContext
    {
        private int windowCount = 0;

        private static SSApplicationContext SS_context;

        /// <summary>
        /// Constructor: reaturns a new SSApplicationContext
        /// </summary>
        /// <returns></returns>
        public static SSApplicationContext getAppContext()
        {
            if (SS_context == null)
            {
                SS_context = new SSApplicationContext();
            }
            return SS_context;
        }

        /// <summary>
        /// Tracks the number of open windows in a thread
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public int RunWindow(Form window)
        {
            // track window count
            windowCount++;

            //exit thread if last window 
            window.FormClosed += (o, e) => 
            {
                windowCount--;
                if (windowCount <= 0)
                    ExitThread();
            };

            // Run the form
            window.Show();

            return windowCount;
        }
    }
}
