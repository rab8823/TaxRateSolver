using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModel
{
    public class TaxRateViewModel : ViewModelBase
    {
        private decimal _rangeStart;

        /// <summary>
        /// Gets or sets RangeStart.
        /// </summary>
        /// <value>The form of data.</value>
        public decimal RangeStart
        {
            get { return _rangeStart; }
            set
            {
                if (_rangeStart != value)
                {
                    _rangeStart = value;
                    NotifyPropertyValueChanged("RangeStart");
                }
            }
        }

        private decimal _rangeEnd;

        /// <summary>
        /// Gets or sets RangeEnd.
        /// </summary>
        /// <value>The form of data.</value>
        public decimal RangeEnd
        {
            get { return _rangeEnd; }
            set
            {
                if (_rangeEnd != value)
                {
                    _rangeEnd = value;
                    NotifyPropertyValueChanged("RangeEnd");
                }
            }
        }

        private decimal _baseTaxAmount;

        /// <summary>
        /// Gets or sets BaseTaxAmount.
        /// </summary>
        /// <value>The form of data.</value>
        public decimal BaseTaxAmount
        {
            get { return _baseTaxAmount; }
            set
            {
                if (_baseTaxAmount != value)
                {
                    _baseTaxAmount = value;
                    NotifyPropertyValueChanged("BaseTaxAmount");
                }
            }
        }

        private double _marginalTaxRate;

        /// <summary>
        /// Gets or sets MarginalTaxRate.
        /// </summary>
        /// <value>The form of data.</value>
        public double MarginalTaxRate
        {
            get { return _marginalTaxRate; }
            set
            {
                if (_marginalTaxRate != value)
                {
                    _marginalTaxRate = value;
                    NotifyPropertyValueChanged("MarginalTaxRate");
                }
            }
        }

        private decimal _marginalRateFloor;

        /// <summary>
        /// Gets or sets MarginalRateFloor.
        /// </summary>
        /// <value>The form of data.</value>
        public decimal MarginalRateFloor
        {
            get { return _marginalRateFloor; }
            set
            {
                if (_marginalRateFloor != value)
                {
                    _marginalRateFloor = value;
                    NotifyPropertyValueChanged("MarginalRateFloor");
                }
            }
        }
    }
}
