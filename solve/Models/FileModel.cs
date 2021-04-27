using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solve.Models
{
    public class FileModel
      
    {
        private static FileModel instance;
        protected FileModel(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public static FileModel getInstance(string name, string path)
        {
            if (instance == null)
            {
                instance = new FileModel(name, path);
            }
            return instance;
        }





        public string Name { get; set; }
        public string Path { get; set; }
    }
}
