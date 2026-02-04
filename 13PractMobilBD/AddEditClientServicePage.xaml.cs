using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace _13PractMobilBD
{
    public partial class AddEditClientServicePage : ContentPage
    {
        private bool _isEditMode = false;
        private ClientServiceDTO _currentClientService;

        public AddEditClientServicePage()
        {
            InitializeComponent();
            InitializePage();
        }

        private async void InitializePage()
        {
            // Загружаем списки клиентов и услуг
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

            // Проверяем режим редактирования
            _currentClientService = Data.ClientService;
            _isEditMode = (_currentClientService != null);

            if (_isEditMode)
            {
                Title = "Редактировать запись";
                btnSave.Text = "Обновить";
                btnDelete.IsVisible = true;

                // Устанавливаем выбранные значения
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
            // Проверки
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

            // Собираем дату и время
            DateTime appointmentDateTime = datePicker.Date + timePicker.Time;

            try
            {
                if (_isEditMode && _currentClientService != null)
                {
                    // Обновление существующей записи
                    var dto = new ClientServiceDTO
                    {
                        ClientId = selectedClient.CardNumber,
                        ServiceId = selectedService.Code,
                        AppointmentDateTime = appointmentDateTime
                    };

                    // Используем PUT с двумя параметрами
                    string endpoint = $"api/ClientServices/{_currentClientService.ClientId}/{_currentClientService.AppointmentDateTime:yyyy-MM-ddTHH:mm:ss}";
                    var response = APIMetods1.Put(dto, endpoint);

                    await DisplayAlert("Успех", "Запись обновлена", "OK");
                }
                else
                {
                    // Создание новой записи
                    var dto = new ClientServiceDTO
                    {
                        ClientId = selectedClient.CardNumber,
                        ServiceId = selectedService.Code,
                        AppointmentDateTime = appointmentDateTime
                    };

                    var result = APIMetods1.Post(dto, "api/ClientServices");
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
            if (_currentClientService == null)
                return;

            bool answer = await DisplayAlert("Подтверждение",
                "Вы уверены, что хотите удалить эту запись?",
                "Да", "Нет");

            if (answer)
            {
                try
                {
                    // DELETE с двумя параметрами
                    string endpoint = $"api/ClientServices/{_currentClientService.ClientId}/{_currentClientService.AppointmentDateTime:yyyy-MM-ddTHH:mm:ss}";
                    var result = APIMetods1.Delete(endpoint);

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