using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Страховая_компания.Forms;

namespace Страховая_компания
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClients_Click(object sender, RoutedEventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            clientForm.Show();
        }

        private void ButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm();
            employeeForm.Show();
        }

        private void ButtonInsurancePayment_Click(object sender, RoutedEventArgs e)
        {
            InsurancePaymentForm insurancePaymentForm = new InsurancePaymentForm();
            insurancePaymentForm.Show();
        }

        private void ButtonObjectsOfInsurance_Click(object sender, RoutedEventArgs e)
        {
            ObjectsOfInsuranceForm objects = new ObjectsOfInsuranceForm();
            objects.Show();
        }

        private void ButtonTreaty_Click(object sender, RoutedEventArgs e)
        {
            TreatyForm treaty = new TreatyForm();
            treaty.Show();
        }
    }
}
