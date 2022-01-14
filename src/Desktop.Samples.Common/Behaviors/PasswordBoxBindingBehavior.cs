using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Desktop.Samples.Common.Behaviors
{
    public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
    {
        public string LoginPass
        {
            get => (string)GetValue(LoginPassProperty);
            set => SetValue(LoginPassProperty, value);
        }
        
        public static readonly DependencyProperty LoginPassProperty =
            DependencyProperty.Register(
                nameof(LoginPass),
                typeof(string),
                typeof(PasswordBoxBindingBehavior),
                new PropertyMetadata((o, e) =>
                {
                    void SetSelection(PasswordBox passwordBox, int start, int length)
                    {
                        passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { start, length });
                    }

                    if (o is PasswordBoxBindingBehavior dp)
                    {
                        dp.AssociatedObject.Password = e.NewValue.ToString();
                        SetSelection(dp.AssociatedObject, dp.AssociatedObject.Password.Length, 0);
                    }
                }));

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginPass = AssociatedObject.Password;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.PasswordChanged -= OnPasswordChanged;
            }
        }
    }
}
