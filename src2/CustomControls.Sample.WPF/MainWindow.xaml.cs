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

namespace CustomControls.Sample.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonStylesPageClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new Uri("ButtonStylesPage.xaml", UriKind.Relative);
        }

        private void TextBoxStylesPageClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new Uri("TextBoxStylesPage.xaml", UriKind.Relative);
        }

        private void Button2StylesPageClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new Uri("Button2StylesPage.xaml", UriKind.Relative);
        }
    }
}
