using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Xml.Serialization;

namespace TZCBRF.Models
{
	[XmlRoot(ElementName = "Accounts", Namespace = "urn:cbr-ru:ed:v2.0")]
	public class Accounts
    {
		[PrimaryKey, AutoIncrement]
		public int AccountID { get; set; }

		[ForeignKey(typeof(BICDirectoryEntry))]
		public int BICDirectoryEntryID { get; set; }

        [XmlAttribute(AttributeName = "Account", Namespace = "")]
        public string Account { get; set; }

        [XmlAttribute(AttributeName = "RegulationAccountType", Namespace = "")]
        public string RegulationAccountType { get; set; }

        [XmlAttribute(AttributeName = "CK", Namespace = "")]
        public string CK { get; set; }

        [XmlAttribute(AttributeName = "AccountCBRBIC", Namespace = "")]
        public string AccountCBRBIC { get; set; }

        [XmlAttribute(AttributeName = "DateIn", Namespace = "")]
        public string DateIn { get; set; }

        [XmlAttribute(AttributeName = "AccountStatus", Namespace = "")]
        public string AccountStatus { get; set; }

        [ManyToOne]  
		public BICDirectoryEntry BICDirectoryEntry { get; set; }
	}
}
