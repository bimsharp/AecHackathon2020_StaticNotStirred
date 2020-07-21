using Autodesk.Revit.Creation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StaticNotStirred_Revit.Helpers
{
    public class CalculationOutputs
    {
        //ToDo: calcs are per change in condition change - like concreteThickness/beams/verticalSurfaces, assume 153 PCF for concrete density

        //http://dl.mycivil.ir/dozanani/ACI/ACI%20347.2R-05%20Guide%20for%20Shoring-Reshoring%20of%20Concrete%20Multistory%20Buildings_MyCivil.ir.pdf

        //Design Criteria
        //Static Loads
        //Live Loads

        //public int Id { get; set; }
        //public double Weight { get; set; }          //lbf
        //public double Capacity { get; set; }        //lbf
        //public double Height { get; set; }          //decimal feet
        //public double FormworkLoad { get; set; }    //linear ft (lbs per foot)

        public Guid Id { get; set; }
        public double ConcreteDensity { get; set; }                 //Pcf
        public string ConcreteDensityUnits { get; set; }

        public double ConstructionLiveLoad { get; set; }            //Psf
        public string ConstructionLiveLoadUnits { get; set; }

        public double FormworkWeight { get; set; }                  //Psf
        public string FormworkWeightUnits { get; set; }

        public double BeamFormwork { get; set; }                    //Psf
        public string BeamFormworkUnits { get; set; }

        public double BeamFormworkAdditionalLength { get; set; }    //Psf
        public string BeamFormworkAdditionalLengthUnits { get; set; }

        public double WallFormwork { get; set; }                    //Psf
        public string WallFormworkUnits { get; set; }

        public double DaysToClosurePourPourBack { get; set; }       //days
        public string DaysToClosurePourPourBackUnits { get; set; }

        public double DaysToFormworkInstall { get; set; }           //days
        public string DaysToFormworkInstallUnits { get; set; }

        //ToDo: from link in schedule
        public double ClearShoreHeight { get; set; }                //ft   
        public string ClearShoreHeightUnits { get; set; }

        //public LoadReductions                                     //

        //LoadReductionPercent                                      //%      //                    v	60	    %	reduction factor of design live loads
        //AccessFloorSystemsOfficeUniformLoad                       //Psf    //                    v	50	    psf Uniform load
        //AccessFloorSystemsOfficeConcentratedLoad                  //lbs    //                    v	2000    lbs Concentrated load
        //AccessFloorSystemsUniformLoad                             //Psf    //    Computer use    v	100	    psf Uniform load
        //AccessFloorSystemsConcentratedLoad                        //lbs    //    Computer use    v	2000    lbs Concentrated load
        //ArmoriesAndDrillRoomsUniformLoad                          //Psf    //                    v	150	    psf Uniform load
        //AssemblyAreasUniformLoad                                  //Psf    //                    v	100	    psf Uniform load

        public CalculationOutputs()
        {
            Id = Guid.NewGuid();
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

                XmlSerializer _serializer = new XmlSerializer(typeof(CalculationOutputs));
                using (TextWriter _writer = new StreamWriter(filePathName))
                {
                    _serializer.Serialize(_writer, this);
                }
            }
            catch (Exception _ex)
            {

            }
        }

        internal static CalculationOutputs DeSerializeFromXml(string filePathName)
        {
            CalculationOutputs _settings = null;
            if (File.Exists(filePathName) == false) return null;

            try
            {
                XmlSerializer _deserializer = new XmlSerializer(typeof(CalculationOutputs));
                TextReader _reader = new StreamReader(filePathName);
                object _obj = _deserializer.Deserialize(_reader);
                _settings = (CalculationOutputs)_obj;
                _reader.Close();
            }
            catch (Exception _ex)
            {

            }

            return _settings;
        }




        //formulae

        //reference for specific ID - to be able to tie a structure to a "code" - phase, QR code, "by pour"

        //workflow is more valuable than interface right now

        // //ToDo: generate 2D plans that would go to field
    }
}
