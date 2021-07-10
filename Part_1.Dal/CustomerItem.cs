using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Part_1.PrimaryDal;

namespace Part_1.Dal
{
    public class CustomerItem: PrimaryDalItem
    {
        #region private variables
        private long _lngId;
        private string _strName;
        //PrivateVariables Next
        #endregion

        #region public properties
        public long Id
        {
            get { return _lngId; }
            set { _lngId = value; }
        }
        public string Name
        {
            get { return _strName; }
            set { _strName = value; }
        }
        //PublicProperties Next
        #endregion

        #region public enums
        public enum SaveOption
        {
            SaveRow = 1,
            //SaveOption Next
        }
        public enum LoadOption
        {
            LoadAll = 1,
            LoadById = 2
            //LoadOption Next
        }
        #endregion

        #region constructors
        public CustomerItem()
        {
            base.ClassName = "CustomerItem";
        }
        #endregion

        #region load code
        public void Load(LoadOption lOption)
        {
            try
            {
                this.Found = false;
                SqlDataReader dr;
                AppSettings app = new AppSettings();
                app.LoadSettings();
                SqlConnection cnn = new SqlConnection(app.DBCnnString);
                SqlCommand cmd = getLoadCmd(lOption);
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    System.Collections.ArrayList ar = GetColumnList(dr);
                    this.Found = true;
                    this.Id = ar.Contains("Id") ? (dr["Id"].Equals(System.DBNull.Value) ? this.Id : long.Parse(dr["Id"].ToString())) : this.Id;
                    this.Name = ar.Contains("Name") ? (dr["Name"].Equals(System.DBNull.Value) ? this.Name : dr["Name"].ToString()) : this.Name;

                    //DataReader NextItem
                }

                dr.Close();
                dr = null;
                cnn.Close();
                cmd = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static System.Collections.ArrayList GetColumnList(SqlDataReader dr)
        {
            System.Collections.ArrayList ar = new System.Collections.ArrayList();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                ar.Add(dr.GetName(i));
            }

            return ar;
        }

        private SqlCommand getLoadCmd(LoadOption lOption)
        {
            switch (lOption)
            {
                case LoadOption.LoadById:
                    return getLoadByIdCmd(lOption);

                //LoadCase Next
                default:
                    return null;
            }
        }

        private SqlCommand getLoadByIdCmd(LoadOption lOption)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetCustomer");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@QryOption", SqlDbType.Int);
                param1.Value = lOption;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@Id", SqlDbType.Int);
                param2.Value = this.Id;
                cmd.Parameters.Add(param2);

                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //LoadCommand Next
        #endregion
    }
}
