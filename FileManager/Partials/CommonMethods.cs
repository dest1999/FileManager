using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace FileManager
{
    internal partial class Program
    {

        private static void ShowErrorMessage(string message)
        {
            MessageBox.ErrorQuery("Error", message, "OK");
        }

        private static void ForceUpdateBothPanels()
        {
            if (rightTree.Source.ToList()[0] is Folder folderR)
            {
                rightTree.SetSource(FolderMapping.GetFolderContent(folderR));
            }
            if (leftTree.Source.ToList()[0] is Folder folderL)
            {
                leftTree.SetSource(FolderMapping.GetFolderContent(folderL));
            }
        }

        private static void GetChangeDirectoryes()
        {
            leftPanelDirectory = leftTree.Source.ToList()[0] as FileSystemObject;
            rightPanelDirectory = rightTree.Source.ToList()[0] as FileSystemObject;
        }

    }
}
