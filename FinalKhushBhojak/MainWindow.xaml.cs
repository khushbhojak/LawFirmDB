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


namespace FinalKhushBhojak
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

        private void FillDataGrid(List<Billing> billings)
        {
            grdBills.ItemsSource = billings;
        }

        private void FillGrid(List<Client> client)
        {
            grdBills.ItemsSource = client;
        }

        private void btnLoadAllBills_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new LawFirmDBEntities())
            {
                var billings = context.Billings.ToList();
                FillDataGrid(billings);
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new LawFirmDBEntities())
            {
                        var attorneys = context.Attorneys.ToList();
                        cmbAttorney.ItemsSource = attorneys;
                        cmbAttorney.DisplayMemberPath = "LastName";
                        cmbAttorney.SelectedValuePath = "AttorneyID";
            }
        }

        
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            int BillID = int.Parse(txtId.Text);

            using (var context = new LawFirmDBEntities())
            {

                var bills = context.Billings.Find(BillID);
                var clt = context.Clients.Find(bills.ClientID);
                var att = context.Attorneys.Find(bills.AttorneyID);
                var category = context.Categories.Find(bills.CategoryID);
                var r = context.Rates.Find(bills.RateID);

                txtClient.Text = clt.FirstName + " " + clt.LastName;
                txtAttorney.Text = att.FirstName + " " + att.LastName;
                txtCategory.Text = category.Category1;
                txtFee.Text = ((double)r.Rate1 * bills.Hours).ToString();
                cmbAttorney.SelectedValue = bills.AttorneyID; ;

            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new LawFirmDBEntities())
            {
                var clients = context.Clients
                    .Where(s => s.FirstName.Contains(txtName.Text) || s.LastName.Contains(txtName.Text)).ToList();

                // display fetched billings in data grid
                FillGrid(clients);
            }

            // clear the fields
            txtId.Text = "";
            cmbAttorney.SelectedIndex = -1;
        }

      

        private void btnClearBills_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = txtClient.Text = txtAttorney.Text = txtFee.Text = txtCategory.Text= txtId.Text="";
        }

        private void cmbAttorney_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new LawFirmDBEntities())
            {
                var x = context.Billings
                .Where(b => b.AttorneyID == (int)cmbAttorney.SelectedValue)
                .ToList();
                grdBills.ItemsSource = x;
            }
        }
    }

}
