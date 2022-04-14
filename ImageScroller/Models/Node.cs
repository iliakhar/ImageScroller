using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ImageScroller.Models
{
    public class Node
    {
        public ObservableCollection<Node> Subfolders { get; set; }

        public string strNodeText { get; set; }
        public string strFullPath { get; }

        public Node(string _strFullPath)
        {
            strFullPath = _strFullPath;
            strNodeText = Path.GetFileName(_strFullPath);
        }

        public static ObservableCollection<Node> GetSubfolders(string strPath)
        {
            ObservableCollection<Node> subfolders = new ObservableCollection<Node>();
            string[] subdirs = Directory.GetDirectories(strPath, "*", SearchOption.TopDirectoryOnly);
            string[] subfiles = Directory.GetFiles(strPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in subdirs)
            {
                Node thisnode = new Node(dir);

                if (Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Length > 0 ||
                    Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly).Length > 0)
                {
                    thisnode.Subfolders = new ObservableCollection<Node>();

                    thisnode.Subfolders = GetSubfolders(dir);
                }
                subfolders.Add(thisnode);

            }

            foreach (string fl in subfiles)
            {
                Node thisnode = new Node(fl);
                if(fl.Contains(".png") || fl.Contains(".jpg") || fl.Contains(".ico") || fl.Contains(".gif"))
                    subfolders.Add(thisnode);
            }

            return subfolders;
        }
    }
}
