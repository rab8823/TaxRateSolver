using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ViewModel;

namespace TaxRateSolver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mw = new MainWindow() { DataContext = new ApplicationViewModel() };
            mw.Show();
            base.OnStartup(e);
        }
    }
}
