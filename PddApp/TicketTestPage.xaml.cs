using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using PddApp.Models;

namespace PddApp
{
    public sealed partial class TicketTestPage : Page
    {
        private Ticket? CurrentTicket;
        private List<QuestionStatus> QuestionStatuses = new List<QuestionStatus>();
        private int CurrentQuestionIndex = 0;

        public TicketTestPage()
        {
            this.InitializeComponent();
        }

     
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is int ticketId)
            {
                CurrentTicket = DataManager.AllTickets?.FirstOrDefault(t => t.Id == ticketId);

                if (CurrentTicket != null)
                {
                    QuestionStatuses = CurrentTicket.Questions
                        .Select((q, index) => new QuestionStatus { QuestionId = index + 1, Question = q, Status = 0 })
                        .ToList();

                    QuestionProgressGridView.ItemsSource = QuestionStatuses;
                    DisplayQuestion(0);
                }
                else
                {
                    TicketTitle.Text = "Ошибка: Билет не найден.";
                }
            }
        }

 
        private void DisplayQuestion(int index)
        {
            if (CurrentTicket == null || index < 0 || index >= QuestionStatuses.Count) return;

            CurrentQuestionIndex = index;
            var status = QuestionStatuses[index];
            var question = status.Question;

      
            TicketTitle.Text = $"Билет {CurrentTicket.Id} | Вопрос {status.QuestionId} из {QuestionStatuses.Count}";
            QuestionText.Text = question.Text;

   
            ExplanationText.Visibility = Visibility.Collapsed;
            ExplanationText.Text = string.Empty;
            CheckButton.Visibility = Visibility.Visible;
            SkipButton.Visibility = Visibility.Visible;
            FinishButton.Visibility = Visibility.Visible;

           
            OptionsPanel.Children.Clear();
            if (question.Type == "Matching" && question.MatchingSetup != null)
            {
                TicketTitle.Text = $"Билет {CurrentTicket.Id} | Вопрос {status.QuestionId} (Соответствие)";

                var matchingGrid = new Grid
                {
                    Margin = new Thickness(0, 0, 0, 20)
                };

                matchingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                matchingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                matchingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); 

                int rowCount = question.MatchingSetup.LeftColumnImages.Count;

                int rightCount = question.MatchingSetup.RightColumnText.Count;

                for (int i = 0; i < rowCount; i++)
                {
                    matchingGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var numberBlock = new TextBlock
                    {
                        Text = $"{i + 1}.",
                        Margin = new Thickness(5),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        FontWeight = Microsoft.UI.Text.FontWeights.Bold
                    };
                    Grid.SetColumn(numberBlock, 0);
                    Grid.SetRow(numberBlock, i);
                    matchingGrid.Children.Add(numberBlock);


                    var leftImage = new Image
                    {
                        Source = new BitmapImage(new Uri($"ms-appx:///{question.MatchingSetup.LeftColumnImages[i]}")),
                        MaxHeight = 100,
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };
                    Grid.SetColumn(leftImage, 1);
                    Grid.SetRow(leftImage, i);
                    matchingGrid.Children.Add(leftImage);

                    if (i < rightCount)
                    {
                        string content = question.MatchingSetup.RightColumnText[i];


                        FrameworkElement rightElement;


                        if (IsImagePath(content))
                        {

                            rightElement = new Image
                            {
                                Source = new BitmapImage(new Uri($"ms-appx:///{content}")),
                                MaxHeight = 100,
                                Stretch = Stretch.Uniform,
                                Margin = new Thickness(10, 5, 0, 5),
                                HorizontalAlignment = HorizontalAlignment.Left
                            };
                        }
                        else
                        {

                            rightElement = new TextBlock
                            {
                                Text = content,
                                Margin = new Thickness(10, 5, 0, 5),
                                VerticalAlignment = VerticalAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                FontSize = 16,
                                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold
                            };
                        }


                        Grid.SetColumn(rightElement, 2);
                        Grid.SetRow(rightElement, i);
                        matchingGrid.Children.Add(rightElement);
                    }
                }

                OptionsPanel.Children.Add(matchingGrid);
                QuestionImage.Visibility = Visibility.Collapsed;
            }


            if (question.Options != null)
            {
                foreach (var option in question.Options)
                {
                    var rb = new RadioButton
                    {
                        Tag = option.Id, 
                        Margin = new Thickness(0, 5, 0, 5)
                    };


                    if (IsImagePath(option.Text))
                    {

                        var optionImage = new Image
                        {
                            Source = new BitmapImage(new Uri($"ms-appx:///{option.Text}")),
                            MaxHeight = 150, 
                            Stretch = Stretch.Uniform,
                            HorizontalAlignment = HorizontalAlignment.Left
                        };
                        rb.Content = optionImage;
                    }
                    else
                    {

                        rb.Content = new TextBlock
                        {
                            Text = option.Text,
                            TextWrapping = TextWrapping.Wrap,
                            FontSize = 16
                        };
                    }

                    OptionsPanel.Children.Add(rb);
                }
            }


            if (question.Type != "Matching")
            {
                LoadQuestionImage(question.ImagePath);
            }

  
            if (status.Status != 0)
            {
                ShowAnswerResult(status);
            }
            else
            {

                CheckButton.Visibility = Visibility.Visible;
            }


            QuestionProgressGridView.ItemsSource = null;
            QuestionProgressGridView.ItemsSource = QuestionStatuses;
        }

        private void LoadQuestionImage(string? imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    Uri uri = new Uri($"ms-appx:///{imagePath}");
                    QuestionImage.Source = new BitmapImage(uri);
                    QuestionImage.Visibility = Visibility.Visible;
                }
                catch
                {
                    QuestionImage.Source = null;
                    QuestionImage.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                QuestionImage.Source = null;
                QuestionImage.Visibility = Visibility.Collapsed;
            }
        }


        private void ShowAnswerResult(QuestionStatus status)
        {

            ExplanationText.Text = status.Status == 1
                ? $"Правильно! {status.Question.Explanation}"
                : $"Ошибка! Правильный ответ - {status.Question.CorrectAnswer}. {status.Question.Explanation}";


            ExplanationText.Foreground = status.Status == 1
                ? new SolidColorBrush(Colors.Green)
                : new SolidColorBrush(Colors.Red);

            ExplanationText.Visibility = Visibility.Visible;
            CheckButton.Visibility = Visibility.Collapsed;
            SkipButton.Visibility = Visibility.Collapsed;
            FinishButton.Visibility = (QuestionStatuses.Count(s => s.Status != 0) == QuestionStatuses.Count)
                ? Visibility.Visible
                : Visibility.Collapsed;


            foreach (var child in OptionsPanel.Children.OfType<RadioButton>())
            {
                child.IsEnabled = false;


                if (status.Question.CorrectAnswer != null)
                {

                    int correctAnswerId = int.Parse(status.Question.CorrectAnswer.ToString()!);


                    if ((int)child.Tag == correctAnswerId)
                    {
                        child.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
                        child.Foreground = new SolidColorBrush(Colors.Green);
                    }
                }
            }
        }


        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            var status = QuestionStatuses[CurrentQuestionIndex];


            RadioButton? selectedRb = OptionsPanel.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);

            if (selectedRb == null)
            {
    
                ExplanationText.Text = "Пожалуйста, выберите один из вариантов ответа.";
                ExplanationText.Foreground = new SolidColorBrush(Colors.Orange);
                ExplanationText.Visibility = Visibility.Visible;
                return;
            }

            ExplanationText.Visibility = Visibility.Collapsed; 


            int selectedAnswerId = (int)selectedRb.Tag;

            bool isCorrect = selectedAnswerId.ToString() == status.Question.CorrectAnswer.ToString();

            status.Status = isCorrect ? 1 : 2; 

            ShowAnswerResult(status);

            if (CurrentQuestionIndex < QuestionStatuses.Count - 1)
            {
              
                DisplayQuestion(CurrentQuestionIndex + 1);
            }
            else
            {
                FinishButton.Visibility = Visibility.Visible;
            }
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            var status = QuestionStatuses[CurrentQuestionIndex];
            status.Status = 3; 


            if (CurrentQuestionIndex < QuestionStatuses.Count - 1)
            {
                DisplayQuestion(CurrentQuestionIndex + 1);
            }
            else
            {

                ShowAnswerResult(status);
                FinishButton.Visibility = Visibility.Visible;
            }
        }


        private void QuestionProgressGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is QuestionStatus selectedStatus)
            {
                DisplayQuestion(selectedStatus.QuestionId - 1); 
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame != null)
            {
                Frame.Navigate(typeof(ResultPage), QuestionStatuses);
            }
        }
        private bool IsImagePath(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;


            return text.EndsWith(".png", StringComparison.OrdinalIgnoreCase);

        }
    }
}