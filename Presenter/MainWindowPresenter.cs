using Shaw.Srp.Model;
using Shaw.Srp.View;
using Shaw.Srp.ViewModel;
using System.Collections.Generic;
using System.Windows;

namespace Shaw.Srp.Presenter
{
    internal class MainWindowPresenter
    {
        private Window _MainWindow;

        internal MainWindowPresenter()
        {
            var plans = new List<PlanViewModel>();
            plans.Add(new PlanViewModel(new E21Plan()));
            plans.Add(new PlanViewModel(new E22Plan()));
            plans.Add(new PlanViewModel(new E23Plan()));
            plans.Add(new PlanViewModel(new E24Plan()));
            plans.Add(new PlanViewModel(new E26Plan()));
            plans.Add(new PlanViewModel(new E27PPlan()));
            plans.Add(new PlanViewModel(new E29Plan()));

            var mainWindowViewModel = new MainWindowViewModel(plans);
            _MainWindow = new MainWindow();
            _MainWindow.DataContext = mainWindowViewModel;
        }

        internal void Show()
        {
            _MainWindow.Show();
        }

    }
}
