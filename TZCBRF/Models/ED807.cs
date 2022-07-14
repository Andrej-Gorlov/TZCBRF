using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TZCBRF.Models
{
    [XmlRoot(ElementName = "ED807", Namespace = "urn:cbr-ru:ed:v2.0")]
    public class ED807
    {

        [XmlElement(ElementName = "BICDirectoryEntry", Namespace = "urn:cbr-ru:ed:v2.0")]
        public List<BICDirectoryEntry> BICDirectoryEntry { get; set; }

        [XmlAttribute(AttributeName = "xmlns", Namespace = "")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "EDNo", Namespace = "")]
        public int EDNo { get; set; }

        [XmlAttribute(AttributeName = "EDDate", Namespace = "")]
        public string EDDate { get; set; }

        [XmlAttribute(AttributeName = "EDAuthor", Namespace = "")]
        public double EDAuthor { get; set; }

        [XmlAttribute(AttributeName = "CreationReason", Namespace = "")]
        public string CreationReason { get; set; }

        [XmlAttribute(AttributeName = "CreationDateTime", Namespace = "")]
        public DateTime CreationDateTime { get; set; }

        [XmlAttribute(AttributeName = "InfoTypeCode", Namespace = "")]
        public string InfoTypeCode { get; set; }

        [XmlAttribute(AttributeName = "BusinessDay", Namespace = "")]
        public DateTime BusinessDay { get; set; }

        [XmlAttribute(AttributeName = "DirectoryVersion", Namespace = "")]
        public int DirectoryVersion { get; set; }
    }
}
