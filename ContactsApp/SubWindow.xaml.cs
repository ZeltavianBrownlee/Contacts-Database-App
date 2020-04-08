using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContactsApp
{
    /// <summary>
    /// Interaction logic for SubWindow.xaml
    /// </summary>
    public partial class SubWindow : Window
    {
        List<Person> people_results = new List<Person>();

        public SubWindow()
        {
            InitializeComponent();

            //CALL ADD COMBO ITEMS CONSTUCTOR
            AddCbItems();           
        }//end constructor

        private void AddCbItems()
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

            //USE DATA ACCESS INSTANCE TO GET ARCHIVED PEOPLE DATA FOM THE DB
            people_results = database_data.GrabArchivedPersonData(newPerson);

            //GO THROUGH EACH PERSON IN PEOPLE RESULTS LIST
            foreach (Person element in people_results)
            {
                //ADD CHECKBOX AND PEOPLE NAMES TO COMBO BOX
                CmbResults.Items.Add(new System.Windows.Controls.CheckBox() { Content = element.ResultData, Tag = element.id});               
            }//end loop
        }//end constructor

        private void BtnRestoreSelected_Click(object sender, RoutedEventArgs e)
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

            //GO THROUGH EACH CHECKBOX IN COMBO BOX
            foreach (System.Windows.Controls.CheckBox current_checkbox in CmbResults.Items)
            {
                if(current_checkbox.IsChecked == true)
                {
                    //INITIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
                    int indexChecked = Convert.ToInt32(current_checkbox.Tag);

                    //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB AND ARCHIVE IT
                    database_data.RestorePerson(newPerson, indexChecked);
                }//end if 
            }//end loop

            //CLEAR COMBO BOX
            CmbResults.Items.Clear();

            //READD INACTIVE RECORDS
            AddCbItems();
        }//end fuction

        private void BtnRestoreAll_Click(object sender, RoutedEventArgs e)
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

            //GO THROUGH EACH CHECKBOX IN COMBO BOX
            foreach (System.Windows.Controls.CheckBox current_checkbox in CmbResults.Items)
            {
                current_checkbox.IsChecked = true;
                
                //INITIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
                int indexChecked = Convert.ToInt32(current_checkbox.Tag);

                //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB AND ARCHIVE IT
                database_data.RestorePerson(newPerson, indexChecked);               
            }//end loop

            //CLEAR COMBO BOX
            CmbResults.Items.Clear();

            //READD INACTIVE RECORDS
            AddCbItems();
        }//end fuction

        private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

            DialogResult result = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete the item(s)?", "Delete item(s) confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //GO THROUGH EACH CHECKBOX IN COMBO BOX
                foreach (System.Windows.Controls.CheckBox current_checkbox in CmbResults.Items)
                {
                    if (current_checkbox.IsChecked == true)
                    {
                        //INITIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
                        int indexChecked = Convert.ToInt32(current_checkbox.Tag);

                        //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB AND ARCHIVE IT
                        database_data.DeletePerson(newPerson, indexChecked);
                    }//end if
                }//end loop

                //CLEAR COMBO BOX
                CmbResults.Items.Clear();

                //READD INACTIVE RECORDS
                AddCbItems();
            }//end if 
        }//end function

        private void BtnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            //NEW PERSON INSTANCE
            Person newPerson = new Person();

            //CREATE DATA ACCESS INSTANCE
            DataAccess database_data = new DataAccess();

            DialogResult result = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete the item(s)?", "Delete item(s) confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //GO THROUGH EACH CHECKBOX IN COMBO BOX
                foreach (System.Windows.Controls.CheckBox current_checkbox in CmbResults.Items)
                {
                    current_checkbox.IsChecked = true;

                    //INITIALIZE VARIABLE TO SELECTED INDEX ID NUMBER
                    int indexChecked = Convert.ToInt32(current_checkbox.Tag);

                    //USE DATA ACCESS INSTANCE TO GET PEOPLE DATA FOM THE DB AND ARCHIVE IT
                    database_data.DeletePerson(newPerson, indexChecked);
                }//end loop

                //CLEAR COMBO BOX
                CmbResults.Items.Clear();

                //READD INACTIVE RECORDS
                AddCbItems();
            }//end if        
        }//end function
    }//end class
}//end namespace
