﻿using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace CarbonAnalysis.Library
{
    public class DistinctCompare : IEqualityComparer<Element>
    {
        public bool Equals(Element x, Element y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Element obj)
        {
            return 1;
        }
    }
}
