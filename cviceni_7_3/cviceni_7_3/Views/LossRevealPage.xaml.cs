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
    public partial class LossRevealPage : ContentPage
    {
        MainPage mainPage;
        public LossRevealPage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
        }

        protected override bool OnBackButtonPressed()
        {
            mainPage.OnRestartButtonClicked(this, new EventArgs());
            Navigation.PopModalAsync();
            return true;
        }

        private void OnNextGameButtonClicked(object sender, EventArgs e)
        {
            mainPage.OnRestartButtonClicked(this, e);
            Navigation.PopModalAsync();
        }
    }
}