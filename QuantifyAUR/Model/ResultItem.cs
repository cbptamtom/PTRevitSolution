using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantifyAUR.Model
{
    public class ResultItem
    {
        public string Alias { get; set; }

        private string _roundedSumUnitQuantity;
        public string RoundedSumUnitQuantity
        {
            get { return _roundedSumUnitQuantity; }
            set { _roundedSumUnitQuantity = value; }
        }

        private double _sumUnitQuantity;
        public double SumUnitQuantity
        {
            get { return _sumUnitQuantity; }
            set
            {
                _sumUnitQuantity = value;
                _roundedSumUnitQuantity = _sumUnitQuantity.ToString("0.000");
            }
        }
    }
}
