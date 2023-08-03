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
    public partial class BestScoresPage : ContentPage
    {
        MainPage mainPage;
        NickEntryPage nickEntryPage;
        BestScoresService bestScoresService;
        public bool JustPeeked { get; private set; }
        public BestScoresPage(MainPage mainPage, NickEntryPage nickEntryPage, BestScoresService bestScoresService)
        {
            this.mainPage = mainPage;
            this.nickEntryPage = nickEntryPage;
            JustPeeked = nickEntryPage is null;
            this.bestScoresService = bestScoresService;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopModalAsync();
            nickEntryPage?.SimulateBackButtonPressed();
            return true;
        }

        public async void OKButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
            nickEntryPage?.SimulateBackButtonPressed();
        }
    }
}