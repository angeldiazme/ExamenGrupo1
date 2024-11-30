using MediaManager;

namespace Examen_Grupo2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override void OnStart()
        {
            base.OnStart();
            CrossMediaManager.Current.Init();
        }
    }
}
