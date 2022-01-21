using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Terminal.Gui;

namespace FileManager
{
    internal partial class Program
    {
        //private static Action running = MainApp;
        public static FileSystemObject currentObjectSelection,
                                        leftPanelDirectory,
                                        rightPanelDirectory;
        private static ListView rightTree = new()
                                {
                                    Width = Dim.Fill(),
                                    Height = Dim.Fill(),
                                },
                                leftTree = new()
                                {
                                    Width = Dim.Fill(),
                                    Height = Dim.Fill(),
                                };

        static void Main(string[] args)
        {
            //while (running != null)
            //{
            //    running.Invoke();
            //}
            //Application.Shutdown();
            MainApp();
        }

        public static void MainApp()
        {
            Application.Init();
            var top = Application.Top;
            var leftPanel = new Window()
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Fill() - 1
            };
            var rightPanel = new Window()
            {
                X = Pos.Percent(50),
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Fill() - 1
            };
            leftPanel.Add(leftTree);
            rightPanel.Add(rightTree);

            leftTree.OpenSelectedItem += LeftTree_OpenSelectedItem;
            rightTree.OpenSelectedItem += RightTree_OpenSelectedItem;

            leftTree.SelectedItemChanged += SelectedItemChanged;
            rightTree.SelectedItemChanged += SelectedItemChanged;

            leftTree.SetSource(FolderMapping.GetFolderContent(new Folder(@"d:\2")));
            rightTree.SetSource(FolderMapping.GetFolderContent(new Folder(@"d:\1") ));//TODO для отладки

            Button renameButton = new ()
            {
                X = 0,
                Y = Pos.Percent(100) - 1,
                Text = "Rename",
                TabStop = false
            };
            Button newFileButton = new ()
            {
                X = Pos.Right(renameButton),
                Y = Pos.Percent(100) - 1,
                Text = "NewF_ile",
                TabStop = false
            };
            Button newFolderButton = new()
            {
                X = Pos.Right(newFileButton),
                Y = Pos.Percent(100) - 1,
                Text = "NewF_older",
                TabStop = false
            };
            Button deleteButton = new()
            {
                X = Pos.Right(newFolderButton),
                Y = Pos.Percent(100) - 1,
                Text = "Delete",
                TabStop = false
            };
            Button copyButton = new()
            {
                X = Pos.Right(deleteButton),
                Y = Pos.Percent(100) - 1,
                Text = "Copy",
                TabStop = false
            };
            Button moveButton = new()
            {
                X = Pos.Right(copyButton),
                Y = Pos.Percent(100) - 1,
                Text = "Move",
                TabStop = false
            };
            renameButton.Clicked += RenameButton_Run;
            newFileButton.Clicked += NewFileButton_Clicked;
            newFolderButton.Clicked += NewFolderButton_Clicked;
            deleteButton.Clicked += DeleteButton_Clicked;
            copyButton.Clicked += CopyButton_Clicked;
            moveButton.Clicked += MoveButton_Clicked;

            top.Add(leftPanel, rightPanel, renameButton, newFileButton, newFolderButton, deleteButton, copyButton, moveButton);
            Application.Run();
        }

        private static void MoveButton_Clicked()
        {
            bool isSuccess = true;
            Exception e = null;
            if (currentObjectSelection is FileSystemObject && !currentObjectSelection.headOfDirectory)
            {
                if (leftPanelDirectory is FileSystemObject && currentObjectSelection.CurrentDirectory != leftPanelDirectory.CurrentDirectory)
                {
                    (isSuccess, e) = currentObjectSelection.Move(leftPanelDirectory);
                }
                else if (rightPanelDirectory is FileSystemObject && currentObjectSelection.CurrentDirectory != rightPanelDirectory.CurrentDirectory)
                {
                    (isSuccess, e) = currentObjectSelection.Move(rightPanelDirectory);
                }
            }
            if (isSuccess)
            {
                ForceUpdateBothPanels();
            }
            else
            {
                ShowErrorMessage(e.Message);
            }
            //throw new NotImplementedException();
        }

        private static void CopyButton_Clicked()
        {
            bool isSuccess = true;
            Exception e = null;
            if (currentObjectSelection is FileSystemObject && !currentObjectSelection.headOfDirectory)
            {
                if(leftPanelDirectory is FileSystemObject && currentObjectSelection.CurrentDirectory != leftPanelDirectory.CurrentDirectory)
                {
                    (isSuccess, e) = currentObjectSelection.Copy(leftPanelDirectory);
                }
                else if (rightPanelDirectory is FileSystemObject && currentObjectSelection.CurrentDirectory != rightPanelDirectory.CurrentDirectory)
                {
                    (isSuccess, e) = currentObjectSelection.Copy(rightPanelDirectory);
                }
            }
            if (isSuccess)
            {
                ForceUpdateBothPanels();
            }
            else
            {
                ShowErrorMessage(e.Message);
            }

        }

        private static void DeleteButton_Clicked()
        {
            (bool isSuccess, Exception e) = currentObjectSelection.Delete();
            if (isSuccess)
            {
                ForceUpdateBothPanels();
            }
            else if (!isSuccess && e != null)
            {
                ShowErrorMessage(e.Message);
            }
        }

        private static void NewFolderButton_Clicked()
        {
            TextField newName = new("")
            {
                Width = Dim.Fill(),
                Height = 1,
                Y = Pos.Center(),
            };

            Button buttonOK = new("OK")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(newName) + 1,
            };

            Button buttonCancel = new("Cancel")
            {
                X = Pos.Right(buttonOK) + 1,
                Y = Pos.Bottom(newName) + 1,
            };

            buttonCancel.Clicked += () => { Application.RequestStop(); };
            buttonOK.Clicked += () => {

                (bool isSuccess, Exception e) = new Folder().Create(Path.Combine(currentObjectSelection.CurrentDirectory, newName.Text.ToString()));
                if (isSuccess)
                {
                    ForceUpdateBothPanels();
                }
                else
                {
                    ShowErrorMessage(e.Message);
                }

                Application.RequestStop();
            };
            var dialog = new Dialog("New Folder", 40, 7, buttonOK, buttonCancel);

            dialog.Add(newName);

            Application.Run(dialog);
        }

        private static void NewFileButton_Clicked()
        {

            TextField newName = new("")
            {
                Width = Dim.Fill(),
                Height = 1,
                Y = Pos.Center(),
            };

            Button buttonOK = new("OK")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(newName) + 1,
            };

            Button buttonCancel = new("Cancel")
            {
                X = Pos.Right(buttonOK) + 1,
                Y = Pos.Bottom(newName) + 1,
            };

            buttonCancel.Clicked += () => { Application.RequestStop(); };
            buttonOK.Clicked += () => {

                (bool isSuccess, Exception e) = new File().Create(Path.Combine(currentObjectSelection.CurrentDirectory , newName.Text.ToString()));
                if (isSuccess)
                {
                    ForceUpdateBothPanels();
                }
                else
                {
                    ShowErrorMessage(e.Message);
                }

                Application.RequestStop();
            };
            var dialog = new Dialog("New File", 40, 7, buttonOK, buttonCancel);

            dialog.Add(newName);

            Application.Run(dialog);

        }

        private static void SelectedItemChanged(ListViewItemEventArgs obj)
        {
            GetChangeDirectoryes();//проверить нужен ли сдесь этот вызов
            if (obj.Value is FileSystemObject fso)
            {
                currentObjectSelection = fso;
            }
        }

        private static void RenameButton_Run()
        {
            TextField newName = new ("")
            {
                Width = Dim.Fill(),
                Height = 1,
                Y = Pos.Center(),
            };

            Button buttonOK = new ("OK")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(newName) + 1,
            };

            Button buttonCancel = new ("Cancel")
            {
                X = Pos.Right(buttonOK) + 1,
                Y = Pos.Bottom(newName) + 1,
            };

            buttonCancel.Clicked += () => { Application.RequestStop(); };
            buttonOK.Clicked += () => {
                (bool isSuccess, Exception e) = currentObjectSelection.Rename(newName.Text.ToString());
                if (isSuccess)
                {
                    ForceUpdateBothPanels();
                }
                else
                {
                    ShowErrorMessage(e.Message);
                }

                Application.RequestStop();
            };
            var dialog = new Dialog("Rename", 40, 7, buttonOK, buttonCancel);

            dialog.Add(newName);

            Application.Run(dialog);
        }

        private static void RightTree_OpenSelectedItem(ListViewItemEventArgs obj)
        {
            OpenSelectedItem(rightTree, obj);
            GetChangeDirectoryes();
        }
        private static void LeftTree_OpenSelectedItem(ListViewItemEventArgs obj)
        {
            OpenSelectedItem(leftTree, obj);
            GetChangeDirectoryes();
        }

        private static void OpenSelectedItem(ListView listView, ListViewItemEventArgs obj)
        {
            if (obj.Value is Folder newRootWievFolder)
            {
                if (newRootWievFolder.headOfDirectory)
                {
                    if (newRootWievFolder.Parent == null)
                    {
                        listView.SetSource(DriveInfo.GetDrives());
                    }
                    else
                    {
                        listView.SetSource(FolderMapping.GetFolderContent(new Folder(newRootWievFolder.Parent)));
                    }
                }
                else
                {
                    listView.SetSource(FolderMapping.GetFolderContent(newRootWievFolder));
                }
            }
            else if (obj.Value is File file)
            {
                file.Run();
            }
            else if (obj.Value is DriveInfo drive)
            {
                listView.SetSource(FolderMapping.GetFolderContent(new Folder(drive.Name)));
            }
        }

    }

}
