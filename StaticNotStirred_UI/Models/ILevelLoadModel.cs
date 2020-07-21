using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_UI.Models
{
    public interface ILevelLoadModel
    {
        List<ISquareLoadModel> CapacityModels { get; set; }
        List<ISquareLoadModel> DemandModels { get; set; }
        List<ISquareLoadModel> ReshoreDemandModels { get; set; }

        Guid Id { get; set; }

        string Name { get; set; }

        double Elevation { get; set; }

        double TopOfSlabElevation { get; set; }

        double ConcreteDepth { get; set; }

        double Capacity { get; set; }

        double Demand { get; set; }

        double ReshoreDemand { get; set; }

        //public XmlSchema GetSchema()
        //{
        //    return null;
        //}
        //
        //void IXmlSerializable.ReadXml(XmlReader reader)
        //{
        //    reader.ReadStartElement("Child");
        //
        //    string strType = reader.GetAttribute("type");
        //    XmlSerializer serial = new XmlSerializer(Type.GetType(strType));
        //    CapacityModels = (List<ISquareLoadModel>)serial.Deserialize(reader);
        //
        //    //string strType = reader.GetAttribute("type");
        //    //XmlSerializer serial = new XmlSerializer(Type.GetType(strType));
        //    DemandModels = (List<ISquareLoadModel>)serial.Deserialize(reader);
        //
        //    //string strType = reader.GetAttribute("type");
        //    //XmlSerializer serial = new XmlSerializer(Type.GetType(strType));
        //    ReshoreDemandModels = (List<ISquareLoadModel>)serial.Deserialize(reader);
        //
        //    reader.ReadEndElement();
        //}
        //
        //void IXmlSerializable.WriteXml(XmlWriter writer)
        //{
        //    writer.WriteStartElement("Child");
        //    string strType = CapacityModels.GetType().FullName;
        //    writer.WriteAttributeString("type", strType);
        //    XmlSerializer serial = new XmlSerializer(Type.GetType(strType));
        //    serial.Serialize(writer, CapacityModels);
        //    writer.WriteEndElement();
        //}
    }
}
