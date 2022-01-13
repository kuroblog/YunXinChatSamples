using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Desktop.Samples.Common.Behaviors
{
    public class CleanLanguageForDesignTimeBehavior : Behavior<UserControl>
    {
        public string LanguageKey
        {
            get => (string)GetValue(LanguageKeyProperty);
            set => SetValue(LanguageKeyProperty, value);
        }

        public static readonly DependencyProperty LanguageKeyProperty =
            DependencyProperty.Register(
                nameof(LanguageKey),
                typeof(string),
                typeof(CleanLanguageForDesignTimeBehavior),
                new PropertyMetadata("Languages"));

        private void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            var uiLanguageResources = AssociatedObject
                .Resources
                .MergedDictionaries?
                .ToList()?
                .Where(lang => lang.Source.OriginalString.ToLower().Contains(LanguageKey.ToLower()))?
                .ToList();

            uiLanguageResources?.ForEach(lang => AssociatedObject.Resources.MergedDictionaries?.Remove(lang));
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoadedHandler;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnLoadedHandler;
            }
        }
    }
}
