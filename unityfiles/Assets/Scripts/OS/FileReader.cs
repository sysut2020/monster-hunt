using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Monsterhunt.Fileoperation {
    /// <summary>
    /// Reads the content of a file line by line or the full content as a string.
    /// </summary>
    public class FileReader {

        public interface IStringContent {
            string GetString();
        }

        public interface ILineContent {
            List<string> AsList();
            string[] AsArray();
        }

        private class StringContent : IStringContent {

            public StringContent() {}

            public StringContent(string content) {
                this.Content = content;
            }

            private string Content { get; set; } = "";

            /// <summary>
            /// Returns the string content of the file that was read from,
            /// empty string if there was no content or IO error
            /// </summary>
            /// <returns></returns>
            public string GetString() {
                return this.Content;
            }

        }

        private class LineContent : ILineContent {

            public LineContent() {}

            public LineContent(string[] content) {
                this.Content = content;
            }

            private string[] Content { get; set; } = new string[0];

            /// <summary>
            /// Returns each line of the file as a list of strings, returns
            /// empty list if there is no file content or IO error.
            /// </summary>
            /// <returns></returns>
            public List<string> AsList() {
                return this.Content.ToList();
            }

            /// <summary>
            /// Retunrs each line of the file as an array, returns empty array if
            /// there is no file content or IOerror.
            /// </summary>
            /// <returns>file content as array</returns>
            public string[] AsArray() {
                return this.Content;
            }

        }

        private string filePath;

        private string FilePath {
            get { return filePath; }
            set {
                if (value == null) {
                    throw new NullReferenceException("Path to file is null");
                } else {
                    filePath = value;
                }
            }
        }

        /// <summary>
        /// Creates the file reader object and assign the path to read from.
        /// </summary>
        /// <param name="filePath"></param>
        public FileReader(string filePath) {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Reads all lines in a file, and returns a ILineContent object
        /// to get content as array or list of string.
        /// </summary>
        /// <returns>ILineContent object</returns>
        public ILineContent ReadAllLines() {
            LineContent lc;
            try {
                lc = new LineContent(File.ReadAllLines(this.filePath));
            } catch (SystemException e) {
                Debug.LogError(e.Message);
                lc = new LineContent();
            }
            return lc;
        }

        /// <summary>
        /// Reads the full content of a file as a string and 
        /// returns a IStringContent object
        /// </summary>
        /// <returns>IStringContent object</returns>
        public IStringContent ReadAllText() {
            StringContent sc;
            try {
                sc = new StringContent(File.ReadAllText(this.filePath));
            } catch (SystemException e) {
                Debug.LogError(e.Message);
                sc = new StringContent();
            }
            return sc;
        }

    }
}