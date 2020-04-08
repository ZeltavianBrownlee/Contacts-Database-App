using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsApp
{
    class Person
    {

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public string cell_number { get; set; }

        public string work_number { get; set; }

        public string home_address { get; set; }
        public string email { get; set; }

        public string notes { get; set; }

        public bool active { get; set; }

        public int person_id { get; set; }


        public string ResultData
        {
            get
            {
                return first_name + " " + last_name;
            }//end get
        }//end property

      


    }
}
