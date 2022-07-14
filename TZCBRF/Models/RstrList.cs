using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Xml.Serialization;

namespace TZCBRF.Models
{
    [XmlRoot(ElementName = "RstrList", Namespace = "")]
    public class RstrList
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ForeignKey(typeof(ParticipantInfo))]
        public int ParticipantInfoID { get; set; }

        [XmlAttribute(AttributeName = "Rstr", Namespace = "")]
        public string Rstr { get; set; }

        [XmlAttribute(AttributeName = "RstrDate", Namespace = "")]
        public DateTime RstrDate { get; set; }

        [OneToOne]
        public ParticipantInfo ParticipantInfo { get; set; }
    }
}
