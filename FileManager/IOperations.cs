using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    internal interface IOperations
    {
        (bool, Exception) Delete();
        (bool, Exception) Rename(string newName);
        FileSystemObject Run();
        (bool, Exception) Copy(FileSystemObject destination);
        (bool, Exception) Move(FileSystemObject destination);
        void Info();
        (bool, Exception) Create(string name);
    }
}
