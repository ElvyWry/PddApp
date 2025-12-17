using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PddApp
{
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();
        }

        private void TicketsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame != null)
            {

                Frame.Navigate(typeof(TicketSelectionPage));
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame != null)
            {

                Frame.Navigate(typeof(AboutPage));
            }
        }
    }
}