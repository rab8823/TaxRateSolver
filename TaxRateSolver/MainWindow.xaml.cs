using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaxRateSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BindingExpression _bindingExpression;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            inputs.Focus();
            Loaded -= OnLoaded;
        }

        private void OnEnterNewEarningsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (_bindingExpression != null)
                {
                    _bindingExpression.UpdateSource();
                }
                else
                {
                    TextBox tb = sender as TextBox;
                    if (tb != null)
                    {
                        _bindingExpression = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
                        if (_bindingExpression != null)
                        {
                            _bindingExpression.UpdateSource();
                        }
                    }
                }
            }
        }
    }
}
