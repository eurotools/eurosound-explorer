using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;

namespace sb_explorer.Classes.PropertyGridHelpers
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    internal class CustomFolderBrowser : UITypeEditor
    {
        //-------------------------------------------------------------------------------------------------------------------------------
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string currentPath = value as string;
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(currentPath) && Directory.Exists(currentPath))
                {
                    folderBrowserDialog.SelectedPath = currentPath;
                }

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    return folderBrowserDialog.SelectedPath;
                }
            }

            return value;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
