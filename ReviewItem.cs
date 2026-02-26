using System;
using System.Collections.Generic;
using System.Text;
namespace QuizApp
{
    public class ReviewItem
    {
        public string QuestionNumber { get; set; }
        public string Question { get; set; }
        public string UserAnswerLetter { get; set; }
        public string UserAnswerText { get; set; }
        public string CorrectLetter { get; set; }
        public string CorrectText { get; set; }
        public Color UserColor { get; set; }
    }
}