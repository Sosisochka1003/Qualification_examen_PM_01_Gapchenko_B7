using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Логика взаимодействия для EmployeeForm.xaml
    /// </summary>
    public partial class EmployeeForm : Window
    {
        Employees selectedItem = new Employees();
        public EmployeeForm()
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

        public ObservableCollection<Employees> FilteredItems { get; set; } = new ObservableCollection<Employees>();

        private List<Employees> GetFilteredResults(string filter)
        {
            using var context = new ExamenContext();

            var filtered = new List<Employees>();

            filtered.AddRange(context.employee.Where(d =>
                                                d.last_name.ToLower().Contains(filter) ||
                                                d.name.ToLower().Contains(filter) ||
                                                d.patronymic.ToLower().Contains(filter)));

            var isNumber = int.TryParse(filter, out int filterNumber);

            if (!isNumber)
            {
                return filtered;
            }

            filtered.AddRange(context.employee.Where(d =>
                                                d.eid == filterNumber ||
                                                d.branch == filterNumber));

            return filtered;

        }
        private void UpdateData()
        {
            using var context = new ExamenContext();
            var emp = context.employee.ToList();
            TestView.ItemsSource = emp;

        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var res = AddElementsToBD.AddEmp(TextBoxBranch.Text, TextBoxLastName.Text, TextBoxName.Text, TextBoxPatronymic.Text);
            if (res == "Добавленно")
            {
                TextBoxBranch.Text = null;
                TextBoxLastName.Text = null;
                TextBoxName.Text = null;
                TextBoxPatronymic.Text = null;
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
                    selectedItem.branch = Convert.ToInt32(TextBoxBranch.Text);
                    selectedItem.last_name = TextBoxLastName.Text;
                    selectedItem.name = TextBoxName.Text;
                    selectedItem.patronymic = TextBoxPatronymic.Text;
                }
                ctx.employee.Update(selectedItem);
                ctx.SaveChanges();
                UpdateData();
                SnackBar("Обновление данных");
                TextBoxBranch.Text = null;
                TextBoxLastName.Text = null;
                TextBoxName.Text = null;
                TextBoxPatronymic.Text = null;
            }
            catch (Exception ex)
            {
                SnackBar("Ошибка заполнения данных");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxBranch.Text = null;
            TextBoxLastName.Text = null;
            TextBoxName.Text = null;
            TextBoxPatronymic.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            ctx.employee.Remove(selectedItem);
            ctx.SaveChanges();
            TextBoxBranch.Text = null;
            TextBoxLastName.Text = null;
            TextBoxName.Text = null;
            TextBoxPatronymic.Text = null;
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
            selectedItem = (Employees)TestView.SelectedItem;
            if (selectedItem != null)
            {
                TextBoxBranch.Text = selectedItem.branch.ToString();
                TextBoxLastName.Text = selectedItem.last_name;
                TextBoxName.Text = selectedItem.name;
                TextBoxPatronymic.Text = selectedItem.patronymic;
                ButtonUpdate.IsEnabled = true;
                ButtonCancel.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
                ButtonAdd.IsEnabled = false;
            }
        }
    }
}
