using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace czip
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void AddRecursive(ZipDirectory zdir, TreeNode root)
        {
            TreeNode tn = new TreeNode(zdir.Name, 0, 0);
            tn.Tag = zdir;
            root.Nodes.Add(tn);
            foreach (ZipDirectory dir in zdir.Directories)
            {
                AddRecursive(dir, tn);
            }
            foreach (ZipFile zfile in zdir.Files)
            {
                TreeNode ftn = new TreeNode(zfile.Name, 2, 2);
                ftn.Tag = zfile;
                tn.Nodes.Add(ftn);
            }
        }

        public void OpenCzip(string path)
        {
            ZipDirectory root = Api.Index(path);
            if (root == null) return; // TODO: Handle this
            TreeNode tn = new TreeNode();
            AddRecursive(root, tn);
            tv_main.Nodes.Add(tn.Nodes[0]);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCzip(@"C:\Users\JonasPC\source\repos\czip\czip.czip"); // Temp path for testing
        }

        private void tv_main_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }

        private void tv_main_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;
        }

        private void tv_main_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            gb_info.Text = e.Node.Text;
            lb_fullpath.Text = e.Node.FullPath;
            toolTip.SetToolTip(lb_fullpath, lb_fullpath.Text);
            if (e.Node.Tag is ZipDirectory)
            {
                lb_type.Text = "Type: Directory";
                lbinfo2.Text = $"Directories: {((ZipDirectory)e.Node.Tag).Directories.Count}";
                lbinfo3.Text = $"Files: {((ZipDirectory)e.Node.Tag).Files.Count}";
            }
            else if (e.Node.Tag is ZipFile)
            {
                lb_type.Text = "Type: File";
                lbinfo2.Text = $"Extension: {((ZipFile)e.Node.Tag).Extension}";
                lbinfo3.Text = $"Size: {((ZipFile)e.Node.Tag).Size} bytes";
            }
        }

        private void infoMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoMonitor monitor = new InfoMonitor();
            monitor.Show();
        }
    }
}
