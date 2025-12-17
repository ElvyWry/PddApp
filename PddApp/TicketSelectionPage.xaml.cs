using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PddApp.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Media;

namespace PddApp
{
    public sealed partial class TicketSelectionPage : Page
    {
        public TicketSelectionPage()
        {
            this.InitializeComponent();
            LoadTicketList();
        }

        private void LoadTicketList()
        {

            if (DataManager.AllTickets != null)
            {

                var ticketWrappers = DataManager.AllTickets
                    .Select(t => new TicketWrapper { Id = t.Id })
                    .ToList();


                TicketsGridView.ItemsSource = ticketWrappers;
            }
            else
            {

                TicketsGridView.Visibility = Visibility.Collapsed;

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
  
            if (Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void TicketsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is TicketWrapper selectedTicket)
            {
                if (Frame != null)
                {

                    Frame.Navigate(typeof(TicketTestPage), selectedTicket.Id);
                }
            }
        }
    }
}