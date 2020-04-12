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

namespace ContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Person> people_results = new List<Person>();
        List<Person> address_results = new List<Person>();
        List<Person> email_results = new List<Person>();
       
        public MainWindow()
        {
            InitializeComponent();
      
            btnDelete.IsEnabled = false;  
            btnUpdate.IsEnabled = false;

           
        }//end constructor



        private void RefreshListBoxBinding()
        {
            //SET BINDING INSTANCE FOR LISTBOX
            lstResults.ItemsSource = people_results;

            //SET BINDING FIELD FOR LISTBOX
            lstResults.DisplayMemberPath = "ResultData";          
        }//end method

        private void RefreshListBoxBindingEmail()
        {
            //SET BINDING INSTANCE FOR LISTBOX
            lstEmails.ItemsSource = email_results;

            //SET BINDING FIELD FOR LISTBOX
            lstEmails.DisplayMemberPath = "Email";
        }//end method

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();
            
            //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB
            people_results = database_data.GetPeople(txtSearch.Text);

            //CALL REFRESH BINDING FUNCTION
            RefreshListBoxBinding();

            //CLEAR EMAIL LISTBOX
            lstEmails.ItemsSource = "";
          
        }//end method

        private void lstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            //INTIALIZE VARIABLE TO SELECTED INDEX
            int indexClicked = lstResults.SelectedIndex;           

           
            //CHECK LISTBOX SELECTION
            if (indexClicked != -1)
            {
                //INTIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
                int idIndex = people_results[indexClicked].id;

                //CREATE DATA ACCESS INSTANCE
                DataAccess database_data = new DataAccess();
                //USE DATA ACCESS INSTANCE TO GET ADDRESS DATA FOM THE DB FOR SELECTED PERSON
                address_results = database_data.GetAddress(idIndex);
                email_results = database_data.GetEmail(idIndex);

                //CALL EMAIL REFRESH BINDING FUNCTION
                RefreshListBoxBindingEmail();

                //SET TEXTBOX TO SELECTED DATA
                txtFirstName.Text = people_results[indexClicked].first_name;
                txtLastName.Text = people_results[indexClicked].last_name;
                txtCellNum.Text = people_results[indexClicked].cell_number;
                txtWorkNum.Text = people_results[indexClicked].work_number;
                txtNotes.Text = people_results[indexClicked].notes;
                txtAddress.Text = address_results[0].home_address;
                txtEmail.Text = "";

            
                //ENABLE BUTTONS
                btnDelete.IsEnabled = true;
                btnUpdate.IsEnabled = true;
            }
            else
            {
                //REST TEXT BOXES TO EMPTY
                txtFirstName.Text="";
                txtLastName.Text="";
                txtCellNum.Text="";
                txtWorkNum.Text="";
                txtEmail.Text="";
                txtAddress.Text="";
                txtNotes.Text="";

                //DISABLE BUTTONS
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
            }//end if       
        }//end method

        private void lstEmails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //INTIALIZE VARIABLE TO SELECTED INDEX
            int emailIndexClicked = lstEmails.SelectedIndex;

            //CHECK EMAIL LISTBOX SELECTION
            if (emailIndexClicked != -1)
            {

                //INTIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
                int emailIndex = email_results[emailIndexClicked].id;

                //SET ADDRESS TEXTBOX EQUAL TO RECORD STORED IN EMAIL RESULTS
                txtEmail.Text = email_results[emailIndexClicked].email;
            }//end if 
        }// end method

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //SET PROPERTIES OF PERSON INSTANCE EQUAL TO VALUE OF TEXT BOXES
            newPerson.first_name = txtFirstName.Text;
            newPerson.last_name = txtLastName.Text;
            newPerson.cell_number = txtCellNum.Text;
            newPerson.work_number = txtWorkNum.Text;
            newPerson.email = txtEmail.Text;
            newPerson.home_address = txtAddress.Text;
            newPerson.notes = txtNotes.Text;
            newPerson.active = true;
            
            if (txtFirstName.Text != "" || txtFirstName.Text != "" || txtCellNum.Text != "" || txtWorkNum.Text != "" || txtEmail.Text != "" || txtAddress.Text != "" || txtNotes.Text != "")
            {
                //CREATE DATA ACCESS INSTANCE
                DataAccess database_data = new DataAccess();

                //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB
                database_data.AddPerson(newPerson);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please enter information into field(s) or make a selection in list.");
            }//end if 
        }//end method

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //INITIALIZE VARIABLE TO SELECTED INDEX
            int indexClicked = lstResults.SelectedIndex;

            //INTIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
            int idIndex = people_results[indexClicked].id;

            //SET PROPERTIES OF PERSON INSTANCE EQUAL TO VALUE OF TEXT BOXES
            newPerson.first_name = txtFirstName.Text;
            newPerson.last_name = txtLastName.Text;
            newPerson.cell_number = txtCellNum.Text;
            newPerson.work_number = txtWorkNum.Text;
            newPerson.email = txtEmail.Text;
            newPerson.home_address = txtAddress.Text;
            newPerson.notes = txtNotes.Text;
             
      
            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

            //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB AND ARCHIVE IT
            database_data.ArchivePerson(newPerson,idIndex);

            //USE DATA ACCESS INSTANCE TO UPDATE PEOPLE DATA FOM THE DB
            people_results = database_data.GetPeople(txtSearch.Text);

            //CALL REFRESH BINGING FUNCTION
            RefreshListBoxBinding();
        }//end method

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //INITIALIZE VARIABLE TO SELECTED INDEX
            int indexClicked = lstResults.SelectedIndex;
            //INTIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
            int idIndex = people_results[indexClicked].id;


            //INTIALIZE VARIABLE TO SELECTED INDEX
            int emailIndexClicked = lstEmails.SelectedIndex;

            //SET PROPERTIES OF PERSON INSTANCE EQUAL TO VALUE OF TEXT BOXES
            newPerson.first_name = txtFirstName.Text;
            newPerson.last_name = txtLastName.Text;
            newPerson.cell_number = txtCellNum.Text;
            newPerson.work_number = txtWorkNum.Text;
            newPerson.email = txtEmail.Text;
            newPerson.home_address = txtAddress.Text;
            newPerson.notes = txtNotes.Text;

            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

           int emailIndex = -2;
            
            //CHECK LISTBOX SELECTION
            if (emailIndexClicked != -1)
            {  
                //INTIALIZE VARIABLE TO EMAIL SELECTED INDEX ID NUMBER
                emailIndex = email_results[emailIndexClicked].id; 
            }//end if 
            //NO LISTBOX SELECTION
            if(emailIndexClicked == -1)
            {
                //INITALIZE VARIABLE TO EMAIL RESULTS ID
                emailIndex = email_results[0].id;
            }//end if 

            //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB
            database_data.UpdatePerson(newPerson, idIndex, emailIndex);

            //USE DATA ACCESS INSTANCE TO UPDATE PEOPLE DATA FOM THE DB
            people_results = database_data.GetPeople(txtSearch.Text);

            //CALL REFRESH BINGING FUNCTION
            RefreshListBoxBinding();
        }//end method

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //SHOW NEW SUBWINDOW ONCLICK
            SubWindow MySubWindow = new SubWindow();
            MySubWindow.ShowDialog();
        }//end method 

       
    }//end class
}//end namespace
