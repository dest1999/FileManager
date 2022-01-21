using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    internal class File : FileSystemObject
    {
        private FileInfo fileInfo;
        public File(string name)
        {
            fileInfo = new (name);
            Name = fileInfo.Name;//name only
            //Parent = fileInfo.FullName;
            Parent = CurrentDirectory = fileInfo.DirectoryName;
        }
        public File()
        {
        }

        public override (bool, Exception) Copy(FileSystemObject destination)
        {
            try
            {
                fileInfo.CopyTo(Path.Combine(destination.FullName, Name), true);
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public override (bool, Exception) Create(string name)
        {
            try
            {
                FileInfo fi = new (name);
                FileStream fs = fi.Create();
                fs.Close();
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public override (bool, Exception) Delete()
        {
            try
            {
                this.fileInfo.Delete();
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
                
            }
        }

        //public override FileSystemObject GetParent()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Info()
        {
            throw new NotImplementedException();
        }

        public override (bool, Exception) Move(FileSystemObject destination)
        {
            try
            {
                fileInfo.MoveTo(Path.Combine(destination.FullName, this.Name), true);
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
            //throw new NotImplementedException();
        }

        public override (bool, Exception) Rename(string newName)
        {
            try
            {
                fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, newName));
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public override FileSystemObject Run()
        {
            try
            {//TODO доработать запуск файлов
                Process.Start(fileInfo.FullName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return this;
        }
    }
}
