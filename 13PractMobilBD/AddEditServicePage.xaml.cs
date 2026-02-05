using System;
using Microsoft.Maui.Controls;

namespace _13PractMobilBD
{
    public partial class AddEditServicePage : ContentPage
    {
        private bool _isEditMode = false;
        private Service _currentService;

        public AddEditServicePage()
        {
            InitializeComponent();
            InitializePage();
        }

        private void InitializePage()
        {
            
            _currentService = Data.Service;

            if (_currentService != null)
            {
              
                _isEditMode = true;

              
                lblCode.Text = $"Код: {_currentService.Code}";
                lblCode.IsVisible = true;

                txtName.Text = _currentService.Name;
                txtPrice.Text = _currentService.Price.ToString();

               
                btnDelete.IsVisible = true;
                btnSave.Text = "Обновить";

                
                Data.Service = null;
            }
            else
            {
               
                _isEditMode = false;
                btnSave.Text = "Добавить";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                await DisplayAlert("Ошибка", "Введите название услуги", "OK");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                await DisplayAlert("Ошибка", "Введите корректную цену", "OK");
                return;
            }

            try
            {
                if (_isEditMode && _currentService != null)
                {
                    var serviceToUpdate = new Service
                    {
                        Code = _currentService.Code,
                        Name = txtName.Text.Trim(),
                        Price = price
                    };

                    
                    var result = APIMetods1.PutWithId(serviceToUpdate, serviceToUpdate.Code, "api/Services");
                }
                else
                {
                    var newService = new Service
                    {
                        Name = txtName.Text.Trim(),
                        Price = price
                    };

                    var result = APIMetods1.Post(newService, "api/Services");
                
                

                    await DisplayAlert("Успех", "Услуга добавлена", "OK");
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
            if (_currentService == null)
                return;

          
            bool answer = await DisplayAlert("Подтверждение",
                $"Вы уверены, что хотите удалить услугу \"{_currentService.Name}\"?",
                "Да", "Нет");

            if (answer)
            {
                try
                {

                    var result = APIMetods1.DeleteWithId(_currentService.Code, "api/Services");

                    await DisplayAlert("Успех", "Услуга удалена", "OK");

                   
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