﻿using Nikse.SubtitleEdit.Core;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Core.Common;

namespace Nikse.SubtitleEdit.Forms.Ocr
{
    public sealed partial class DownloadTesseract4 : Form
    {
        public const string TesseractDownloadUrl = "https://github.com/SubtitleEdit/support-files/raw/master/Tesseract500.Alpha.20201127.tar.gz";

        public DownloadTesseract4(string version)
        {
            InitializeComponent();
            Text = Configuration.Settings.Language.GetTesseractDictionaries.Download + " Tesseract " + version;
            labelPleaseWait.Text = Configuration.Settings.Language.General.PleaseWait;
            labelDescription1.Text = Configuration.Settings.Language.GetTesseractDictionaries.Download + " Tesseract OCR";

            var wc = new WebClient { Proxy = Utilities.GetProxy() };
            wc.DownloadDataAsync(new Uri(TesseractDownloadUrl));

            wc.DownloadDataCompleted += wc_DownloadDataCompleted;
            wc.DownloadProgressChanged += (o, args) =>
            {
                labelPleaseWait.Text = Configuration.Settings.Language.General.PleaseWait + "  " + args.ProgressPercentage + "%";
            };
        }

        private void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(Configuration.Settings.Language.GetTesseractDictionaries.DownloadFailed + Environment.NewLine +
                                $"Please download {TesseractDownloadUrl} manually and unpack into this folder: \"{Configuration.TesseractDirectory}\"" + Environment.NewLine +
                                Environment.NewLine +
                                e.Error.Message + ": " + e.Error.StackTrace);
                DialogResult = DialogResult.Cancel;
                return;
            }

            string dictionaryFolder = Configuration.TesseractDirectory;
            if (!Directory.Exists(dictionaryFolder))
            {
                Directory.CreateDirectory(dictionaryFolder);
            }

            var tempFileName = FileUtil.GetTempFileName(".tar");
            using (var ms = new MemoryStream(e.Result))
            using (var fs = new FileStream(tempFileName, FileMode.Create))
            using (var zip = new GZipStream(ms, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[1024];
                int nRead;
                while ((nRead = zip.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, nRead);
                }
            }

            using (var tr = new TarReader(tempFileName))
            {
                foreach (var th in tr.Files)
                {
                    string fn = Path.Combine(dictionaryFolder, th.FileName.Replace('/', Path.DirectorySeparatorChar));
                    if (th.IsFolder)
                    {
                        Directory.CreateDirectory(Path.Combine(dictionaryFolder, th.FileName.Replace('/', Path.DirectorySeparatorChar)));
                    }
                    else if (th.FileSizeInBytes > 0)
                    {
                        th.WriteData(fn);
                    }
                }
            }
            File.Delete(tempFileName);
            Cursor = Cursors.Default;
            DialogResult = DialogResult.OK;
        }
    }
}
