/// <summary>
/// Copyright 2022, Loki Alexander Button Hornsby (Loki Hornsby), All rights reserved.
/// Licensed under the BSD 3-Clause "New" or "Revised" License
/// </summary>

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

using Interfaces;

namespace Software {
    public static class Toolbar {
        // Events
        public delegate void ToolbarEvent(CustomInterface i);
        public static event ToolbarEvent InterfaceChanged;

        /// <summary>
        /// https://stackoverflow.com/questions/13603654/check-only-one-toolstripmenuitem
        /// </summary>
        static void UncheckOtherToolStripMenuItems(ToolStripMenuItem selectedMenuItem) {
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
        static void SetVerboseLevel(object sender, EventArgs? e, Logger.VerboseType type) {
            // Change verbosity
            Logger.SetVerbosity(type);

            // Uncheck unneeded items
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        }

        /// <summary>
        /// Change loaded interface
        /// </summary>
        static void ChangeInterface(object sender, EventArgs? e){
            // Uncheck unneeded items
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);

            // Trigger Event
            string DLL = @"C:\Program Files (x86)\Steam\steamapps\common\Broforce\Broforce_beta_Data\Managed\Assembly-CSharp.dll";
            CustomInterface inter = new CustomInterface("Dummy", DLL);
            
            // The selected Interface has been changed
            InterfaceChanged(inter);
        }

        /// <summary>
        /// Add a new interface
        /// </summary>
        static void AddInterface(object sender, EventArgs? e, Form form){
            // Configuration menu
            Form other = new InterfaceGUI();
            other.textBox.Text = "Hi!";
            form.ShowDialogue(other);
        }

        /// <summary>
        /// Toolbar Setup
        /// </summary>
        public static void Setup(Form form){
            // Menu Strip
            MenuStrip ms = new MenuStrip();

            // Options
            ToolStripMenuItem options = new ToolStripMenuItem("Interfaces");

            // Get interfaces
            List<CustomInterface> interfaces = Loader.GetInterfaces();

            // Interface(s) found!
            if (interfaces != null && interfaces.Count > 0){
                // Load interfaces into the toolbar
                for (int i = 0; i < interfaces.Count; i++){
                    ToolStripMenuItem change = new ToolStripMenuItem(interfaces[i].GetName(), null, new EventHandler(ChangeInterface));
                    options.DropDownItems.Add(change);
                }
            }

            // New interface button
            ToolStripMenuItem New = new ToolStripMenuItem("Add new interface", null, new EventHandler((sender, e) => AddInterface(form)));
            options.DropDownItems.Add(New);

            // Append options
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
            SetVerboseLevel(low_verbose, null, Logger.VerboseType.Low);
            
            // Finishing Setup 
            ms.Dock = DockStyle.Top;
            form.MainMenuStrip = ms;
            form.Controls.Add(ms);
        }
    }
}