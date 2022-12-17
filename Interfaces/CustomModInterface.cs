using System;

namespace ModInterface {
    public class CustomModInterface {
        /// <summary>
        /// Data class which stores all the needed information about our interface
        /// </summary>
        public class Data {
            string name;

            /// <summary>
            /// Constructor for data of the interface
            /// </summary>
            public Data(string _name) {
                name = _name;
            }  

            /// <summary>
            /// Return name for the interface
            /// </summary>
            public string GetName(){
                return name;
            } 
        }

        // Data
        Data data;

        /// <summary>
        /// Constructor for our custom interface
        /// </summary>
        /// <param name="name"> Name of your interface </param>
        public CustomModInterface(string name){
            data = new Data(name);
        }   

        /// <summary>
        /// Get data for the interface
        /// </summary>
        public Data GetData() {
            return data;
        }
    }
}