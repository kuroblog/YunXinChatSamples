using Desktop.Samples.Shell.ViewModels;
using System;
using System.Windows;

namespace Desktop.Samples.Shell.Views
{
    public partial class ShellView : Window
    {
        public ShellView(ShellViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
