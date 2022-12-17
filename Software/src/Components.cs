namespace Software {
        /*
        /// <summary>
        /// Reset STORE.txt
        /// </summary>
        void Reset(object sender, EventArgs e){
            Logger.Clear();

            if (File.Exists(BI.EXE.StorageFilePath)){ 
                //Logger.log("Resetting STORE.txt.", Logger.LogType.Success, Logger.VerboseType.Medium);

                File.Delete(BI.EXE.StorageFilePath);
            } else {
                //Logger.log("Already reset STORE.txt!", Logger.LogType.Warning, Logger.VerboseType.Medium);
            }

            //Logger.log("Resetting...", Logger.LogType.Success, Logger.VerboseType.Low);

            StartGUI();
        }

        /// <summary>
        /// Log the launch info
        /// </summary>
        void GenerateLaunchInfo(object sender, DragEventArgs e){
            bool fail = false;
            bool retry = false;
            string s = "";

            /// <summary>
            /// Checks wether STORE.txt exists
            /// </summary>
            if (File.Exists(BI.EXE.StorageFilePath)){
                s = "Found STORE.txt";
            } else {
                s = "STORE.txt was not found.";

                fail = true;
            }

            //Logger.log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Medium);
            
            /// <summary>
            /// Checks wether the path inside STORE.txt exists
            /// </summary>
            if (BI.EXE.GetLocation() != null){
                s = "Found executable";
            } else {
                s = "Couldn't Find executable";

                fail = true;
            }

            //Logger.log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Medium);
            
            /// <summary>
            /// Final output - decides wether reset is needed or if operation was successfull
            /// </summary>
            if (!fail){ 
                s = "Opening ... This will log anything you need to see whilst playing.";
            } else {
                retry = true;

                if (File.Exists(BI.EXE.StorageFilePath)){ 
                    s = "The path in STORE.txt was incorrect. Resetting...";
    
                    File.Delete(BI.EXE.StorageFilePath);
                } else {
                    s = "Asking user to upload executable file for ...";
                }
            }

            //Logger.log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Low);

            // Reset if needed
            if (retry){ 
                StartGUI();
            } else if (!fail) { // Continue into load sequence
                BI.BeginLoad();
            }
        }

        /// <summary>
        /// Check if program is admin
        /// </summary>
        bool IsAdministrator(){
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        void StartGUI(string ID, string name, string path){
            // Tip
            if (!IsAdministrator()) //Logger.log(
                "If you would like to see a higher level of logging at startup then run this application as administrator. This will stop you from uploading files for security reasons.", 
                Logger.LogType.Success, Logger.VerboseType.Low
            );

            Logger.AddNewLine();

            // Starting Message
            if (BI.EXE.GetLocation() == null){ // Exe location found?
                // Drag and drop
                Logger.AllowDragAndDrop(true);
                Logger.TxtBox.DragDrop += (sender, e) => GenerateLaunchInfo(sender, e);

                // Text
                //Logger.log("Interface ID: " + ID, Logger.LogType.Success, Logger.VerboseType.Low);
        
                //Logger.log(
                    @"Drag and Drop " + 
                    name +
                    " into this window. (This is usually located at " + 
                    path + ")", 
                    Logger.LogType.Success, 
                    Logger.VerboseType.Low
                );

                if (BI.InstanceIsRunning(BI.EXE.GetLocation())) //Logger.log(@"/!\ Ensure that you close any running instances of your selected game before doing this /!\", Logger.LogType.Warning, Logger.VerboseType.Low);
            } else {
                // Drag and drop
                Logger.AllowDragAndDrop(false);

                // Show launch info
                GenerateLaunchInfo(null, null);
            }
        }
        */
}