﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backup.Properties;

namespace Backup
{
    class ProcessIcon : IDisposable
    {
        /// <summary>
        /// The NotifyIcon object.
        /// </summary>
        NotifyIcon ni;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessIcon"/> class.
        /// </summary>
        public ProcessIcon()
        {
            // Instantiate the NotifyIcon object.
            ni = new NotifyIcon();
        }

        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.          
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Icon = Resources.SystemTrayIcon;
            ni.Text = "System Tray Utility Application Demonstration Program";
            ni.Visible = true;

            // Attach a context menu.
            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            // When the application closes, this will remove the icon from the system tray immediately.
            ni.Dispose();
        }

        /// <summary>
        /// Handles the MouseClick event of the ni control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            // Handle mouse button clicks.
            if (e.Button == MouseButtons.Left)
            {
                // Start Windows Explorer.
                System.Diagnostics.Process.Start("explorer", null);
            }
        }
    }
}

