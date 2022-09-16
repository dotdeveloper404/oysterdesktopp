using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace App
{
    /// <summary>
    /// Class implementing support for "minimize to tray" functionality.
    /// </summary>
    public static class MinimizeToTray
    {
        /// <summary>
        /// Enables "minimize to tray" behavior for the specified Window.
        /// </summary>
        /// <param name="window">Window to enable the behavior for.</param>
        public static void Enable(Window window)
        {
            // No need to track this instance; its event handlers will keep it alive
            new MinimizeToTrayInstance(window);
        }

        /// <summary>
        /// Class implementing "minimize to tray" functionality for a Window instance.
        /// </summary>
        private class MinimizeToTrayInstance
        {
            private Window _window;
            private NotifyIcon _notifyIcon;
            private bool _balloonShown;

            /// <summary>
            /// Initializes a new instance of the MinimizeToTrayInstance class.
            /// </summary>
            /// <param name="window">Window instance to attach to.</param>
            public MinimizeToTrayInstance(Window window)
            {
                Debug.Assert(window != null, "window parameter is null.");
                _window = window;
                _window.StateChanged += new EventHandler(HandleStateChanged);
            }

            /// <summary>
            /// Handles the Window's StateChanged event.
            /// </summary>
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleStateChanged(object sender, EventArgs e)
            {
                if (_notifyIcon == null)
                {
                    // Initialize NotifyIcon instance "on demand"
                    _notifyIcon = new NotifyIcon();
                    _notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
                    _notifyIcon.MouseDoubleClick += new MouseEventHandler(HandleNotifyIconOrBalloonClicked);
                    _notifyIcon.BalloonTipClicked += new EventHandler(HandleNotifyIconOrBalloonClicked);

                }
                // Update copy of Window Title in case it has changed
                _notifyIcon.Text = _window.Title;

                _notifyIcon.ContextMenu = new ContextMenu();
                _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Contact Support", new EventHandler(ContactSupport)));
                _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("-"));

                _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Quit OysterVPN", new EventHandler(Exit)));
                _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Show OysterVPN", new EventHandler(ShowApp)));

                // Show/hide Window and NotifyIcon
                var minimized = (_window.WindowState == WindowState.Minimized);
                _window.ShowInTaskbar = !minimized;
                _notifyIcon.Visible = minimized;
                if (minimized && !_balloonShown)
                {
                    // If this is the first time minimizing to the tray, show the user what happened
                    _notifyIcon.ShowBalloonTip(1000, null, _window.Title, ToolTipIcon.None);
                    _balloonShown = true;
                }
            }

            /// <summary>
            /// Handles a click on the notify icon or its balloon.
            /// </summary>
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleNotifyIconOrBalloonClicked(object sender, EventArgs e)
            {
                // Restore the Window
               _window.WindowState = WindowState.Normal;
            }

            private void Exit(object sender,EventArgs e)
            {
                OysterVPN.OysterVpn.disconnect();
                Environment.Exit(Environment.ExitCode);
            }

            private void ShowApp(object sender,EventArgs e)
            {
                _window.WindowState = WindowState.Normal;
            }

            private void ContactSupport(object sender,EventArgs e)
            {
                try
                {
                    string navigateUri = "http://support.oystervpn.co/?utm_source=app&utm_medium=windows&utm_campaign=support";
                    Process.Start(new ProcessStartInfo(navigateUri));
                }
                catch (Exception ex)
                {
                    ILog logger = log4net.LogManager.GetLogger("ErrorLog");
                    logger.Error(ex.Message);
                }
            }
        }
    }
}
