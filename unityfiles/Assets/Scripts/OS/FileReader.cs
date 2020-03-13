using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Monsterhunt.Fileoperation {
    public class FileReader {

        public class FileContent {

            internal FileContent() { }
            internal string[] Content { get; set; } = new string[0];
            public List<string> AsList() {
                return this.Content.ToList();
            }

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

        public FileReader(string filePath) {
            this.FilePath = filePath;
        }

        public FileContent ReadAllLines() {
            var fc = new FileContent();
            try {
                fc.Content = File.ReadAllLines(this.filePath);
            } catch (SystemException e) {
                Debug.LogError(e.Message);
            }
            return fc;
        }

    }
}