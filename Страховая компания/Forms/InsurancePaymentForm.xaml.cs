using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
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
    /// Логика взаимодействия для InsurancePaymentForm.xaml
    /// </summary>
    public partial class InsurancePaymentForm : Window
    {
        InsurancePayments selectedItem = new InsurancePayments();
        public InsurancePaymentForm()
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

        public ObservableCollection<InsurancePayments> FilteredItems { get; set; } = new ObservableCollection<InsurancePayments>();

        private List<InsurancePayments> GetFilteredResults(string filter)
        {
            using var context = new ExamenContext();

            var filtered = new List<InsurancePayments>();

            filtered.AddRange(context.insurancepayment.Where(d =>
                                                d.date.ToString() == filter));

            var isNumber = int.TryParse(filter, out int filterNumber);

            if (!isNumber)
            {
                return filtered;
            }

            filtered.AddRange(context.insurancepayment.Where(d =>
                                                d.ipid == filterNumber ||
                                                d.id_treaty== filterNumber ||
                                                d.payout_amount == filterNumber));

            return filtered;

        }
        private void UpdateData()
        {
            using var context = new ExamenContext();
            var insur = context.insurancepayment.ToList();
            var treat = context.treaty.ToList();
            foreach (var item in treat)
            {
                ComboBoxTreaty.Items.Add(item.tid);
            }
            
            TestView.ItemsSource = insur;

        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxTreaty.SelectedValue != null && TextBoxPayout.Text != null && DatePickerDate.SelectedDate != null)
            {
                string res = AddElementsToBD.AddInsurancePayment((DateTime)DatePickerDate.SelectedDate, ComboBoxTreaty.SelectedValue.ToString(), TextBoxPayout.Text);
                if (res == "Добавленно")
                {
                    DatePickerDate.Text = null;
                    ComboBoxTreaty.Text = null;
                    TextBoxPayout.Text = null;
                }
                SnackBar(res);
                UpdateData();
            }
            else
            {
                SnackBar("Ошибка заполнения данных");
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            try
            {
                var tra = ctx.treaty.FirstOrDefault( t => t.tid == Convert.ToInt32(ComboBoxTreaty.SelectedValue));
                if (selectedItem != null && tra != null)
                {
                    selectedItem.date = DateOnly.FromDateTime((DateTime)DatePickerDate.SelectedDate);
                    selectedItem.id_treaty = tra.tid;
                    selectedItem.treaty = tra;
                    selectedItem.payout_amount = Convert.ToInt32(TextBoxPayout.Text);
                }
                ctx.insurancepayment.Update(selectedItem);
                ctx.SaveChanges();
                UpdateData();
                SnackBar("Обновление данных");
                DatePickerDate.Text = null;
                ComboBoxTreaty.Text = null;
                TextBoxPayout.Text = null;
            }
            catch (Exception ex)
            {
                SnackBar("Обшибка заполнения данных");
                return;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DatePickerDate.Text = null;
            ComboBoxTreaty.Text = null;
            TextBoxPayout.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using var ctx = new ExamenContext();
            ctx.insurancepayment.Remove(selectedItem);
            ctx.SaveChanges();
            ButtonsVisible();
            SnackBar("Запись удалена");
            UpdateData();
            DatePickerDate.Text = null;
            ComboBoxTreaty.Text = null;
            TextBoxPayout.Text = null;
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
            selectedItem = (InsurancePayments)TestView.SelectedItem;
            if (selectedItem != null)
            {
                DatePickerDate.Text = selectedItem.date.ToString();
                ComboBoxTreaty.Text = selectedItem.id_treaty.ToString();
                TextBoxPayout.Text = selectedItem.payout_amount.ToString();
                ButtonUpdate.IsEnabled = true;
                ButtonCancel.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
                ButtonAdd.IsEnabled = false;
            }
        }
    }
}
