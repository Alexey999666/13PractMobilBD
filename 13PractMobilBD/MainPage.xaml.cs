using System;
using Microsoft.Maui.Controls;

namespace _13PractMobilBD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadServices(); 
        }

        

        private void LoadServices()
        {
            try
            {
                var services = APIMetods1.Get<List<Service>>("api/Services");
                if (services != null)
                {
                   
                    lvService.ItemsSource = null;
                    lvService.ItemsSource = services;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
        }

        private async void OnAddServiceClicked(object sender, EventArgs e)
        {
           
            var editPage = new AddEditServicePage();

           
            editPage.Disappearing += (s, args) =>
            {
                
                LoadServices();
            };

            await Navigation.PushModalAsync(editPage);
        }
        private async void OnClientServicesClicked(object sender, EventArgs e)
        {
            var clientServicesPage = new ClientServicesPage();
            await Navigation.PushAsync(clientServicesPage);
        }
        private async void OnServiceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Service selectedService)
            {
                lvService.SelectedItem = null; 

              
                Data.Service = selectedService;

                var editPage = new AddEditServicePage();

               
                editPage.Disappearing += (s, args) =>
                {
                   
                    LoadServices();
                };

                await Navigation.PushModalAsync(editPage);
            }
        }
    }
}