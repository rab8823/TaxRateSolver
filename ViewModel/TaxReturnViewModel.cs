using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModel
{
    public class TaxReturnViewModel : ViewModelBase
    {
        private decimal _pretaxEarnings;

        /// <summary>
        /// Gets or sets PreTaxEarnings.
        /// </summary>
        /// <value>The form of data.</value>
        public decimal PreTaxEarnings
        {
            get { return _pretaxEarnings; }
            set
            {
                if (_pretaxEarnings != value)
                {
                    _pretaxEarnings = value;
                    CalculateAverageTax();
                    NotifyPropertyValueChanged("PreTaxEarnings");
                }
            }
        }

        private decimal _taxLiability;

        /// <summary>
        /// Gets or sets TaxLiability.
        /// </summary>
        /// <value>The form of data.</value>
        public decimal TaxLiability
        {
            get { return _taxLiability; }
            set
            {
                if (_taxLiability != value)
                {
                    _taxLiability = value;
                    CalculateAverageTax();
                    NotifyPropertyValueChanged("TaxLiability");
                }
            }
        }

        private decimal _avgTaxRate;

        /// <summary>
        /// Gets or sets AverageTaxRate.
        /// </summary>
        /// <value>The form of data.</value>
        public decimal AverageTaxRate
        {
            get { return _avgTaxRate; }
            set
            {
                if (_avgTaxRate != value)
                {
                    _avgTaxRate = value;
                    NotifyPropertyValueChanged("AverageTaxRate");
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

        private TaxReturnViewModel(){}

        public TaxReturnViewModel(decimal liability, decimal preTaxEarnings, double marginalTaxRate)
        {
            PreTaxEarnings = preTaxEarnings;
            TaxLiability = liability;
            MarginalTaxRate = marginalTaxRate;
        }

        private void CalculateAverageTax()
        {
            AverageTaxRate = TaxLiability / PreTaxEarnings;
        }
    }
}
