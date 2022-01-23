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
            }
            catch (Exception e)
            {
                return (false, e);
            }
            return (true, null);
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

        public override List<string> Info()
        {
            long size = 0;
            bool isCountingSizeSuccess = true;
            List<string> returnList = new();

            try
            {
                foreach (var file in Directory.EnumerateFiles(this.directoryInfo.FullName, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        size += (new File(file)).Size;
                    }
                    catch (Exception)
                    {
                        isCountingSizeSuccess = false;
                    }
                }
            }
            catch (Exception)
            {
                isCountingSizeSuccess = false;
            }

            returnList.Add(Name);
            returnList.Add(size.ToString("N0") + " bytes");
            returnList.Add("Size corrected: " + isCountingSizeSuccess);
            return returnList;
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

        public (bool isSuccess, Exception e, List<FileSystemObject> searchResults) Search(string searchFilename)
        {
            List<FileSystemObject> searchResult = new();
            bool isSuccess = true;


            foreach (var item in Directory.EnumerateDirectories (this.directoryInfo.FullName, searchFilename, SearchOption.AllDirectories))
            {
                searchResult.Add(new Folder(item));
            }
            foreach (var item in Directory.EnumerateFiles(this.directoryInfo.FullName, searchFilename, SearchOption.AllDirectories))
            {
                searchResult.Add(new Folder(item));
            }

            return (isSuccess, null, searchResult);
        }
    }
}
