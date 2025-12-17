using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

using PddApp.Models;

namespace PddApp
{

    public class QuestionStatus
    {
        public int QuestionId { get; set; } 
        public Question Question { get; set; } 

   
        public int Status { get; set; } = 0;

 
        public SolidColorBrush Color
        {
            get
            {
                return Status switch
                {
                    1 => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 128, 0)), 
                    2 => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), 
                    3 => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 211, 211, 211)), 
                    _ => new SolidColorBrush(Windows.UI.Color.FromArgb(255, 211, 211, 211)), 
                };
            }
        }
    }
}