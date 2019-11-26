
namespace FileSpy
{
    #region  using directive

    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    #endregion
    public partial class MainWindow : INotifyPropertyChanged
    {

        public ObservableCollection<TreeNode> Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
                this.OnPropertyChanged("Source");
            }
        }

        private void TraversingDevice()
        {
            var devices = Environment.GetLogicalDrives();
            foreach (var device in devices)
            {
                var temp = new TreeNode(null)
                {
                    Name = device.Substring(0, device.IndexOf(':')),
                    IsDevice = true,
                    Path = device,
                };
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.source.Add(temp);
                }));
                Task task = new Task(() =>
                {
                    this.TraversingFolder(temp);
                });
                task.Start();
            }
        }

        private Int64 TraversingFolder(TreeNode path)
        {
            Int64 result = 0;
            try
            {
                foreach (var folder in Directory.GetDirectories(path.Path))
                {
                    var temp = new TreeNode(path)
                    {
                        Name = folder.Substring(folder.Replace('\\', '/').LastIndexOf('/') + 1),
                        IsFolder = true,
                        Path = folder,
                    };
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        path.ChildNodes.Add(temp);
                    }));
                    result += this.TraversingFolder(temp);
                }
                foreach (var file in Directory.GetFiles(path.Path))
                {
                    var temp = new TreeNode(path)
                    {
                        Name = file.Substring(file.Replace('\\', '/').LastIndexOf('/') + 1),
                        IsFile = true,
                        Path = file,
                    };
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        path.ChildNodes.Add(temp);
                    }));
                    FileInfo info = new FileInfo(file);
                    temp.Length = info.Length;
                    result += info.Length;
                }
                path.Length = result;
                path.Size = result.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<TreeNode> source = new ObservableCollection<TreeNode>();
        private void OnPropertyChanged(in String propertyName)
        {
            var temp = this.PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
