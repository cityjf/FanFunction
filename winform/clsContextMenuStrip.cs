using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace FanFunction
{
    /// <summary>
    /// 追加同目录下可执行文件到上下文菜单中
    /// 调用方法：new clsContextMenuStrip(this.FindForm());
    /// </summary>
    public class clsContextMenuStrip
    {
        //窗体
        System.Windows.Forms.Form form;
        //上下文
        System.Windows.Forms.ContextMenuStrip menu;
        //当前可执行文件目录
        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        /// <summary>
        /// 追加同目录下可执行文件到上下文菜单中
        /// 调用方法：new clsContextMenuStrip(this.FindForm());
        /// </summary>
        public clsContextMenuStrip(Form f)
        {
            form = f;
            menu = f.Controls.Owner.ContextMenuStrip;
            if (menu == null)
            {
                menu = new ContextMenuStrip();
                form.Controls.Owner.ContextMenuStrip = menu;
            }
            Init();
        }
        /// <summary>
        /// 获取当前程序目录下的所有exe，追加到上下文菜单中,排除当前执行文件
        /// </summary>
        private void Init()
        {
            var currExePath = Process.GetCurrentProcess().MainModule.FileName;
            var all = Directory.GetFileSystemEntries(path, "*.exe", SearchOption.AllDirectories);
            foreach (var filePath in all)
            {
                if (currExePath.Equals(filePath, StringComparison.CurrentCultureIgnoreCase))
                    continue;
                string name = Path.GetFileNameWithoutExtension(filePath);
                Image image = Image.FromHbitmap(Icon.ExtractAssociatedIcon(filePath).ToBitmap().GetHbitmap());
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = name;
                item.Image = image;
                item.ToolTipText = filePath;
                item.Click += new EventHandler(item_Click);
                menu.Items.Add(item);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Process.Start(item.ToolTipText);
        }
    }
}
