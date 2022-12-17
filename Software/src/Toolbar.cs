using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;  
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;
using System.Runtime.CompilerServices;

using ModInterface;

namespace Software {
    public class Toolbar {
        /// <summary>
        /// https://stackoverflow.com/questions/13603654/check-only-one-toolstripmenuitem
        /// </summary>
        void UncheckOtherToolStripMenuItems(ToolStripMenuItem selectedMenuItem) {
            selectedMenuItem.Checked = true;

            // Select the other MenuItens from the ParentMenu(OwnerItens) and unchecked this,
            // The current Linq Expression verify if the item is a real ToolStripMenuItem
            // and if the item is a another ToolStripMenuItem to uncheck this.
            foreach (var ltoolStripMenuItem in (from object 
                                                    item in selectedMenuItem.Owner.Items 
                                                let ltoolStripMenuItem = item as ToolStripMenuItem 
                                                where ltoolStripMenuItem != null 
                                                where !item.Equals(selectedMenuItem) 
                                                select ltoolStripMenuItem))
                    (ltoolStripMenuItem).Checked = false;

            // This line is optional, for show the mainMenu after click
            //selectedMenuItem.Owner.Show();
        }

        /// <summary>
        /// Change Verbosity of logger
        /// </summary>
        void SetVerboseLevel(object sender, EventArgs? e, Logger.VerboseType type) {
            // Change verbosity
            //Logger.SetVerbosity(type);

            // Uncheck unneeded items
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        }

        /// <summary>
        /// Change loaded interface
        /// </summary>
        void ChangeInterface(object sender, EventArgs? e){
            // Get interfaces
            InterfaceLoader.PollInterfaces();

            // Uncheck unneeded items
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        }

        /// <summary>
        /// Toolbar constructor
        /// </summary>
        public Toolbar(Form form){
            // Tool Menu
            form.IsMdiContainer = true;

            // Menu Strip
            MenuStrip ms = new MenuStrip();

            // Options
            ToolStripMenuItem options = new ToolStripMenuItem("Interfaces");

            for (int i = 0; i < 200; i++){
                ToolStripMenuItem change = new ToolStripMenuItem(i.ToString(), null, new EventHandler(ChangeInterface));
                options.DropDownItems.Add(change);
            }

            ms.Items.Add(options);

            // Verbosity
            ToolStripMenuItem verbosity = new ToolStripMenuItem("Verbosity");
            ToolStripMenuItem low_verbose = new ToolStripMenuItem(
                "Low", null, new EventHandler((sender, e) => SetVerboseLevel(sender, e, Logger.VerboseType.Low)));
                verbosity.DropDownItems.Add(low_verbose);
            ToolStripMenuItem medium_verbose = new ToolStripMenuItem(
                "Medium", null, new EventHandler((sender, e) => SetVerboseLevel(sender, e, Logger.VerboseType.Medium)));
                verbosity.DropDownItems.Add(medium_verbose);
            ToolStripMenuItem high_verbose = new ToolStripMenuItem(
                "High", null, new EventHandler((sender, e) => SetVerboseLevel(sender, e, Logger.VerboseType.High)));
                verbosity.DropDownItems.Add(high_verbose);

            ms.Items.Add(verbosity);

            // Override startup verbosity
            /*if (IsAdministrator()){
                SetVerboseLevel(high_verbose, null, Logger.VerboseType.High);

                //Logger.log("Reminder: Running this application as administrator will usually not allow for upload of files. This serves as a way to quickly debug startup.", 
                    //Logger.LogType.Warning, Logger.VerboseType.Low);
            } else {
                SetVerboseLevel(low_verbose, null, Logger.VerboseType.Low);
            }*/
            
            // Finishing Setup 
            ms.Dock = DockStyle.Top;
            form.MainMenuStrip = ms;
            form.Controls.Add(ms);
        }
    }
}