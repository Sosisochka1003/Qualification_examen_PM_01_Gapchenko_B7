using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Страховая_компания.DataBase;
using Страховая_компания.Functions;

namespace Страховая_компания.Forms
{
    /// <summary>
    /// Логика взаимодействия для ClientForm.xaml
    /// </summary>
    public partial class ClientForm : Window
    {
        Clients selectedItem = new Clients();

        public ClientForm()
        {
            InitializeComponent();
            UpdateData();
        }

        public async void SnackBar(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.Message.Content = text;
                SnackbarOne.IsActive = true;
            });
            await Task.Delay(2500);
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.IsActive = false;
            });
        }

        private void ButtonsVisible()
        {
            ButtonUpdate.IsEnabled = false;
            ButtonCancel.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAdd.IsEnabled = true;
        }

        public ObservableCollection<Clients> FilteredItems { get; set; } = new ObservableCollection<Clients>();

        private List<Clients> GetFilteredResults(string filter)
        {
            using var context = new ExamenContext();
            
            var filtered = new List<Clients>();

            filtered.AddRange(context.client.Where(d =>
                                                d.last_name.ToLower().Contains(filter) ||
                                                d.name.ToLower().Contains(filter) ||
                                                d.patronymic.ToLower().Contains(filter) ||
                                                d.gender.ToLower().Contains(filter)));

            var isNumber = int.TryParse(filter, out int filterNumber);

            if (!isNumber)
            {
                return filtered;
            }

            filtered.AddRange(context.client.Where(d =>
                                                d.cid == filterNumber ||
                                                d.passport_series == filterNumber ||
                                                d.passport_number == filterNumber));

            return filtered;
            
        }
        private void UpdateData()
        {
            using var context = new ExamenContext();
                var client = context.client.ToList();
                TestView.ItemsSource = client;
            
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string res = AddElementsToBD.AddClient(TextBoxLastName.Text, TextBoxName.Text, TextBoxPatronymic.Text, TextBoxGender.Text, (DateTime)DatePickerDateOfBirth.SelectedDate, TextBoxSeriesPass.Text, TextBoxNumberPass.Text);
            if (res == "Добавленно")
            {
                TextBoxLastName.Text = null;
                TextBoxName.Text = null;
                TextBoxPatronymic.Text = null;
                TextBoxGender.Text = null;
                DatePickerDateOfBirth.Text = null;
                TextBoxSeriesPass.Text = null;
                TextBoxNumberPass.Text = null;
            }
            SnackBar(res);
            UpdateData();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            try
            {
                if (selectedItem != null)
                {
                    selectedItem.last_name = TextBoxLastName.Text;
                    selectedItem.name = TextBoxName.Text;
                    selectedItem.patronymic = TextBoxPatronymic.Text;
                    selectedItem.gender = TextBoxGender.Text;
                    selectedItem.date_of_birth = DateOnly.FromDateTime((DateTime)DatePickerDateOfBirth.SelectedDate);
                    selectedItem.passport_series = Convert.ToInt32(TextBoxSeriesPass.Text);
                    selectedItem.passport_number = Convert.ToInt32(TextBoxNumberPass.Text);
                }
                ctx.client.Update(selectedItem);
                ctx.SaveChanges();
                SnackBar("Обновление данных");
                ButtonsVisible();
                UpdateData();
                TextBoxLastName.Text = null;
                TextBoxName.Text = null;
                TextBoxPatronymic.Text = null;
                TextBoxGender.Text = null;
                DatePickerDateOfBirth.Text = null;
                TextBoxSeriesPass.Text = null;
                TextBoxNumberPass.Text = null;
            }
            catch (Exception asd)
            {
                SnackBar("Неверные данные");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxLastName.Text = null;
            TextBoxName.Text = null;
            TextBoxPatronymic.Text = null;
            TextBoxGender.Text = null;
            DatePickerDateOfBirth.Text = null;
            TextBoxSeriesPass.Text = null;
            TextBoxNumberPass.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            ctx.client.Remove(selectedItem);
            ctx.SaveChanges();
            TextBoxLastName.Text = null;
            TextBoxName.Text = null;
            TextBoxPatronymic.Text = null;
            TextBoxGender.Text = null;
            DatePickerDateOfBirth.Text = null;
            TextBoxSeriesPass.Text = null;
            TextBoxNumberPass.Text = null;
            ButtonsVisible();
            SnackBar("Запись удалена");
            UpdateData();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Clients)TestView.SelectedItem;
            if (selectedItem != null)
            {
                TextBoxLastName.Text = selectedItem.last_name;
                TextBoxName.Text = selectedItem.name;
                TextBoxPatronymic.Text = selectedItem.patronymic;
                TextBoxGender.Text = selectedItem.gender;
                DatePickerDateOfBirth.Text = selectedItem.date_of_birth.ToString();
                TextBoxSeriesPass.Text = selectedItem.passport_series.ToString();
                TextBoxNumberPass.Text = selectedItem.passport_number.ToString();
                ButtonUpdate.IsEnabled = true;
                ButtonCancel.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
                ButtonAdd.IsEnabled = false;
            }
        }
    }
}
