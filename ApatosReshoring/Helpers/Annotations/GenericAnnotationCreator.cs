using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Annotations
{
    internal class GenericAnnotationCreator
    {
        //ToDo: add these to Pour and Layout sheet Views
        //ShoreConditions

        //Slab Thickness
        //Clear Shore Height
        //Top of Slab Above
        //Top of Slab Current

        //public static List<GenericAnnotation> Create

        //Get floors above
        //Get X-Y squares, mapped to each floor with elevation

        //Here's my understanding of our project:
        //High-level, the plan is to automate a typically manual process of determining equipment selection, placement/pour schedule, and spacing requirements for temporary re-shoring in multi-level concrete post-tension slabs.This process can take between two weeks and several months, depending on the scope and complexity of the needed temporary re-shoring.
        //
        //We're using a bundle of technologies - BlueBeam Markups, BlueBeam Scripting XML Exports, Revit models, a custom Revit C# addin, and Power BI.
        //
        //Technical details on our approach:
        //  1) BlueBeam will provide structural Live Load and Static Load data about our building
        //  2) Revit will provide a 3D model with structural members like floors, walls & beams, which would typically be relevant for additional Static Load data
        //  3) the Revit addin will aggregate load data from BlueBeam & Revit and provide it to Power BI
        //  4) Power BI will (1) perform calculations for shore spacing, equipment selection and removal schedule, and (2) provide this data to the Revit addin
        //  5) The Revit addin will use the calculated data to (1) place Temporary Shore elements in the Revit model using the spacing and equipment selection data, and(2) generate complete deliverables including Views, Schedules, Sheets & Annotations by Concrete Pour, following the removal schedule
        //
        //Additionally, the Revit plugin will support "specs" that a user may define for alternate shoring selections to generate a true comparison of options.
    }
}
