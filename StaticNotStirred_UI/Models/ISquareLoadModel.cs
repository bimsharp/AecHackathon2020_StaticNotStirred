using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Models
{
    public interface ISquareLoadModel
    {
        Guid Id { get; set; }

        string Source { get; set; }

        string Name { get; set; }

        double AmountPerSquareFoot { get; set; }

        double MinX { get; set; }

        double MinY { get; set; }

        double MaxX { get; set; }

        double MaxY { get; set; }

    }
}
