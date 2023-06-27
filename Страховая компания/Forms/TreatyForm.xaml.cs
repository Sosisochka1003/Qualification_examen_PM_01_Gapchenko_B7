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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Страховая_компания.DataBase;
using Страховая_компания.Functions;

namespace Страховая_компания.Forms
{
    /// <summary>
    /// Логика взаимодействия для TreatyForm.xaml
    /// </summary>
    public partial class TreatyForm : Window
    {
        Treaty selectedItem = new Treaty();
        public TreatyForm()
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

        public ObservableCollection<Treaty> FilteredItems { get; set; } = new ObservableCollection<Treaty>();

        private List<Treaty> GetFilteredResults(string filter)
        {
            using var context = new ExamenContext();

            var filtered = new List<Treaty>();

            filtered.AddRange(context.treaty.Where(d =>
                                                d.date_of_conclusion.ToString() == filter));

            var isNumber = int.TryParse(filter, out int filterNumber);

            if (!isNumber)
            {
                return filtered;
            }

            filtered.AddRange(context.treaty.Where(d =>
                                                d.tid == filterNumber ||
                                                d.id_client== filterNumber ||
                                                d.id_emp == filterNumber ||
                                                d.id_object == filterNumber));

            return filtered;

        }
        private void UpdateData()
        {
            using var context = new ExamenContext();
            var treaty = context.treaty.ToList();
            var clients = context.client.ToList();
            var emp = context.employee.ToList();
            var objects = context.objectofinsurance.ToList();
            foreach ( var item in clients )
            {
                ComboBoxClients.Items.Add( item.cid );
            }
            foreach ( var item in objects )
            {
                ComboBoxObjects.Items.Add(item.ooiid);
            }
            foreach ( var item in emp)
            {
                ComboBoxEmployee.Items.Add(item.eid);
            }
            TestView.ItemsSource = treaty;

        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string res = AddElementsToBD.AddTreaty((DateTime)DatePickerDateConclusion.SelectedDate, ComboBoxClients.SelectedValue.ToString(), ComboBoxEmployee.SelectedValue.ToString(), ComboBoxObjects.SelectedValue.ToString(), TextBoxInsurance.Text);
            if (res == "Добавленно")
            {
                DatePickerDateConclusion.Text = null;
                ComboBoxClients.Text = null;
                ComboBoxEmployee.Text = null;
                ComboBoxObjects.Text = null;
                TextBoxInsurance.Text = null;
            }
            SnackBar(res);
            UpdateData();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            try
            {
                var cli = ctx.client.FirstOrDefault(c => c.cid == Convert.ToInt32(ComboBoxClients.SelectedValue));
                var emp = ctx.employee.FirstOrDefault(e => e.eid == Convert.ToInt32(ComboBoxEmployee.SelectedValue));
                var obj = ctx.objectofinsurance.FirstOrDefault(o => o.ooiid == Convert.ToInt32(ComboBoxObjects.SelectedValue));
                if (selectedItem != null && cli != null && emp != null && obj != null)
                {
                    selectedItem.date_of_conclusion = DateOnly.FromDateTime((DateTime)DatePickerDateConclusion.SelectedDate);
                    selectedItem.id_client = cli.cid;
                    selectedItem.clients = cli;
                    selectedItem.id_emp = emp.eid;
                    selectedItem.employees = emp;
                    selectedItem.id_object = obj.ooiid;
                    selectedItem.objects = obj;
                    selectedItem.insurance_payment = Convert.ToInt32(TextBoxInsurance.Text);
                }
                ctx.treaty.Update(selectedItem);
                ctx.SaveChanges();
                UpdateData();
                SnackBar("Обновление данных");
                DatePickerDateConclusion.Text = null;
                ComboBoxClients.Text = null;
                ComboBoxEmployee.Text = null;
                ComboBoxObjects.Text = null;
                TextBoxInsurance.Text = null;

            }
            catch (Exception ex)
            {
                SnackBar("Ошибка заполнения данных");
                return;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DatePickerDateConclusion.Text = null;
            ComboBoxClients.Text = null;
            ComboBoxEmployee.Text = null;
            ComboBoxObjects.Text = null;
            TextBoxInsurance.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            ctx.treaty.Remove(selectedItem);
            ctx.SaveChanges();
            DatePickerDateConclusion.Text = null;
            ComboBoxClients.Text = null;
            ComboBoxEmployee.Text = null;
            ComboBoxObjects.Text = null;
            TextBoxInsurance.Text = null;
            ButtonsVisible();
            SnackBar("Запись удалена");
            UpdateData();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
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
            selectedItem = (Treaty)TestView.SelectedItem;
            if (selectedItem != null)
            {
                DatePickerDateConclusion.Text = selectedItem.date_of_conclusion.ToString();
                ComboBoxClients.Text = selectedItem.id_client.ToString();
                ComboBoxEmployee.Text = selectedItem.id_emp.ToString();
                ComboBoxObjects.Text = selectedItem.id_object.ToString();
                TextBoxInsurance.Text = selectedItem.insurance_payment.ToString();
                ButtonUpdate.IsEnabled = true;
                ButtonCancel.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
                ButtonAdd.IsEnabled = false;
            }
        }
    }
}
