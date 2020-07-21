using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_UI.Tests.Models
{
    [XmlInclude(typeof(SquareLoadModel))]
    public class SquareLoadModel : ISquareLoadModel
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
        public double AmountPerSquareFoot { get; set; }
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }

        public SquareLoadModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
