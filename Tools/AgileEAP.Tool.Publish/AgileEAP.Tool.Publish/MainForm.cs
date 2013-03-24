using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace AgileEAP.Tool.Publish
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtInput.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        List<string> IgnoreFolders = new List<string>() { "Log", "obj", "TempFileDirectory" };

        bool Incloude(string file)
        {
            return !file.EndsWith(".cs") && !file.EndsWith(".csproj") && !file.EndsWith(".csproj.user");
        }

        void PackagePluginBin(string sourcePath, string destPath, string shareBinPath)
        {
            foreach (var file in Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (Incloude(file))
                {
                    try
                    {
                        string destFile = string.Empty;
                        string fileName = Path.GetFileName(file);
                        if (fileName.Contains(".Plugin."))
                        {
                            if (!Directory.Exists(destPath))
                                Directory.CreateDirectory(destPath);

                            destFile = Path.Combine(destPath, Path.GetFileName(file));
                        }
                        else
                        {
                            destFile = Path.Combine(shareBinPath, Path.GetFileName(file));
                        }

                        File.Copy(file, destFile);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex);
                    }
                }
            }

            foreach (var dir in Directory.GetDirectories(sourcePath, "*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    string newDirName = Path.GetFileName(dir);
                    if (!IgnoreFolders.Contains(newDirName))
                    {
                        var newDir = Path.Combine(shareBinPath, newDirName);
                        PackageFiles(dir, newDir);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex);
                }
            }
        }

        string shareBinPath = string.Empty;

        void PackageFiles(string sourcePath, string destPath)
        {
            foreach (var file in Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (Incloude(file))
                {
                    try
                    {
                        if (!Directory.Exists(destPath))
                            Directory.CreateDirectory(destPath);

                        string destFile = Path.Combine(destPath, Path.GetFileName(file));
                        File.Copy(file, destFile);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex);
                    }
                }
            }

            foreach (var dir in Directory.GetDirectories(sourcePath, "*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    string newDirName = Path.GetFileName(dir);

                    if (!IgnoreFolders.Contains(newDirName))
                    {
                        var newDir = Path.Combine(destPath, newDirName);
                        if (string.Equals(newDirName, "bin", StringComparison.OrdinalIgnoreCase))
                        {
                            PackagePluginBin(dir, newDir, shareBinPath);
                        }
                        else
                        {
                            PackageFiles(dir, newDir);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex);
                }
            }
        }

        private void btnPackage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text) || string.IsNullOrEmpty(txtOutput.Text))
            {
                MessageBox.Show("源代码目录和输出目录都不能为空！");
                return;
            }

            lblMessage.Text = "正在处理，请稍等!";

            shareBinPath = Path.Combine(txtOutput.Text.Trim(), "bin");
            if (!Directory.Exists(shareBinPath))
                Directory.CreateDirectory(shareBinPath);

            PackageFiles(txtInput.Text.Trim(), txtOutput.Text.Trim());

            MessageBox.Show("处理完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtOutput.Text = folderBrowserDialog2.SelectedPath;
            }
        }
    }
}
