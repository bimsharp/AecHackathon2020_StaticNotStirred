using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers
{
    internal static class Extension
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        MemberInfo[] memInfo = type.GetMember(type.GetEnumName(val));
                        object[] descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }


        //From Jeremy Tammik - The Building Coder - Util.cs
        public static XYZ ProjectOnto(this Plane plane, XYZ p)
        {
            double _d = SignedDistanceTo(plane, p);

            XYZ _q = p - _d * plane.Normal;

            return _q;
        }

        //From Jeremy Tammik - The Building Coder - Util.cs
        public static UV ProjectInto(this Plane plane, XYZ p)
        {
            XYZ _q = ProjectOnto(plane, p);
            XYZ _o = plane.Origin;
            XYZ _d = _q - _o;
            double _u = _d.DotProduct(plane.XVec);
            double _v = _d.DotProduct(plane.YVec);
            return new UV(_u, _v);
        }

        //From Jeremy Tammik - The Building Coder - Util.cs
        public static double SignedDistanceTo(this Plane plane, XYZ p)
        {
            XYZ _v = p - plane.Origin;

            return plane.Normal.DotProduct(_v);
        }
    }
}
