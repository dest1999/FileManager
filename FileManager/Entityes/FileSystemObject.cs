using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager
{
    internal abstract class FileSystemObject : IOperations
    {
        public string FullName { get; protected set; }
        public string Name { get; protected set; }//name only
        public string? Parent { get; protected set; }
        public string CurrentDirectory { get; protected set; }
        public bool headOfDirectory { get; protected set; } = false;

        public abstract (bool, Exception) Create(string name);
        public abstract (bool, Exception) Delete();
        public abstract (bool, Exception) Rename(string newName);
        public abstract (bool, Exception) Copy(FileSystemObject destination);
        public abstract (bool, Exception) Move(FileSystemObject destination);
        public abstract void Info();

        public abstract FileSystemObject Run(); //для папки переход в неё, для файла-запуск его или ассоциированного приложения

        public override string ToString()
        {
            if (headOfDirectory)
            {
                return FullName ?? "<TOP LEVEL>";
            }
            return Name;
        }


    }
}
