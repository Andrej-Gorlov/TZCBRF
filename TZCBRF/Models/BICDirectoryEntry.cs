using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TZCBRF.Models
{
    [XmlRoot(ElementName = "BICDirectoryEntry", Namespace = "urn:cbr-ru:ed:v2.0")]
    public class BICDirectoryEntry
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        [XmlElement(ElementName = "Accounts", Namespace = "urn:cbr-ru:ed:v2.0")]
        public List<Accounts> Accounts { get; set; }

        [XmlAttribute(AttributeName = "BIC", Namespace = "urn:cbr-ru:ed:v2.0")]
        public string BIC { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        [XmlElement(ElementName = "ParticipantInfo", Namespace = "urn:cbr-ru:ed:v2.0")]
        public ParticipantInfo ParticipantInfo { get; set; }
    }
}
