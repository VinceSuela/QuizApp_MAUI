namespace QuizApp;

public partial class MainPage : ContentPage
{
	IDispatcherTimer timer;
	TimeSpan timeRemaining = TimeSpan.FromSeconds(30);
	int index;

	Questions questions = new Questions();
	readonly List<QuizItem> quizItems = new List<QuizItem>();

	public MainPage()
	{
		InitializeComponent();

		subjectSelection.IsVisible = true;
		quizCard.IsVisible = false;
		btnStartQuiz.IsVisible = true;
	}

	public void showText()
	{
		var currentQuiz = quizItems[index];
		lblQuestions.Text = $"{index + 1}. {currentQuiz.Question}";

		btnA.Text = "A. " + currentQuiz.Options.A;
		btnB.Text = "B. " + currentQuiz.Options.B;
		btnC.Text = "C. " + currentQuiz.Options.C;
		btnD.Text = "D. " + currentQuiz.Options.D;

		quizProgress.Progress = (double)(index + 1) / quizItems.Count;
	}

	private async void RecordAnswerAndAdvance(string answer, Button clickedButton)
	{
		questions.userAnswerItems[index].Answer = answer;

		if (clickedButton != null && clickedButton.Handler != null)
		{
			await clickedButton.ScaleTo(0.95, 50);
			await clickedButton.ScaleTo(1, 50);
		}
		if (index >= quizItems.Count - 1)
		{
			timer.Stop();
			lblTimer.Text = "Review Answers";
			ShowReview();
		}
		else
		{
			index++;
			showText();
		}
	}
	private void btnA_Clicked(object sender, EventArgs e)
	{
		RecordAnswerAndAdvance("A", btnA);
		timeRemaining = TimeSpan.FromSeconds(30);
	}
	private void btnB_Clicked(object sender, EventArgs e)
	{
		RecordAnswerAndAdvance("B", btnB);
		timeRemaining = TimeSpan.FromSeconds(30);
	}
	private void btnC_Clicked(object sender, EventArgs e)
	{
		RecordAnswerAndAdvance("C", btnC);
		timeRemaining = TimeSpan.FromSeconds(30);
	}
	private void btnD_Clicked(object sender, EventArgs e)
	{
		RecordAnswerAndAdvance("D", btnD);
		timeRemaining = TimeSpan.FromSeconds(30);
	}

	private void btnShowScore_Clicked(object sender, EventArgs e)
	{
		int score = 0;

		for (int i = 0; i < questions.userAnswerItems.Count; i++)
		{
			if (quizItems[i].Answer == questions.userAnswerItems[i].Answer)
				score++;
		}

		DisplayAlertAsync("Final Score", $"{score} / {questions.userAnswerItems.Count}", "OK");
	}

	private void ShowReview()
	{
		List<ReviewItem> review = new List<ReviewItem>();

		for (int i = 0; i < questions.userAnswerItems.Count; i++)
		{
			string correct = quizItems[i].Answer;
			string user = questions.userAnswerItems[i].Answer;

			string userText = "";
			string correctText = "";

			switch (user)
			{
				case "A": userText = quizItems[i].Options.A; break;
				case "B": userText = quizItems[i].Options.B; break;
				case "C": userText = quizItems[i].Options.C; break;
				case "D": userText = quizItems[i].Options.D; break;
				default: userText = ""; break;
			}

			switch (correct)
			{
				case "A": correctText = quizItems[i].Options.A; break;
				case "B": correctText = quizItems[i].Options.B; break;
				case "C": correctText = quizItems[i].Options.C; break;
				case "D": correctText = quizItems[i].Options.D; break;
			}

			review.Add(new ReviewItem
			{
				QuestionNumber = $"{quizItems[i].Id}. ",
				Question = $"{quizItems[i].Question}",
				UserAnswerLetter = user,
				UserAnswerText = userText,
				CorrectLetter = correct,
				CorrectText = correctText,
				UserColor = user == correct ? Colors.Green : Colors.Red
			});
		}

		reviewList.ItemsSource = review;

		btnA.IsVisible = false;
		btnB.IsVisible = false;
		btnC.IsVisible = false;
		btnD.IsVisible = false;
		lblQuestions.IsVisible = false;
		quizCard.IsVisible = false;

		reviewList.IsVisible = true;
		btnShowScore.IsVisible = true;
		btnQuizAgain.IsVisible = true;
	}
	private void btnStartQuiz_Clicked(object sender, EventArgs e)
	{
		string checkedRadio = rbMobile.IsChecked ? "dotnetQuiz.json" : "comProgQuiz.json";

		var items = Questions.GetQuizItems(checkedRadio);

		if (items == null || items.Count == 0)
		{
			DisplayAlert("Error", "Could not load quiz questions.", "OK");
			return;
		}

		index = 0;
		quizItems.Clear();
		quizItems.AddRange(items);

		questions.userAnswerItems.Clear();
		foreach (var item in quizItems)
		{
			questions.userAnswerItems.Add(new QuizItem { Id = item.Id });
		}

		selectionContainer.IsVisible = false;
		btnStartQuiz.IsVisible = false;
		quizCard.IsVisible = true;
		lblQuestions.IsVisible = true;
		btnA.IsVisible = btnB.IsVisible = btnC.IsVisible = btnD.IsVisible = true;

		showText();

		if (timer == null)
		{
			timer = Application.Current.Dispatcher.CreateTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += (s, args) =>
			{
				if (timeRemaining.TotalSeconds > 0)
				{
					timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));
					lblTimer.Text = timeRemaining.ToString(@"ss");
				}
				else
				{
					timeRemaining = TimeSpan.FromSeconds(30);
					RecordAnswerAndAdvance("No Answer", null);
				}
			};
		}

		timeRemaining = TimeSpan.FromSeconds(30);
		timer.Start();
	}

	private void btnQuizAgain_Clicked(object sender, EventArgs e)
	{
		selectionContainer.IsVisible = true;
		btnStartQuiz.IsVisible = true;
		btnA.IsVisible = false;
		btnB.IsVisible = false;
		btnC.IsVisible = false;
		btnD.IsVisible = false;
		lblQuestions.IsVisible = false;
		quizCard.IsVisible = false;
		reviewList.IsVisible = false;
		btnShowScore.IsVisible = false;
		btnQuizAgain.IsVisible = false;
		quizProgress.Progress = 0;
	}
}
