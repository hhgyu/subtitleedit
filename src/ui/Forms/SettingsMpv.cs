﻿using Nikse.SubtitleEdit.Core;
using Nikse.SubtitleEdit.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Logic.VideoPlayers;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class SettingsMpv : Form
    {

        private readonly bool _justDownload;
        private string _downloadUrl;

        public SettingsMpv(bool justDownload)
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);
            _justDownload = justDownload;
            labelPleaseWait.Text = string.Empty;
            if (Configuration.IsRunningOnLinux)
            {
                comboBoxVideoOutput.Text = Configuration.Settings.General.MpvVideoOutputLinux;
            }
            else
            {
                comboBoxVideoOutput.Text = Configuration.Settings.General.MpvVideoOutputWindows;
            }

            buttonCancel.Text = Configuration.Settings.Language.General.Cancel;
            buttonOK.Text = Configuration.Settings.Language.General.Ok;
            Text = Configuration.Settings.Language.SettingsMpv.Title;
            if (!Configuration.IsRunningOnLinux)
            {
                buttonDownload.Text = Configuration.Settings.Language.SettingsMpv.DownloadMpv;
            }

            if (Configuration.IsRunningOnLinux)
            {
                comboBoxVideoOutput.Items.Clear();
                comboBoxVideoOutput.Items.AddRange(new object[] { "vaapi", "opengl", "sdl", "vdpau" });
                Controls.Remove(buttonDownload);
            }
            UiUtil.FixLargeFonts(this, buttonOK);

            if (justDownload)
            {
                comboBoxVideoOutput.Enabled = false;
            }
        }

        private void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(Configuration.Settings.Language.SettingsMpv.DownloadMpvFailed + Environment.NewLine +
                                Environment.NewLine +
                                $"Manual action: Download \"{_downloadUrl}\" and unpack to \"{Configuration.DataDirectory}\"." + Environment.NewLine +
                                Environment.NewLine +
                                e.Error.Message);
                labelPleaseWait.Text = string.Empty;
                buttonOK.Enabled = true;
                buttonDownload.Enabled = !Configuration.IsRunningOnLinux;
                Cursor = Cursors.Default;
                return;
            }

            string dictionaryFolder = Configuration.DataDirectory;
            using (var ms = new MemoryStream(e.Result))
            using (ZipExtractor zip = ZipExtractor.Open(ms))
            {
                List<ZipExtractor.ZipFileEntry> dir = zip.ReadCentralDir();
                foreach (ZipExtractor.ZipFileEntry entry in dir)
                {
                    if (entry.FilenameInZip.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        string fileName = Path.GetFileName(entry.FilenameInZip);
                        string path = Path.Combine(dictionaryFolder, fileName);
                        
                        try
                        {
                            zip.ExtractFile(entry, path);
                        }
                        catch
                        {
                            path = Path.Combine(dictionaryFolder, fileName + ".new-mpv");
                            zip.ExtractFile(entry, path);
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
            labelPleaseWait.Text = string.Empty;
            buttonOK.Enabled = true;
            buttonDownload.Enabled = !Configuration.IsRunningOnLinux;

            MessageBox.Show(Configuration.Settings.Language.SettingsMpv.DownloadMpvOk);
            if (_justDownload)
            {
                DialogResult = DialogResult.OK;
            }
        }


        private void ButtonDownloadClick(object sender, EventArgs e)
        {
            try
            {
                labelPleaseWait.Text = Configuration.Settings.Language.General.PleaseWait;
                buttonOK.Enabled = false;
                buttonDownload.Enabled = false;
                Refresh();
                Cursor = Cursors.WaitCursor;
                _downloadUrl = "https://github.com/SubtitleEdit/support-files/blob/master/mpv/libmpv" + IntPtr.Size * 8 + ".zip?raw=true";
                var wc = new WebClient { Proxy = Utilities.GetProxy() };

                wc.DownloadDataCompleted += wc_DownloadDataCompleted;
                wc.DownloadProgressChanged += (o, args) =>
                {
                    labelPleaseWait.Text = Configuration.Settings.Language.General.PleaseWait + "  " + args.ProgressPercentage + "%";
                };
                wc.DownloadDataAsync(new Uri(_downloadUrl));
            }
            catch (Exception exception)
            {
                labelPleaseWait.Text = string.Empty;
                buttonOK.Enabled = true;
                buttonDownload.Enabled = !Configuration.IsRunningOnLinux;
                Cursor = Cursors.Default;
                MessageBox.Show(exception.Message + Environment.NewLine + Environment.NewLine + exception.StackTrace);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBoxVideoOutput.Text))
            {
                Configuration.Settings.General.MpvVideoOutputWindows = comboBoxVideoOutput.Text;
            }

            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SettingsMpv_Load(object sender, EventArgs e)
        {
            if (Configuration.IsRunningOnWindows && !LibMpvDynamic.IsInstalled)
            {
                ButtonDownloadClick(null, null);
            }
        }
    }
}
