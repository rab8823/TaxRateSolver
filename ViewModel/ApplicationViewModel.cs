using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {

        private ObservableCollection<TaxRateViewModel> _taxSchedule;

        /// <summary>
        /// Gets or sets TaxSchedule.
        /// </summary>
        /// <value>The form of data.</value>
        public ObservableCollection<TaxRateViewModel> TaxSchedule
        {
            get { return _taxSchedule; }
            set
            {
                if (_taxSchedule != value)
                {
                    _taxSchedule = value;
                    if (TaxReturns != null) { TaxReturns.Clear(); }
                    NotifyPropertyValueChanged("TaxSchedule");
                }
            }
        }

        private ObservableCollection<TaxReturnViewModel> _taxReturns;

        /// <summary>
        /// Gets or sets TaxReturns.
        /// </summary>
        /// <value>The form of data.</value>
        public ObservableCollection<TaxReturnViewModel> TaxReturns
        {
            get { return _taxReturns; }
            set
            {
                if (_taxReturns != value)
                {
                    _taxReturns = value;
                    NotifyPropertyValueChanged("TaxReturns");
                }
            }
        }

        private string _preTaxEarnings;

        /// <summary>
        /// Gets or sets PreTaxEarnings.
        /// </summary>
        /// <value>The form of data.</value>
        public string PreTaxEarnings
        {
            get { return _preTaxEarnings; }
            set
            {
                if (_preTaxEarnings != value)
                {
                    _preTaxEarnings = value;
                    if (!string.IsNullOrEmpty(_preTaxEarnings)) { ParseEarnings(); }
                    NotifyPropertyValueChanged("PreTaxEarnings");
                }
            }
        }

        private ObservableCollection<decimal> _earnings;

        /// <summary>
        /// Gets or sets Earnings.
        /// </summary>
        /// <value>The form of data.</value>
        public ObservableCollection<decimal> Earnings
        {
            get { return _earnings; }
            set
            {
                if (_earnings != value)
                {
                    _earnings = value;
                    CalculateAllTaxes();
                    NotifyPropertyValueChanged("Earnings");
                }
            }
        }

        public ApplicationViewModel()
        {
            // For now just default to US Federal Corporate tax rates
            TaxSchedule = new ObservableCollection<TaxRateViewModel>
            {
                new TaxRateViewModel(){ BaseTaxAmount = 0M,       MarginalTaxRate = 0.15, RangeStart = 0M,        RangeEnd = 50000M,           MarginalRateFloor = 0M },
                new TaxRateViewModel(){ BaseTaxAmount = 7500M,    MarginalTaxRate = 0.25, RangeStart = 50000M,    RangeEnd = 75000M,           MarginalRateFloor = 50000M },
                new TaxRateViewModel(){ BaseTaxAmount = 13750M,   MarginalTaxRate = 0.34, RangeStart = 75000M,    RangeEnd = 100000M,          MarginalRateFloor = 75000M },
                new TaxRateViewModel(){ BaseTaxAmount = 22250M,   MarginalTaxRate = 0.39, RangeStart = 100000M,   RangeEnd = 335000M,          MarginalRateFloor = 100000M },
                new TaxRateViewModel(){ BaseTaxAmount = 113900M,  MarginalTaxRate = 0.34, RangeStart = 335000M,   RangeEnd = 10000000M,        MarginalRateFloor = 335000M },
                new TaxRateViewModel(){ BaseTaxAmount = 3400000M, MarginalTaxRate = 0.35, RangeStart = 10000000M, RangeEnd = 15000000M,        MarginalRateFloor = 10000000M },
                new TaxRateViewModel(){ BaseTaxAmount = 5150000M, MarginalTaxRate = 0.38, RangeStart = 15000000M, RangeEnd = 18333333M,        MarginalRateFloor = 15000000M },
                new TaxRateViewModel(){ BaseTaxAmount = 0M,       MarginalTaxRate = 0.35, RangeStart = 18333333M, RangeEnd = decimal.MaxValue, MarginalRateFloor = 0M }
            };
            foreach (var item in TaxSchedule)
            {
                item.PropertyChanged += OnPropertyChanged;                
            }
            TaxReturns = new ObservableCollection<TaxReturnViewModel>();
            Earnings = new ObservableCollection<decimal>();
            Earnings.CollectionChanged += OnEarningsChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TaxRateViewModel trvm = sender as TaxRateViewModel;
            if (trvm != null) { ReCalculateTaxes(trvm); }
        }

        private void OnEarningsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        
                        CalculateTaxes((decimal)item);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        private void ParseEarnings()
        {
            foreach (var item in PreTaxEarnings.Split(' '))
            {
                decimal value;
                if (decimal.TryParse(item.Trim(), out value))
                {
                    Earnings.Add(value);
                }
            }
            PreTaxEarnings = string.Empty;
        }

        private void ReCalculateTaxes(TaxRateViewModel updatedRate)
        {
            try
            {
                List<TaxReturnViewModel> affected = new List<TaxReturnViewModel>();
                foreach (var item in TaxReturns)
                {
                    TaxRateViewModel newRate = null;
                    try
                    {
                        newRate = FindApplicableTaxRate(item.PreTaxEarnings);
                    }
                    catch (InvalidOperationException)
                    {
                        item.TaxLiability = decimal.Zero;
                        item.MarginalTaxRate = 0;
                    }
                    if (newRate == updatedRate)
                    {
                        affected.Add(item);
                    }
                }
                //var affected = from e in Earnings
                //               where FindApplicableTaxRate(e) == updatedRate
                //               let taxReturn = from r in TaxReturns
                //                               where r.PreTaxEarnings == e
                //                               select r
                //               select taxReturn;

                foreach (var item in affected)
                {
                    var newTax = CalculateTaxLiability(item.PreTaxEarnings, updatedRate);
                    item.TaxLiability = newTax;
                    item.MarginalTaxRate = updatedRate.MarginalTaxRate;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private void CalculateTaxes(decimal value)
        {
            var bracket = FindApplicableTaxRate(value);
            decimal tax = CalculateTaxLiability(value, bracket);
            TaxReturns.Add(new TaxReturnViewModel(tax, value, bracket.MarginalTaxRate));
        }

        private void CalculateAllTaxes()
        {
            if (TaxReturns != null && TaxReturns.Count > 0) { TaxReturns.Clear(); }
            foreach (var earnings in Earnings)
            {
                CalculateTaxes(earnings);
            }
        }

        private TaxRateViewModel FindApplicableTaxRate(decimal value)
        {
            TaxRateViewModel result = null;
            foreach (var taxBracket in TaxSchedule)
            {
                if (value > taxBracket.RangeStart && value <= taxBracket.RangeEnd)
                {
                    result = taxBracket;
                    break;
                }
            }
            if (result == null) { throw new InvalidOperationException("Could not find applicable tax bracket"); }
            return result;
        }

        public decimal CalculateTaxLiability(decimal pretaxEarning, TaxRateViewModel taxBracket)
        {
            return taxBracket.BaseTaxAmount + (decimal)taxBracket.MarginalTaxRate * (pretaxEarning - taxBracket.MarginalRateFloor);
        }
    }
}
