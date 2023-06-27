using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для ObjectsOfInsuranceForm.xaml
    /// </summary>
    public partial class ObjectsOfInsuranceForm : Window
    {
        ObjectsOfInsurance selectedItem = new ObjectsOfInsurance();
        public ObjectsOfInsuranceForm()
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

        public ObservableCollection<ObjectsOfInsurance> FilteredItems { get; set; } = new ObservableCollection<ObjectsOfInsurance>();

        private List<ObjectsOfInsurance> GetFilteredResults(string filter)
        {
            using var context = new ExamenContext();

            var filtered = new List<ObjectsOfInsurance>();

            filtered.AddRange(context.objectofinsurance.Where(d =>
                                                d.name.ToLower().Contains(filter)));

            var isNumber = int.TryParse(filter, out int filterNumber);

            if (!isNumber)
            {
                return filtered;
            }

            filtered.AddRange(context.objectofinsurance.Where(d =>
                                                d.ooiid == filterNumber));

            return filtered;

        }
        private void UpdateData()
        {
            using var context = new ExamenContext();
            var objects = context.objectofinsurance.ToList();
            TestView.ItemsSource = objects;

        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string res = AddElementsToBD.AddObjectOfInsurance(TextBoxname.Text);
            if (res == "Добавленно")
            {
                TextBoxname.Text = null;
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
                    selectedItem.name = TextBoxname.Text;
                }
                ctx.objectofinsurance.Update(selectedItem);
                ctx.SaveChanges();
                UpdateData();
                SnackBar("Обновление данных");
                TextBoxname.Text = null;
            }
            catch (Exception ex)
            {
                SnackBar("Ошибка заполнения данных");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxname.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            ctx.objectofinsurance.Remove(selectedItem);
            ctx.SaveChanges();
            TextBoxname.Text = null;
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
            selectedItem = (ObjectsOfInsurance)TestView.SelectedItem;
            if (selectedItem != null)
            {
                TextBoxname.Text = selectedItem.name;
                ButtonUpdate.IsEnabled = true;
                ButtonCancel.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
                ButtonAdd.IsEnabled = false;
            }
        }
    }
}
