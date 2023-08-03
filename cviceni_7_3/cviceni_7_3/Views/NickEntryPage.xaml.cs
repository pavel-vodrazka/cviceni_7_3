using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cviceni_7_3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NickEntryPage : ContentPage
    {
        MainPage mainPage;
        BestScoresService bestScoresService;
        string nick;

        public NickEntryPage(MainPage mainPage, BestScoresService bestScoresService)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            this.bestScoresService = bestScoresService;
        }

        protected override bool OnBackButtonPressed()
        {
            mainPage.OnRestartButtonClicked(this, new EventArgs());
            Navigation.PopModalAsync();
            return true;
        }

        public void SimulateBackButtonPressed()
        {
            OnBackButtonPressed();
        }

        private async void OKButtonClicked(object sender, EventArgs e)
        {
            nick = NickEntry.Text;
            if (nick != null && nick != string.Empty)
            {
                HangmanPlayerScore score = new HangmanPlayerScore(nick, mainPage.game.Misses);
                await bestScoresService.SaveBestPlayerScoreAsync(score);
            }
            BestScoresPage bestScoresPage = new BestScoresPage(mainPage, this, bestScoresService);
            bestScoresPage.BindingContext = bestScoresService.BestScores.ToList().OrderBy(hps => hps.Misses).ThenBy(hps => hps.Id);
            await Navigation.PushModalAsync(bestScoresPage);
        }
    }
}