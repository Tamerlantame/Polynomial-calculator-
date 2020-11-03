﻿using System;
using System.IO;
using System.Windows.Forms;
using ConsoleUI.Forms;
using WinFormsUI.Sessions;

namespace WinFormsUI.Forms
{

    public partial class UIFormWithTabs : Form

    {
        public UIFormWithTabs()
        {
            InitializeComponent();
            CreateGraphSession();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxOutput.Text = "";
            string[] text = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Menu//GraphHelp.txt"));
            foreach (string item in text)
            {
                richTextBoxOutput.Text = richTextBoxOutput.Text + item + "\n";
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGraphSession();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LyaMelikTabPage currentPage = (LyaMelikTabPage)tabControl1.SelectedTab;

            currentPage.CurrentSession.SaveSession();
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void CreateGraphSession()
        {
            string title = "GraphSession " + (tabControl1.TabCount + 1).ToString();

            RichTextBox newRichTextBox = new RichTextBox
            {
                Location = new System.Drawing.Point(0, 0),
                Name = title,
                Size = richTextBox1.Size,
                Text = ""
            };
            GraphSession newSession = new GraphSession(newRichTextBox, richTextBoxOutput);
            LyaMelikTabPage newTabPage = new LyaMelikTabPage(newSession)
            {
                Name = title
            };
            tabControl1.TabPages.Add(newTabPage);
            tabControl1.SelectTab(newTabPage);
            newTabPage.Controls.Add(newRichTextBox);

            newSession.Start();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
               string sessionText =File.ReadAllText(dialog.FileName);
                LyaMelikTabPage currentPage = (LyaMelikTabPage)tabControl1.SelectedTab;
                currentPage.CurrentSession.SetInputBoxText(sessionText);
            }
        }
    }
}
