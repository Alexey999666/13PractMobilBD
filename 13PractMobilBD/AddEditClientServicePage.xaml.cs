using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace _13PractMobilBD
{
    public partial class AddEditClientServicePage : ContentPage
    {
        private bool _isEditMode = false;
        private ClientServiceDTO _currentClientService;
        private int? _currentId;

        public AddEditClientServicePage()
        {
            InitializeComponent();
            InitializePage();
        }

        private async void InitializePage()
        {

            _currentId = Data.SelectedClientServiceId;

            _currentClientService = Data.ClientService;
            _isEditMode = (_currentClientService != null && _currentId.HasValue);
            try
            {
                var clients = APIMetods1.Get<List<Client>>("api/Clients");
                var services = APIMetods1.Get<List<Service>>("api/Services");

                if (clients != null && services != null)
                {
                    pickerClient.ItemsSource = clients;
                    pickerService.ItemsSource = services;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить данные: {ex.Message}", "OK");
            }

            if (_isEditMode && _currentClientService != null)
            {

                

                Title = "Редактировать запись";
                btnSave.Text = "Обновить";
                btnDelete.IsVisible = true;

               
                if (pickerClient.ItemsSource is List<Client> clientsList)
                {
                    var selectedClient = clientsList.FirstOrDefault(c => c.CardNumber == _currentClientService.ClientId);
                    if (selectedClient != null)
                        pickerClient.SelectedItem = selectedClient;
                }

                if (pickerService.ItemsSource is List<Service> servicesList)
                {
                    var selectedService = servicesList.FirstOrDefault(s => s.Code == _currentClientService.ServiceId);
                    if (selectedService != null)
                        pickerService.SelectedItem = selectedService;
                }

                datePicker.Date = _currentClientService.AppointmentDateTime.Date;
                timePicker.Time = _currentClientService.AppointmentDateTime.TimeOfDay;
                Data.ClientService = null;
                Data.SelectedClientServiceId = null;
            }
            else
            {
                Title = "Новая запись";
                btnSave.Text = "Добавить";
                datePicker.Date = DateTime.Now;
                timePicker.Time = DateTime.Now.TimeOfDay;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            
            if (pickerClient.SelectedItem == null)
            {
                await DisplayAlert("Ошибка", "Выберите клиента", "OK");
                return;
            }

            if (pickerService.SelectedItem == null)
            {
                await DisplayAlert("Ошибка", "Выберите услугу", "OK");
                return;
            }

            var selectedClient = (Client)pickerClient.SelectedItem;
            var selectedService = (Service)pickerService.SelectedItem;

            
            DateTime appointmentDateTime = datePicker.Date + timePicker.Time;

            try
            {
                var dto = new ClientServiceDTO
                {
                    ClientId = selectedClient.CardNumber,
                    ServiceId = selectedService.Code,
                    AppointmentDateTime = appointmentDateTime
                };

                if (_isEditMode && _currentId.HasValue)
                {


                    dto.IdclientServices = _currentId.Value;

                    // Используем стандартный PUT с ID
                    APIMetods1.PutWithId(dto, _currentId.Value, "api/ClientServices");
                    await DisplayAlert("Успех", "Запись обновлена", "OK");
                }
                else
                {
                    
                    APIMetods1.Post(dto, "api/ClientServices");
                    await DisplayAlert("Успех", "Запись добавлена", "OK");
                }

                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось сохранить: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (!_currentId.HasValue)
                return;

            bool answer = await DisplayAlert("Подтверждение",
                "Вы уверены, что хотите удалить эту запись?",
                "Да", "Нет");

            if (answer)
            {
                try
                {
                   
                    APIMetods1.DeleteWithId(_currentId.Value, "api/ClientServices");
                    await DisplayAlert("Успех", "Запись удалена", "OK");
                    await Navigation.PopModalAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", $"Не удалось удалить: {ex.Message}", "OK");
                }
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}