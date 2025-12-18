using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace PddApp
{
    public sealed partial class ResultPage : Page
    {
        public ResultPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is List<QuestionStatus> finalStatuses)
            {
                DisplayResults(finalStatuses);
            }
        }

        private void DisplayResults(List<QuestionStatus> statuses)
        {
            int total = statuses.Count;
            int correct = statuses.Count(s => s.Status == 1);
            int incorrect = statuses.Count(s => s.Status == 2);
            int skipped = statuses.Count(s => s.Status == 3);

            TotalQuestionsText.Text = $"Всего вопросов: {total}";
            CorrectText.Text = $"Правильных ответов: {correct}";
            IncorrectText.Text = $"Неправильных ответов: {incorrect}";
            SkippedText.Text = $"Пропущено: {skipped}";
            if (correct >= 7)
            {
                FinalResultText.Text = "Тест СДАН!";
                FinalResultText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
            }
            else
            {
                FinalResultText.Text = "Тест НЕ СДАН!";
                FinalResultText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

            if (Frame != null)
            {

                Frame.Navigate(typeof(WelcomePage));


                Frame.BackStack.Clear();
            }
        }
    }
}