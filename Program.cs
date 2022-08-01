using Shaw.Srp.Presenter;
using System;
using System.Windows;

namespace Shaw.Srp
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application();
            app.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var mainWindowPresenter = new MainWindowPresenter();
            mainWindowPresenter.Show();

            app.Run(app.MainWindow);
        }
    }
}
