using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_UI.Tests.Serializable
{
    [XmlInclude(typeof(BuildingLoadModel))]
    public class BuildingLoadModel
    {
        public Guid Id { get; set; }

        public List<LevelLoadModel> LevelLoadModels { get; set; }

        public double ConstructionLiveLoadWeightTotal { get; set; }
        public int LevelsAboveGroundCount { get; set; }
        public int LevelsBelowGroundCount { get; set; }
        public double FormWeightPerLinearFoot { get; set; }
        public double StructuralBeamWeightPerLinearFoot { get; set; }
        public double StructuralColumnWeightPerLinearFoot { get; set; }
        public double StructuralWallWeightPerLinearFoot { get; set; }
        public double AdditionalWeightPerLinearFoot { get; set; }

        public BuildingLoadModel()
        {
            Id = Guid.NewGuid();
            LevelLoadModels = new List<LevelLoadModel>();
        }

        internal void SerializeToXml(string filePathName)
        {
            try
            {
                // serialize and save it as xml file
                string _directory = Path.GetDirectoryName(filePathName);
                if (!Directory.Exists(_directory))
                {
                    Directory.CreateDirectory(_directory);
                }

                XmlSerializer _serializer = new XmlSerializer(typeof(BuildingLoadModel));
                using (TextWriter _writer = new StreamWriter(filePathName))
                {
                    _serializer.Serialize(_writer, this);
                }
            }
            catch (Exception _ex)
            {

            }
        }

        internal static void SerializeToXml(Models.BuildingLoadModel buildingLoadModel, string filePathName)
        {
            //ToDo: this is ridiculous.  this whole class shouldn't be needed, 
            BuildingLoadModel _clone = new BuildingLoadModel
            {

            };
        }

        internal static BuildingLoadModel DeSerializeFromXml(string filePathName)
        {
            BuildingLoadModel _settings = null;
            if (File.Exists(filePathName) == false) return null;

            try
            {
                XmlSerializer _deserializer = new XmlSerializer(typeof(BuildingLoadModel));
                TextReader _reader = new StreamReader(filePathName);
                object _obj = _deserializer.Deserialize(_reader);
                _settings = (BuildingLoadModel)_obj;
                _reader.Close();
            }
            catch (Exception _ex)
            {

            }

            return _settings;
        }

        //public XmlSchema GetSchema()
        //{
        //    return null;
        //}

        //void IXmlSerializable.ReadXml(XmlReader reader)
        //{
        //    reader.ReadStartElement("Child");
        //    string strType = reader.GetAttribute("type");
        //    XmlSerializer serial = new XmlSerializer(Type.GetType(strType));
        //    LevelLoadModels = (List<ILevelLoadModel>)serial.Deserialize(reader);
        //    reader.ReadEndElement();
        //}

        //void IXmlSerializable.WriteXml(XmlWriter writer)
        //{
        //    writer.WriteStartElement("Child");
        //    string strType = LevelLoadModels.GetType().FullName;
        //    writer.WriteAttributeString("type", strType);
        //    XmlSerializer serial = new XmlSerializer(Type.GetType(strType));
        //    serial.Serialize(writer, LevelLoadModels);
        //    writer.WriteEndElement();
        //}
    }
}