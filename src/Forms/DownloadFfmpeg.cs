﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Core;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class DownloadFfmpeg : Form
    {
        public string FFmpegPath { get; internal set; }

        public DownloadFfmpeg()
        {
            InitializeComponent();
            Text = Configuration.Settings.Language.SettingsFfmpeg.Title;
        }

        private void DownloadFfmpeg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void DownloadFfmpeg_Shown(object sender, EventArgs e)
        {
            try
            {
                labelPleaseWait.Text = Configuration.Settings.Language.General.PleaseWait;
                buttonOK.Enabled = false;
                Refresh();
                Cursor = Cursors.WaitCursor;
                string url = "https://github.com/SubtitleEdit/support-files/raw/master/ffpmeg/ffmpeg-" + IntPtr.Size * 8 + ".zip";
                var wc = new WebClient { Proxy = Utilities.GetProxy() };

                wc.DownloadDataCompleted += wc_DownloadDataCompleted;
                wc.DownloadProgressChanged += (o, args) =>
                {
                    labelPleaseWait.Text = Configuration.Settings.Language.General.PleaseWait + "  " + args.ProgressPercentage + "%";
                };
                wc.DownloadDataAsync(new Uri(url));
            }
            catch (Exception exception)
            {
                labelPleaseWait.Text = string.Empty;
                buttonOK.Enabled = true;
                Cursor = Cursors.Default;
                MessageBox.Show(exception.Message + Environment.NewLine + Environment.NewLine + exception.StackTrace);
            }
        }

        private void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                labelPleaseWait.Text = string.Format(Configuration.Settings.Language.SettingsFfmpeg.XDownloadFailed, "ffmpeg");
                buttonOK.Enabled = true;
                Cursor = Cursors.Default;
                return;
            }

            string folder = Path.Combine(Configuration.DataDirectory, "ffmpeg");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            using (var ms = new MemoryStream(e.Result))
            using (ZipExtractor zip = ZipExtractor.Open(ms))
            {
                List<ZipExtractor.ZipFileEntry> dir = zip.ReadCentralDir();
                foreach (ZipExtractor.ZipFileEntry entry in dir)
                {
                    string fileName = Path.GetFileName(entry.FilenameInZip);
                    if (fileName != null)
                    {
                        string path = Path.Combine(folder, fileName);
                        if (fileName.EndsWith("ffmpeg.exe", StringComparison.OrdinalIgnoreCase))
                            FFmpegPath = path;
                        zip.ExtractFile(entry, path);
                    }
                }
            }

            Cursor = Cursors.Default;
            labelPleaseWait.Text = string.Empty;
            buttonOK.Enabled = true;
            labelPleaseWait.Text = string.Format(Configuration.Settings.Language.SettingsFfmpeg.XDownloadOk, "ffmpeg");
        }
    }
}
