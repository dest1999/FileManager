using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    internal class Folder : FileSystemObject
    {
        private DirectoryInfo directoryInfo;
        public Folder()
        {

        }
        public Folder(string name, bool isHead = false)
        {
            directoryInfo = new (name);
            Name = directoryInfo.Name + "\\";//name only
            Parent = directoryInfo.Parent?.FullName;//path only
            FullName = directoryInfo.FullName;
            headOfDirectory = isHead;
            if (isHead)
            {
                CurrentDirectory = FullName;
            }
            else
            {
                CurrentDirectory = Parent;
            }
        }

        public override (bool, Exception) Copy(FileSystemObject destination)
        {
            try
            {
                destination = new Folder(Path.Combine(destination.FullName, Name));
                destination.Create(destination.FullName);

                foreach (var file in directoryInfo.GetFiles())
                {
                    new File(file.FullName).Copy(destination);
                }

                foreach (var dir in directoryInfo.GetDirectories())
                {
                    var newFolder = new Folder(dir.FullName);
                    newFolder.Copy(destination);
                }

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
                DirectoryInfo di = new(name);
                di.Create();
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public override (bool, Exception) Delete()
        {
            if (!headOfDirectory)
            {
                try
                {
                    this.directoryInfo.Delete(true);
                    return (true, null);
                }
                catch (Exception e)
                {
                    return (false, e);
                }
            }
            return (false, null);
        }

        //public override FileSystemObject GetParent()
        //{
        //    var parent = directoryInfo.Parent ;
        //    return new Folder(parent.FullName);
        //}

        public override void Info()
        {
            throw new NotImplementedException();
        }

        public override (bool, Exception) Move(FileSystemObject destination)
        {
            try
            {
                destination = new Folder(Path.Combine(destination.FullName, Name));
                destination.Create(destination.FullName);

                foreach (var file in directoryInfo.GetFiles())
                {
                    new File(file.FullName).Move(destination);
                }

                foreach (var dir in directoryInfo.GetDirectories())
                {
                    var newFolder = new Folder(dir.FullName);
                    newFolder.Move(destination);
                }
                
            }
            catch (Exception e)
            {
                return (false, e);
            }
            this.Delete();
            return (true, null);
        }

        public override (bool, Exception) Rename(string newName)
        {
            try
            {
                directoryInfo.MoveTo(Path.Combine(directoryInfo.Parent.ToString(), newName));
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }

        public override FileSystemObject Run()
        {
            this.headOfDirectory = true;
            return this;
        }
    }
}
