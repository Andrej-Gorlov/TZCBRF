using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using TZCBRF.Models;

namespace TZCBRF.ViewModel
{
    public class BICViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private int bic;
        public int BIC
        {
            get { return bic; }
            set
            {
                bic = value;
                NotifyPropertyChanged("BIC");
            }
        }

        private string nameP;
        public string NameP 
        {
            get { return nameP; }
            set
            {
                nameP = value;
                NotifyPropertyChanged("NameP");
            }
        }
        private string cntrCd;
        public string CntrCd
        {
            get { return cntrCd; }
            set
            {
                cntrCd = value;
                NotifyPropertyChanged("CntrCd");
            }
        }
        private int rgn;
        public int Rgn
        {
            get { return rgn; }
            set
            {
                rgn = value;
                NotifyPropertyChanged("Rgn");
            }
        }
        private string ind;
        public string Ind
        {
            get { return ind; }
            set
            {
                ind = value;
                NotifyPropertyChanged("Ind");
            }
        }
        private string tnp;
        public string Tnp
        {
            get { return tnp; }
            set
            {
                tnp = value;
                NotifyPropertyChanged("Tnp");
            }
        }
        private string nnp;
        public string Nnp
        {
            get { return nnp; }
            set
            {
                nnp = value;
                NotifyPropertyChanged("Nnp");
            }
        }
        private string adr;
        public string Adr
        {
            get { return adr; }
            set
            {
                adr = value;
                NotifyPropertyChanged("dr");
            }
        }
        private string dateIn;
        public string DateIn
        {
            get { return dateIn; }
            set
            {
                dateIn = value;
                NotifyPropertyChanged("DateIn");
            }
        }
        private string ptType;
        public string PtType
        {
            get { return ptType; }
            set
            {
                ptType = value;
                NotifyPropertyChanged("PtType");
            }
        }
        private string srvcs;
        public string Srvcs
        {
            get { return srvcs; }
            set
            {
                srvcs = value;
                NotifyPropertyChanged("Srvcs");
            }
        }
        private int xchType;
        public int XchType
        {
            get { return xchType; }
            set
            {
                xchType = value;
                NotifyPropertyChanged("XchType");
            }
        }
        private string participantStatus;
        public string ParticipantStatus
        {
            get { return participantStatus; }
            set
            {
                participantStatus = value;
                NotifyPropertyChanged("ParticipantStatus");
            }
        }
        private string regN;
        public string RegN
        {
            get { return regN; }
            set
            {
                regN = value;
                NotifyPropertyChanged("RegN");
            }
        }
        private List<ParticipantInfo> participantInfo;
        public List<ParticipantInfo> ParticipantInfo
        {
            get { return participantInfo; }
            set
            {
                participantInfo = value;
                NotifyPropertyChanged("ParticipantInfo");
            }
        }
        public ICommand cmdAddBICDirectoryEntry { get; private set; }
        public bool CanExectute
        {
            get { return !string.IsNullOrEmpty(BIC.ToString()); }
        }
        public BICViewModel()
        {
            LoadingBIC();
        }

        private async void LoadingBIC()
        {
            MemoryStream zipfile = DownloadBICFile();
            string xmlPath = Path.Combine(Path.GetTempPath(), "ed807.xml");
            using (ZipArchive zip = new ZipArchive(zipfile))
            {
                zip.Entries.First().ExtractToFile(xmlPath, true);
            }

            Validation(xmlPath);

            XmlSerializer serializer = new XmlSerializer(typeof(ED807));
            XmlReaderSettings settings = new XmlReaderSettings();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var xml = File.ReadAllText(xmlPath, Encoding.GetEncoding("windows-1251"));

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    var obj = (ED807)serializer.Deserialize(xmlReader);

                    foreach (var item in obj.BICDirectoryEntry)
                    {
                        await App.Database.BICDirectoryEntrySaveTaskAsync(item);
                        var fid = await App.Database.GetBICDirectoryEntryAsync();

                        foreach (var account in item.Accounts)
                        {
                            Accounts ac = new();
                            ac = account;
                            ac.BICDirectoryEntryID = fid.LastOrDefault().ID;
                            await App.Database.AccountsSaveTaskAsync(ac);
                        }

                        ParticipantInfo participant = new();
                        participant = item.ParticipantInfo;
                        participant.BICDirectoryEntryID = fid.LastOrDefault().ID;
                        await App.Database.ParticipantInfoSaveTaskAsync(participant);

                        if (item.ParticipantInfo.RstrList != null)
                        {
                            RstrList rstr = new RstrList();
                            rstr = item.ParticipantInfo.RstrList;
                            rstr.ParticipantInfoID = fid.LastOrDefault().ID;
                            await App.Database.RstrListSaveTaskAsync(rstr);
                        }
                    }
                }
            }
            GetBICDirectoryEntry();
        }
        private MemoryStream DownloadBICFile()
        {
            for (int shiftDays = 0; true; ++shiftDays)
            {
                try
                {
                    return LoadFile(string.Format("http://www.cbr.ru/VFS/mcirabis/BIKNew/{0:yyyyMMdd}ED01OSBR.zip", DateTime.Today.AddDays(-shiftDays)));
                }
                catch (System.Net.WebException webEx)
                {
                    if (shiftDays >= 10)
                        throw;
                    if ((webEx.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                        continue;
                }
            }
        }
        private MemoryStream LoadFile(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = WebRequest.DefaultWebProxy;
            request.UseDefaultCredentials = true;
            request.CookieContainer = new CookieContainer();
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                string location = response.Headers.Get("Location");
                if (location != null)
                {
                    Uri u = location[0] == '/' ? new Uri(request.RequestUri, location) : new Uri(location);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(u);
                    req.Proxy = request.Proxy;
                    req.UseDefaultCredentials = request.UseDefaultCredentials;
                    req.AllowAutoRedirect = request.AllowAutoRedirect;
                    req.CookieContainer = new CookieContainer();
                    req.CookieContainer.Add(response.Cookies);
                    response = (HttpWebResponse)req.GetResponse();
                    if (response.Headers.Get("Location") != null)
                        throw new Exception("Twice redirect");
                }
                var stm = response.GetResponseStream();
                MemoryStream mem = new MemoryStream();
                byte[] buffer = new byte[1024 * 512];
                int read = stm.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    mem.Write(buffer, 0, read);
                    read = stm.Read(buffer, 0, buffer.Length);
                }
                return mem;
            }
            finally
            {
                response.Close();
            }
        }
        private static void Validation(string xmlPath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var path = "C:\\xsdvalidation\\xsdvalidation";

            XmlSchemaSet schema = new XmlSchemaSet();

            schema.Add("urn:cbr-ru:ed:basetypes:v2.0", path + "\\cbr_ed_basetypes_v2018.3.0.xsd");
            schema.Add("urn:cbr-ru:ed:leaftypes:v2.0", path + "\\cbr_ed_leaftypes_v2022.2.0.xsd");
            schema.Add("urn:cbr-ru:ed:v2.0", path + "\\cbr_ed_objects_v2022.2.1.xsd");
            schema.Add("urn:cbr-ru:ed:v2.0", path + "\\cbr_ed807_v2022.2.1.xsd");

            XmlReader rd = XmlReader.Create(xmlPath);
            XDocument doc = XDocument.Load(rd);
            doc.Validate(schema, ValidationEventHandler);
        }
        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        public async void GetBICDirectoryEntry()
        {
            ParticipantInfo = await App.Database.GetParticipantInfoAsync();
        }
    }
}
