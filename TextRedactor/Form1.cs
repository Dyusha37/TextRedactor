using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextRedactor
{
    public partial class Form1 : Form
    {
        private string[] lines;
        private int linesPrinted;
        int c;

        public Form1()
        {
            InitializeComponent();
        }
        private string fileName = "Untitled";
        private void ToolStripMenu_New_Click(object sender, EventArgs e)
        {
            fileName = "Untitled";
            richTextBox1.Visible = true;
            richTextBox1.Clear();
        }

        private void ToolStripMenu_Open_Click(object sender, EventArgs e)
        {
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                fileName = dlgOpen.FileName;
                try
                {
                    using (StreamReader reader = File.OpenText(fileName))
                    {
                        richTextBox1.Clear();
                        richTextBox1.Visible = true;
                        richTextBox1.Text = reader.ReadToEnd();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Simple Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }

        private void MenuItemSave_Click(object sender, EventArgs e)
        {
            if (fileName == "Untitled")
                MenuItemSaveAs_Click(sender, e);
            else
            {
                try
                {
                    Stream stream = File.OpenWrite(fileName);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(richTextBox1.Text);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Simple Editor",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void MenuItemSaveAs_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                fileName = dlgSave.FileName;
                try
                {
                    Stream stream = File.OpenWrite(fileName);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(richTextBox1.Text);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Simple Editor",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Brush brush = new SolidBrush(richTextBox1.ForeColor);
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++],
                    richTextBox1.Font, brush, x, y);
                y += richTextBox1.Font.Height;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    linesPrinted = 0;
                    e.HasMorePages = false;
                }
            }
        }

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            char[] param = { '\n' };
            lines = richTextBox1.Text.Split(param);
            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
        }

        private void MenuItempageSetup_Click(object sender, EventArgs e)
        {

            PageSetupDialog.ShowDialog();
        }

        private void MenuItemPrint_Click(object sender, EventArgs e)
        {
            printDialog.ShowDialog();
        }

        private void MenuItemPrintPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog.ShowDialog();
        }

        private void MenuItemFormat_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
                richTextBox1.Font = fontDialog.Font;

        }

        private void MenuItemColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog.Color;
                Brush brush = new SolidBrush(richTextBox1.ForeColor);
            }
        }
    }
}
