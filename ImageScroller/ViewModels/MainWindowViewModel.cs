using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Reactive;
using System.Text;
using Avalonia;
using Avalonia.Platform;
using ImageScroller.Models;
using ReactiveUI;


namespace ImageScroller.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool isArrowsEnable;
        public bool IsArrowsEnable
        {
            get => isArrowsEnable;
            set=>this.RaiseAndSetIfChanged(ref isArrowsEnable, value);
        }
        private List<string> imgInDirect;
        private string str;
        public string Str
        {
            get => str;
            set => this.RaiseAndSetIfChanged(ref str, value);
        }
        private Node selectItem;
        public Node SelectItem
        {
            get => selectItem;
            set
            {
                if(value.strNodeText.Contains(".png") || value.strNodeText.Contains(".jpg") || value.strNodeText.Contains(".ico")
                    || value.strNodeText.Contains(".gif"))
                {
                    selectItem = value;
                    imgInDirect.Clear();
                    int imgCount = 0;
                    Str = selectItem.strFullPath;
                    string pathDir = selectItem.strFullPath.Substring(0, selectItem.strFullPath.LastIndexOf(@"\"));
                    string[] allFiles = Directory.GetFiles(pathDir, "*", SearchOption.TopDirectoryOnly);
                    foreach (var dir in allFiles)
                    {
                        if (dir.Contains(".png") || dir.Contains(".jpg") || dir.Contains(".ico") || value.strNodeText.Contains(".gif"))
                        {
                            imgCount++;
                            imgInDirect.Add(dir);
                        }
                        IsArrowsEnable = imgCount > 1;
                    }
                }
                
            }
        }
        public ObservableCollection<Node> Items { get; }
        public ObservableCollection<Node> SelectedItems { get; }
        public string strFolder { get; set; }

        public MainWindowViewModel()
        {
            strFolder = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin\")) + @"\Assets";
            Str = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin\"))+@"\Assets\avalonia-logo.ico";
            IsArrowsEnable = false;
            imgInDirect = new List<string>();
            imgInDirect.Add(Str);

            Items = new ObservableCollection<Node>();

            Node rootNode = new Node(strFolder);
            rootNode.Subfolders = Node.GetSubfolders(strFolder);

            Items.Add(rootNode);

            MoveToLeft = ReactiveCommand.Create(() => MoveByArrow(-1));
            MoveToRight = ReactiveCommand.Create(() => MoveByArrow(1));
        }
        public ReactiveCommand<Unit, int> MoveToLeft { get; }
        public ReactiveCommand<Unit, int> MoveToRight { get; }
        public int MoveByArrow(int direction)
        {
            int ind = imgInDirect.BinarySearch(Str);
            if(direction < 0)
            {
                if (ind > 0)
                    Str = imgInDirect[ind - 1];
                else
                    Str = imgInDirect[imgInDirect.Count - 1];
            }
            if (direction > 0)
            {
                if (ind < imgInDirect.Count - 1)
                    Str = imgInDirect[ind + 1];
                else
                    Str = imgInDirect[0];
            }
            return 0;
        }
    }
}
