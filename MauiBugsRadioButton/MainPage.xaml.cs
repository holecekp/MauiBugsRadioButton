namespace MauiBugsRadioButton
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            string two = 2.ToString();
            LabelTwo.Text = two;
            LabelTwoIsCorrect.Text = IsCorrectText(two, "2");

            string twelve = 12.ToString();
            LabelTwelve.Text = twelve;
            LabelTwelveIsCorrect.Text = IsCorrectText(twelve, "12");

            string twoHundred = 200.ToString();
            LabelTwohundred.Text = twoHundred;
            LabelTwohundredIsCorrect.Text = IsCorrectText(twoHundred, "200");
        }

        private string IsCorrectText(string actual, string expected)
        {
            AndroidLog($"Testing Int.Parse - Expected: {expected}. Actual: {actual}");

            return actual == expected ? "OK" : "Incorrect !!!!!";
        }

        public static void AndroidLog(string s)
        {
#if ANDROID
            Android.Util.Log.Info("Int.ParseTest", s);
#endif
        }
    }

}
