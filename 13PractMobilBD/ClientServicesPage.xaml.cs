using System;
using Microsoft.Maui.Controls;

namespace _13PractMobilBD
{
    public partial class ClientServicesPage : ContentPage
    {
        public ClientServicesPage()
        {
            InitializeComponent();
            LoadClientServices();
        }

        private void LoadClientServices()
        {
            try
            {
                var clientServices = APIMetods1.Get<List<ClientServiceInfoDTO>>("api/ClientServices/Info");
                if (clientServices != null)
                {
                    lvClientServices.ItemsSource = null;
                    lvClientServices.ItemsSource = clientServices;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"Не удалось загрузить записи: {ex.Message}", "OK");
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            Data.ClientServiceInfo = null;
            Data.ClientService = null;
            var editPage = new AddEditClientServicePage();
            editPage.Disappearing += (s, args) => LoadClientServices();
            await Navigation.PushModalAsync(editPage);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ClientServiceInfoDTO selectedItem)
            {
                lvClientServices.SelectedItem = null;

               
                Data.ClientService = new ClientServiceDTO
                {
                    ClientId = selectedItem.ClientId,
                    ServiceId = selectedItem.ServiceId,
                    AppointmentDateTime = selectedItem.AppointmentDateTime
                };

                var editPage = new AddEditClientServicePage();
                editPage.Disappearing += (s, args) => LoadClientServices();
                await Navigation.PushModalAsync(editPage);

                
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadClientServices();
        }
    }
}