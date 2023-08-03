using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace cviceni_7_3
{
    public partial class MainPage : ContentPage
    {
        internal HangmanGame game;
        BestScoresService bestScoresService;

        public MainPage(BestScoresService bestScoresService)
        {
            this.bestScoresService = bestScoresService;
            game = new HangmanGame(10);

            InitializeComponent();

            BindingContext = game;
        }

        public async void OnRestartButtonClicked(object sender, EventArgs e)
        {
            try
            {
                game.Draw();
            }
            catch (AllWordsTriedException)
            {
                await DisplayAlert("", "Už jsi hádal všechna slova", "Začít znovu");
                game = game.Reset();
                BindingContext = game;
            }
        }

        private async void OnChartButtonClicked(object sender, EventArgs e)
        {
            BestScoresPage bestScoresPage = new BestScoresPage(this, null, bestScoresService);
            bestScoresPage.BindingContext = bestScoresService.BestScores.ToList().OrderBy(hps => hps.Misses).ThenBy(hps => hps.Id);
            await Navigation.PushModalAsync(bestScoresPage);
        }

        private async void OnTipButtonClicked(object sender, EventArgs e)
        {
            if (TipEntry.Text != null && TipEntry.Text != "")
            {
                HangmanGameState state = game.GuessChar(TipEntry.Text[0]);
                if (state.HasFlag(HangmanGameState.Lost))
                {
                    LossRevealPage lossRevealPage = new LossRevealPage(this);
                    lossRevealPage.BindingContext = game;
                    await Navigation.PushModalAsync(lossRevealPage);
                }
                if (state.HasFlag(HangmanGameState.Won))
                {
                    NickEntryPage nickEntryPage = new NickEntryPage(this, bestScoresService);
                    nickEntryPage.BindingContext = game;
                    await Navigation.PushModalAsync(nickEntryPage);
                }
            }
        }

        private void OnTipEntryFocused(object sender, FocusEventArgs e)
        {
            ((Entry)sender).Text = "";
        }
    }
}
