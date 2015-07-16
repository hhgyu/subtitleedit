﻿using Nikse.SubtitleEdit.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Nikse.SubtitleEdit.Logic.SubtitleFormats
{
    public abstract class SubtitleFormat
    {
        private static IList<SubtitleFormat> _allSubtitleFormats = null;

        /// <summary>
        /// Formats supported by Subtitle Edit
        /// </summary>
        public static IList<SubtitleFormat> AllSubtitleFormats
        {
            get
            {
                if (_allSubtitleFormats != null)
                    return _allSubtitleFormats;

                _allSubtitleFormats = new List<SubtitleFormat>
                {
                    new SubRip(),
                    new AbcIViewer(),
                    new AdobeAfterEffectsFTME(),
                    new AdobeEncore(),
                    new AdobeEncoreLineTabNewLine(),
                    new AdobeEncoreTabs(),
                    new AdobeEncoreWithLineNumbers(),
                    new AdobeEncoreWithLineNumbersNtsc(),
                    new AdvancedSubStationAlpha(),
                    new AQTitle(),
                    new AvidCaption(),
                    new AvidDvd(),
                    new BelleNuitSubtitler(),
                    new CaptionAssistant(),
                    new Captionate(),
                    new CaptionateMs(),
                    new CaraokeXml(),
                    new Csv(),
                    new Csv2(),
                    new Csv3(),
                    new DCSubtitle(),
                    new DCinemaSmpte2010(),
                    new DCinemaSmpte2007(),
                    new DigiBeta(),
                    new DvdStudioPro(),
                    new DvdStudioProSpaceOne(),
                    new DvdStudioProSpace(),
                    new DvdSubtitle(),
                    new DvdSubtitleSystem(),
                    new Ebu(),
                    new Eeg708(),
                    new F4Text(),
                    new F4Rtf(),
                    new F4Xml(),
                    new FabSubtitler(),
                    new FilmEditXml(),
                    new FinalCutProXml(),
                    new FinalCutProXXml(),
                    new FinalCutProXmlGap(),
                    new FinalCutProXCM(),
                    new FinalCutProXml13(),
                    new FinalCutProXml14(),
                    new FinalCutProXml14Text(),
                    new FinalCutProTestXml(),
                    new FinalCutProTest2Xml(),
                    new FlashXml(),
                    new FLVCoreCuePoints(),
                    new Footage(),
                    new GpacTtxt(),
                    new ImageLogicAutocaption(),
                    new IssXml(),
                    new ItunesTimedText(),
                    new Json(),
                    new JsonType2(),
                    new JsonType3(),
                    new JsonType4(),
                    new JsonType5(),
                    new JsonType6(),
                    new Lrc(),
                    new MicroDvd(),
                    new MidwayInscriberCGX(),
                    new MPlayer2(),
                    new OpenDvt(),
                    new Oresme(),
                    new OresmeDocXDocument(),
                    new PE2(),
                    new PinnacleImpression(),
                    new PListCaption(),
                    new QubeMasterImport(),
                    new QuickTimeText(),
                    new RealTime(),
                    new RhozetHarmonic(),
                    new Sami(),
                    new SamiModern(),
                    new SamiYouTube(),
                    new Scenarist(),
                    new ScenaristClosedCaptions(),
                    new ScenaristClosedCaptionsDropFrame(),
                    new SmilTimesheetData(),
                    new SoftNiSub(),
                    new SoftNicolonSub(),
                    new SonyDVDArchitect(),
                    new SonyDVDArchitectExplicitDuration(),
                    new SonyDVDArchitectLineAndDuration(),
                    new SonyDVDArchitectTabs(),
                    new SonyDVDArchitectWithLineNumbers(),
                    new Spruce(),
                    new SpruceWithSpace(),
                    new StructuredTitles(),
                    new SubStationAlpha(),
                    new SubtitleEditorProject(),
                    new SubViewer10(),
                    new SubViewer20(),
                    new SwiftInterchange2(),
                    new SwiftText(),
                    new SwiftTextLineNumber(),
                    new SwiftTextLineNOAndDur(),
                    new Tek(),
                    new TimeXml(),
                    new TimeXml2(),
                    new TimedText10(),
                    new TimedText200604(),
                    new TimedText200604CData(),
                    new TimedText(),
                    new TitleExchangePro(),
                    new Titra(),
                    new TmpegEncText(),
                    new TmpegEncAW5(),
                    new TmpegEncXml(),
                    new TMPlayer(),
                    new TranscriberXml(),
                    new Tmx14(),
                    new TurboTitler(),
                    new UniversalSubtitleFormat(),
                    new UTSubtitleXml(),
                    new Utx(),
                    new UtxFrames(),
                    new UleadSubtitleFormat(),
                    new VocapiaSplit(),
                    new WebVTT(),
                    new WebVTTFileWithLineNumber(),
                    new YouTubeAnnotations(),
                    new YouTubeSbv(),
                    new YouTubeTranscript(),
                    new YouTubeTranscriptOneLine(),
                    new ZeroG(),

                    // new Idx(),
                    new UnknownSubtitle1(),
                    new UnknownSubtitle2(),
                    new UnknownSubtitle3(),
                    new UnknownSubtitle4(),
                    new UnknownSubtitle5(),
                    new UnknownSubtitle6(),
                    new UnknownSubtitle7(),
                    new UnknownSubtitle8(),
                    new UnknownSubtitle9(),
                    new UnknownSubtitle10(),
                    new UnknownSubtitle11(),
                    new UnknownSubtitle12(),
                    new UnknownSubtitle13(),
                    new UnknownSubtitle14(),
                    new UnknownSubtitle15(),
                    new UnknownSubtitle16(),
                    new UnknownSubtitle17(),
                    new UnknownSubtitle18(),
                    new UnknownSubtitle19(),
                    new UnknownSubtitle20(),
                    new UnknownSubtitle21(),
                    new UnknownSubtitle22(),
                    new UnknownSubtitle23(),
                    new UnknownSubtitle24(),
                    new UnknownSubtitle25(),
                    new UnknownSubtitle26(),
                    new UnknownSubtitle27(),
                    new UnknownSubtitle28(),
                    new UnknownSubtitle29(),
                    new UnknownSubtitle30(),
                    new UnknownSubtitle31(),
                    new UnknownSubtitle32(),
                    new UnknownSubtitle33(),
                    new UnknownSubtitle34(),
                    new UnknownSubtitle35(),
                    new UnknownSubtitle36(),
                    new UnknownSubtitle37(),
                    new UnknownSubtitle38(),
                    new UnknownSubtitle39(),
                    new UnknownSubtitle40(),
                    new UnknownSubtitle41(),
                    new UnknownSubtitle42(),
                    new UnknownSubtitle43(),
                    new UnknownSubtitle44(),
                    new UnknownSubtitle45(),
                    new UnknownSubtitle46(),
                    new UnknownSubtitle47(),
                    new UnknownSubtitle48(),
                    new UnknownSubtitle49(),
                    new UnknownSubtitle50(),
                    new UnknownSubtitle51(),
                    new UnknownSubtitle52(),
                    new UnknownSubtitle53(),
                    new UnknownSubtitle54(),
                    new UnknownSubtitle55(),
                    new UnknownSubtitle56(),
                    new UnknownSubtitle57(),
                    new UnknownSubtitle58(),
                    new UnknownSubtitle59(),
                    new UnknownSubtitle60(),
                    new UnknownSubtitle61(),
                    new UnknownSubtitle62(),
                    new UnknownSubtitle63(),
                    new UnknownSubtitle64(),
                    new UnknownSubtitle65(),
                    new UnknownSubtitle66(),
                    new UnknownSubtitle67(),
                    new UnknownSubtitle68(),
                    new UnknownSubtitle69(),
                    new UnknownSubtitle70(),
                    new UnknownSubtitle71(),
                    new UnknownSubtitle72(),
                    new UnknownSubtitle73(),
                    new UnknownSubtitle74(),
                    new UnknownSubtitle75(),
                    new UnknownSubtitle76(),
                };

                string path = Configuration.PluginsDirectory;
                if (Directory.Exists(path))
                {
                    string[] pluginFiles = Directory.GetFiles(path, "*.DLL");
                    foreach (string pluginFileName in pluginFiles)
                    {
                        try
                        {
                            var assembly = System.Reflection.Assembly.Load(FileUtil.ReadAllBytesShared(pluginFileName));
                            string objectName = Path.GetFileNameWithoutExtension(pluginFileName);
                            if (assembly != null)
                            {
                                foreach (var exportedType in assembly.GetExportedTypes())
                                {
                                    try
                                    {
                                        object pluginObject = Activator.CreateInstance(exportedType);
                                        var po = pluginObject as SubtitleFormat;
                                        if (po != null)
                                            _allSubtitleFormats.Insert(1, po);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                return _allSubtitleFormats;
            }
        }

        protected int _errorCount;

        abstract public string Extension
        {
            get;
        }

        abstract public string Name
        {
            get;
        }

        abstract public bool IsTimeBased
        {
            get;
        }

        public bool IsFrameBased
        {
            get
            {
                return !IsTimeBased;
            }
        }

        public string FriendlyName
        {
            get
            {
                return string.Format("{0} ({1})", Name, Extension);
            }
        }

        public int ErrorCount
        {
            get
            {
                return _errorCount;
            }
        }

        abstract public bool IsMine(List<string> lines, string fileName);

        abstract public string ToText(Subtitle subtitle, string title);

        abstract public void LoadSubtitle(Subtitle subtitle, List<string> lines, string fileName);

        public bool IsVobSubIndexFile
        {
            get
            {
                return string.CompareOrdinal(Extension, ".idx") == 0;
            }
        }

        public virtual void RemoveNativeFormatting(Subtitle subtitle, SubtitleFormat newFormat)
        {
        }

        public virtual List<string> AlternateExtensions
        {
            get
            {
                return new List<string>();
            }
        }

        public static int MillisecondsToFrames(double milliseconds)
        {
            return (int)Math.Round(milliseconds / (TimeCode.BaseUnit / Configuration.Settings.General.CurrentFrameRate));
        }

        public static int MillisecondsToFramesMaxFrameRate(double milliseconds)
        {
            int frames = (int)Math.Round(milliseconds / (TimeCode.BaseUnit / Configuration.Settings.General.CurrentFrameRate));
            if (frames >= Configuration.Settings.General.CurrentFrameRate)
                frames = (int)(Configuration.Settings.General.CurrentFrameRate - 0.01);
            return frames;
        }

        public static int FramesToMilliseconds(double frames)
        {
            return (int)Math.Round(frames * (TimeCode.BaseUnit / Configuration.Settings.General.CurrentFrameRate));
        }

        public static int FramesToMillisecondsMax999(double frames)
        {
            int ms = (int)Math.Round(frames * (TimeCode.BaseUnit / Configuration.Settings.General.CurrentFrameRate));
            if (ms > 999)
                ms = 999;
            return ms;
        }

        public virtual bool HasStyleSupport
        {
            get
            {
                return false;
            }
        }

        public bool BatchMode { get; set; }

        public static string ToUtf8XmlString(XmlDocument xml, bool omitXmlDeclaration = false)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = omitXmlDeclaration,
            };
            var result = new StringBuilder();

            using (var xmlWriter = XmlWriter.Create(result, settings))
            {
                xml.Save(xmlWriter);
            }

            return result.ToString().Replace(" encoding=\"utf-16\"", " encoding=\"utf-8\"").Trim();
        }

        public virtual bool IsTextBased
        {
            get
            {
                return true;
            }
        }

    }
}
