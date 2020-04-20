using System;
using System.IO;
using UnityEngine;

namespace Monsterhunt.Fileoperation {
    /// <summary>
    /// Reads the content of a file line by line or the full content as a string.
    /// </summary>
    public class FileReader {
        public interface ILineContent {
            string[] AsArray();
        }

        private class LineContent : ILineContent {
            /// <summary>
            /// Empty constructor to initialize object without any content
            /// </summary>
            public LineContent() {
            }

            public LineContent(string[] content) {
                this.Content = content;
            }

            private string[] Content { get; } = new string[0];

            /// <summary>
            /// Returns each line of the file as an array, returns empty array if
            /// there is no file content or IO error.
            /// </summary>
            /// <returns>file content as array</returns>
            public string[] AsArray() {
                return this.Content;
            }
        }

        private string filePath;

        private string FilePath {
            set {
                if (value == null) {
                    throw new NullReferenceException("Path to file is null");
                }

                filePath = value;
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
            }
            catch (SystemException e) {
                Debug.LogError(e.Message);
                lc = new LineContent();
            }

            return lc;
        }
    }
}