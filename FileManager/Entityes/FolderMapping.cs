using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    internal class FolderMapping
    {
        public static List<FileSystemObject> GetFolderContent(FileSystemObject currentFolder = null)
        {
            var currentFolderList = new List<FileSystemObject>();

            try
            {
                if(currentFolder == null)
                {
                    currentFolder = new Folder(Directory.GetCurrentDirectory(), true);
                }

                currentFolderList.Add(new Folder(currentFolder.FullName , true));

                foreach (var folder in Directory.GetDirectories(currentFolder.FullName))
                {
                    currentFolderList.Add(new Folder(folder));
                }

                foreach (var file in Directory.GetFiles(currentFolder.FullName))
                {
                    currentFolderList.Add(new File(file));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);//для отладки
            }

            return currentFolderList;
        }
    }
}
