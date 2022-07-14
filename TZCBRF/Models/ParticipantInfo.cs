using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Xml.Serialization;

namespace TZCBRF.Models
{
    [XmlRoot(ElementName = "ParticipantInfo", Namespace = "urn:cbr-ru:ed:v2.0")]
    public class ParticipantInfo
    {
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		[ForeignKey(typeof(BICDirectoryEntry))]
		public int BICDirectoryEntryID { get; set; }

        [XmlAttribute(AttributeName = "NameP", Namespace = "")]
        public string NameP { get; set; }

        [XmlAttribute(AttributeName = "CntrCd", Namespace = "")]
        public string CntrCd { get; set; }

        [XmlAttribute(AttributeName = "Rgn", Namespace = "")]
        public int Rgn { get; set; }

        [XmlAttribute(AttributeName = "Ind", Namespace = "")]
        public string Ind { get; set; }

        [XmlAttribute(AttributeName = "Tnp", Namespace = "")]
        public string Tnp { get; set; }

        [XmlAttribute(AttributeName = "Nnp", Namespace = "")]
        public string Nnp { get; set; }

        [XmlAttribute(AttributeName = "Adr", Namespace = "")]
        public string Adr { get; set; }

        [XmlAttribute(AttributeName = "DateIn", Namespace = "")]
        public string DateIn { get; set; }

        [XmlAttribute(AttributeName = "PtType", Namespace = "")]
        public string PtType { get; set; }

        [XmlAttribute(AttributeName = "Srvcs", Namespace = "")]
        public string Srvcs { get; set; }

        [XmlAttribute(AttributeName = "XchType", Namespace = "")]
        public int XchType { get; set; }

        [XmlAttribute(AttributeName = "ParticipantStatus", Namespace = "")]
        public string ParticipantStatus { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        [XmlElement(ElementName = "RstrList", Namespace = "urn:cbr-ru:ed:v2.0")]
        public RstrList RstrList { get; set; }

        [XmlAttribute(AttributeName = "RegN", Namespace = "")]
        public string RegN { get; set; }

        [OneToOne]
		public ParticipantInfo BICDirectoryEntry { get; set; }
	}
}
