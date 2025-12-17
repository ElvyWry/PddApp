using Microsoft.UI.Xaml;
using PddApp.Services;

namespace PddApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            Title = "Приложение для тестирования ПДД";

            var tickets = DataService.LoadTicketsFromFile("tickets.json");


            DataManager.AllTickets = tickets;


            MainFrame.Navigate(typeof(WelcomePage));
        }
    }
}