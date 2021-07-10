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
    public class BillList: PrimaryDalList
    {
        #region private variables
        private BillItem _billItem;
        #endregion

        #region public properties
        public BillItem Item
        {
            set { this._billItem = value; }
            get { return this._billItem; }
        }
        #endregion

        #region public enums
        public enum LoadOption
        {
            LoadAll = 1,
            LoadById = 2,
            LoadByOutstandingBill = 3
            //LoadOption Next
        }
        #endregion

        #region constructors
        public BillList()
        {
            base.ClassName = "BillList";
            this._billItem = new BillItem();
        }
        #endregion

        #region override public functions
        public override DataSet GetDataSet(Object lOption)
        {
            AppSettings app = new AppSettings();
            app.LoadSettings();
            SqlConnection cnn = new SqlConnection(app.DBCnnString);
            SqlCommand cmd = this.getLoadCmd((LoadOption)lOption);
            cmd.Connection = cnn;
            cnn.Open();

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds, "tbl");
            cnn.Close();
            return ds;
        }
        #endregion

        #region load code
        public void Load(LoadOption lOption)
        {
            try
            {
                SqlDataReader dr;
                AppSettings app = new AppSettings();
                app.LoadSettings();
                SqlConnection cnn = new SqlConnection(app.DBCnnString);
                SqlCommand cmd = getLoadCmd(lOption);
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                BillItem item;

                while (dr.Read())
                {
                    System.Collections.ArrayList ar = GetColumnList(dr);
                    item = new BillItem();
                    item.BillAmount = ar.Contains("BillAmount") ? (dr["BillAmount"].Equals(System.DBNull.Value) ? item.BillAmount : float.Parse(dr["BillAmount"].ToString())) : item.BillAmount;
                    item.BillDate = ar.Contains("BillDate") ? (dr["BillDate"].Equals(System.DBNull.Value) ? item.BillDate : DateTime.Parse(dr["BillDate"].ToString())) : item.BillDate;
                    item.CustomerId = ar.Contains("CustomerId") ? (dr["CustomerId"].Equals(System.DBNull.Value) ? item.CustomerId : long.Parse(dr["CustomerId"].ToString())) : item.CustomerId;
                    item.Id = ar.Contains("Id") ? (dr["Id"].Equals(System.DBNull.Value) ? item.Id : long.Parse(dr["Id"].ToString())) : item.Id;
                    item.PaidAmount = ar.Contains("PaidAmount") ? (dr["PaidAmount"].Equals(System.DBNull.Value) ? item.PaidAmount : float.Parse(dr["PaidAmount"].ToString())) : item.PaidAmount;                    
                    item.PaidDate = ar.Contains("PaidDate") ? (dr["PaidDate"].Equals(System.DBNull.Value) ? item.PaidDate : DateTime.Parse(dr["PaidDate"].ToString())) : item.PaidDate;

                    //DataReader NextItem
                    base.Add(item);
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
                case LoadOption.LoadAll:
                    return getLoadAllCmd(lOption);
                case LoadOption.LoadByOutstandingBill:
                    return getLoadOutstandingBillsCmd(lOption);

                //LoadCase Next
                default:
                    return null;
            }
        }

        private SqlCommand getLoadByIdCmd(LoadOption lOption)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetBill");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@QryOption", SqlDbType.Int);
                param1.Value = lOption;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@Id", SqlDbType.Int);
                param2.Value = this.Item.Id;
                cmd.Parameters.Add(param2);

                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private SqlCommand getLoadAllCmd(LoadOption lOption)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetBill");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@QryOption", SqlDbType.Int);
                param1.Value = lOption;
                cmd.Parameters.Add(param1);

                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private SqlCommand getLoadOutstandingBillsCmd(LoadOption lOption)
        {
            return this.getLoadAllCmd(lOption);
        }
        //LoadCommand Next
        #endregion
    }
}
