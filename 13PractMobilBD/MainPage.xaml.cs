namespace _13PractMobilBD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            lvService.ItemsSource = APIMetods1.Get<List<Service>>("api/Services");
            var gg= APIMetods1.Get<List<Service>>("api/Services");
        }

        

        private async void OnAddServiceClicked(object sender, EventArgs e)
        {
            // Переход на страницу добавления/редактирования
            await Navigation.PushModalAsync(new AddEditServicePage());
        }

        private async void OnServiceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Service selectedService)
            {
                // Передача выбранной услуги на страницу редактирования
                Data.Service = selectedService;
                await Navigation.PushModalAsync(new AddEditServicePage());
                lvService.SelectedItem = null; // Сброс выбора
            }
        }
    }

}
