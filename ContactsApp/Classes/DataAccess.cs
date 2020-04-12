using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ContactsApp
{
    class DataAccess
    {

        public List<Person> GetPeople(string term)
        {
            List<Person> lst_return = new List<Person>();

            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY SELECT PERSONS
            string queryStatement = "SELECT * FROM Persons WHERE last_name LIKE '%" + term + "%' OR first_name LIKE '%" + term + "%' ;";
            
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            SqlDataReader data_reader = sql_command.ExecuteReader();

            //READ DATA RETURNED
            while (data_reader.Read())
            {
                //ARRAY FOR DATA IN CURRENT ROW
                object[] records = new object[7];

                //NEW PERSON INSTANCE
                Person new_person = new Person();

                //POPLATE DATA DEOM DATA READER INTO ARAY
                data_reader.GetValues(records);

                //SET PROPERTIES OF PERSON INSTANCE
                new_person.id = Convert.ToInt32(records[0]);
                new_person.first_name = records[1].ToString();
                new_person.last_name = records[2].ToString();
                new_person.cell_number = records[3].ToString();
                new_person.work_number = records[4].ToString();            
                new_person.notes = records[5].ToString();
                new_person.active = Convert.ToBoolean(records[6]);

                //CHECK TO MAKE SURE PERSON IS ACTIVE
                if (new_person.active == true)
                { 
                    //ADD TO LIST
                    lst_return.Add(new_person);
                }//end if 
            }//end while

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

            //RETURN LIST OF PERSONS
            return lst_return;
        }//end method

        public List<Person> GetAddress(int idIndex)
        {
            List<Person> lst_address_return = new List<Person>();

            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY SELECT ADDRESS
            string queryStatement = $"SELECT * FROM Address Where person_id = {idIndex};"; 
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            SqlDataReader data_reader = sql_command.ExecuteReader();

            //READ DATA RETURNED
            while (data_reader.Read())
            {
                //ARRAY FOR DATA IN CURRENT ROW
                object[] records = new object[3];

                //NEW PERSON INSTANCE
                Person new_person = new Person();

                //POPLATE DATA DEOM DATA READER INTO ARAY
                data_reader.GetValues(records);

                //SET PROPERTIES OF PERSON INSTANCE
                new_person.id = Convert.ToInt32(records[0]);
                new_person.person_id = Convert.ToInt32(records[1]);
                new_person.home_address = records[2].ToString();
                
                //ADD TO LIST
                lst_address_return.Add(new_person);               
            }//end while

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

            //RETURN LIST OF PERSONS
            return lst_address_return;
        }//end method


        public List<Person> GetEmail(int idIndex)
        {
            List<Person> lst_email_return = new List<Person>();

            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY SELECT EMAIL
            string queryStatement = $"SELECT * FROM Email Where person_id = {idIndex};";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            SqlDataReader data_reader = sql_command.ExecuteReader();

            //READ DATA RETURNED
            while (data_reader.Read())
            {
                //ARRAY FOR DATA IN CURRENT ROW
                object[] records = new object[3];

                //NEW PERSON INSTANCE
                Person new_person = new Person();

                //POPLATE DATA DEOM DATA READER INTO ARAY
                data_reader.GetValues(records);

                //SET PROPERTIES OF PERSON INSTANCE
                new_person.id = Convert.ToInt32(records[0]);
                new_person.person_id = Convert.ToInt32(records[1]);
                new_person.email= records[2].ToString();

                //ADD TO LIST
                lst_email_return.Add(new_person);
            }//end while

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

            //RETURN LIST OF PERSONS
            return lst_email_return;
        }//end method

        public void DeletePerson(Person newPerson, int idIndex)
        {
            // GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY DELETE PERSONS
            string queryStatement = $"DELETE FROM Persons WHERE id = '{idIndex}'";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUER
            sql_command.ExecuteNonQuery();

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

        }//end method

        public void AddPerson(Person newPerson)
        {
            // GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY WITH SCOPE IDENTITY TO GET LAST INSERTED RECORD
            string queryStatement = $"INSERT INTO Persons (first_name, last_name, cell_number, work_number,  notes, active) VALUES ('{newPerson.first_name}', '{newPerson.last_name}', '{newPerson.cell_number}', '{newPerson.work_number}'," +
                $" '{newPerson.notes}' ,'{newPerson.active}'); SELECT SCOPE_IDENTITY() ";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //INTIALIZE VARIABLE TO LAST RECORD ID
            int lastRecordId = Convert.ToInt32(sql_command.ExecuteScalar());

            //BUILD QUERY INSERT ADDRESS
            queryStatement = $"INSERT INTO Address (home_address,person_id) VALUES ('{newPerson.home_address }',{lastRecordId})";
             sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            //BUILD QUERY INSERT EMAIL
            queryStatement = $"INSERT INTO Email (email,person_id) VALUES ('{newPerson.email }',{lastRecordId})";
            sql_command = new SqlCommand(queryStatement, sql_connection);


            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();
        }//end method

        public void UpdatePerson(Person newPerson, int idIndex, int emailIndex)
        {

            // GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY UPDATE PERSONS
            string queryStatement = $"UPDATE Persons SET first_name = '{newPerson.first_name}', last_name = '{newPerson.last_name}', cell_number = '{newPerson.cell_number}', work_number = '{newPerson.work_number}', notes = '{newPerson.notes}' WHERE id LIKE '{idIndex}'";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            //BUILD QUERY UPDATE ADDRESS
            queryStatement = $"Update Address SET home_address = '{newPerson.home_address}' WHERE person_id LIKE '{idIndex}';";
            sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            //BUILD QUERY UPDATE EMAIL
            queryStatement = $"Update Email SET email = '{newPerson.email}' WHERE id LIKE '{emailIndex}';";
            sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

        }//end method

        public void ArchivePerson(Person newPerson,int idIndex)
        {
            // GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
           
            //BUILD QUERY
            string queryStatement = $"UPDATE Persons SET active = 'False' WHERE id LIKE '{idIndex}'";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUER
            sql_command.ExecuteNonQuery();

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

        }//end method

        public List<Person> GrabArchivedPersonData(Person newPerson)
        {
            List<Person> lst_return = new List<Person>();

            // GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY
            string queryStatement = $"SELECT * from Persons WHERE active = 'False'";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            SqlDataReader data_reader = sql_command.ExecuteReader();

            //READ DATA RETURNED
            while (data_reader.Read())
            {
                //ARRAY FOR DATA IN CURRENT ROW
                object[] records = new object[7];

                //NEW PERSON INSTANCE
                Person new_person = new Person();

                //POPLATE DATA DEOM DATA READER INTO ARAY
                data_reader.GetValues(records);


                //SET PROPERTIES OF PERSON INSTANCE
                new_person.id = Convert.ToInt32(records[0]);
                new_person.first_name = records[1].ToString();
                new_person.last_name = records[2].ToString();
                new_person.cell_number = records[3].ToString();
                new_person.work_number = records[4].ToString();
                //new_person.email = records[5].ToString();
                //new_person.home_address = records[6].ToString();
                new_person.notes = records[5].ToString();
                new_person.active = Convert.ToBoolean(records[6]);

                //ADD TO LIST
                lst_return.Add(new_person);
            }//end while

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

            //RETURN LIST OF PERSONS
            return lst_return;

        }//end method

        public void RestorePerson(Person newPerson,int idIndex)
        {
            // GET CONNECTION STRING FROM HELPER CLASS
            string connectionString = Helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY
            string queryStatement = $"UPDATE Persons SET active = 'True' WHERE id LIKE '{idIndex}'";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUER
            sql_command.ExecuteNonQuery();

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

        }//end method
    }
}
