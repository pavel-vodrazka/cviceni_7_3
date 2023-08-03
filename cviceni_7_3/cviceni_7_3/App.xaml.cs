using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cviceni_7_3
{
    public partial class App : Application
    {
        BestScoresService bestPlayersService;

        public App()
        {
            InitializeComponent();
            bestPlayersService = new BestScoresService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bestPlayers.db"));
            MainPage = new MainPage(bestPlayersService);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
