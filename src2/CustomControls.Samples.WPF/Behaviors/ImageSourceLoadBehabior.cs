using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;

namespace CustomControls.Samples.WPF.Behaviors
{
    public class ImageSourceLoadBehabior : Behavior<Image>
    {
        public string ImageUriSource
        {
            get { return (string)GetValue(ImageUriSourceProperty); }
            set { SetValue(ImageUriSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageUriSourceProperty =
            DependencyProperty.Register(nameof(ImageUriSource), typeof(string), typeof(ImageSourceLoadBehabior), new PropertyMetadata(null));

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImageUriSource))
            {
                AssociatedObject.Source = null;
            }
            else
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(ImageUriSource, UriKind.Absolute);
                bitmapImage.EndInit();

                AssociatedObject.Source = bitmapImage;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnLoaded;
            }
        }
    }
}
