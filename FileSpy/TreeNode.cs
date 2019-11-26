namespace FileSpy
{
    #region using directive

    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    #endregion

    public sealed class TreeNode : INotifyPropertyChanged
    {
        public TreeNode(TreeNode parent)
        {
            this.parentNode = parent;
        }

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.OnProperty("Name");
            }
        }

        public String Size
        {
            get
            {
                String unit = "B";
                Double size = this.Length;
                while (size > 1024.0)
                {
                    size = size / 1024.0;
                    switch (unit)
                    {
                        case "B":
                            unit = "KB";
                            break;
                        case "KB":
                            unit = "MB";
                            break;
                        case "MB":
                            unit = "GB";
                            break;
                    }
                }
                var result = size.ToString("F4") + " " + unit;
                if (size == 0)
                {
                    result = "****";
                }
                return result;
            }
            set
            {
                this.OnProperty("Size");
            }
        }

        public Int64 Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        public ObservableCollection<TreeNode> ChildNodes
        {
            get
            {
                return this.childNodes;
            }
            set
            {
                this.childNodes = value;
                this.OnProperty("ChildNode");
            }
        }

        public String Image
        {
            get
            {
                var result = String.Empty;
                if (this.IsDevice)
                {
                    result = @"images\device.ico";
                }
                else if (this.IsFolder)
                {
                    result = @"images\folder.ico";
                }
                else if (this.IsFile)
                {
                    result = @"images\file.ico";
                }
                return result;
            }
        }

        public String Path { get; set; }

        public TreeNode ParentNode { get { return this.parentNode; } }

        public bool IsFile { get; set; }
        public bool IsFolder { get; set; }
        public bool IsDevice { get; set; }

        private void OnProperty(in String propertyName)
        {
            var temp = this.PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private String name = String.Empty;

        private Int64 length = 0;

        private ObservableCollection<TreeNode> childNodes = new ObservableCollection<TreeNode>();
        private readonly TreeNode parentNode;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
