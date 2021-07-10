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
    public class BillItem: PrimaryDalItem
    {
        #region private variables
        private long _lngId;
        private long _lngCustomerId;
        private DateTime _dtBillDate;
        private float _fltBillAmount;
        private float _fltPaidAmount;
        private DateTime? _dtPaidDate;
        //PrivateVariables Next
        #endregion

        #region public properties
        public long Id
        {
            get { return _lngId; }
            set { _lngId = value; }
        }
        public long CustomerId
        {
            get { return _lngCustomerId; }
            set { _lngCustomerId = value; }
        }
        public DateTime BillDate
        {
            get { return _dtBillDate; }
            set { _dtBillDate = value; }
        }
        public float BillAmount
        {
            get { return _fltBillAmount; }
            set { _fltBillAmount = value; }
        }
        public float PaidAmount
        {
            get { return _fltPaidAmount; }
            set { _fltPaidAmount = value; }
        }
        public DateTime? PaidDate
        {
            get { return _dtPaidDate; }
            set { _dtPaidDate = value; }
        }
        //PublicProperties Next
        #endregion

        #region public enums
        public enum SaveOption
        {
            SaveRow = 1,
            MarkAsPaid = 2
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
        public BillItem()
        {
            base.ClassName = "BillItem";
        }
        #endregion

        #region save code
        public void Save(SaveOption sOption)
        {
            try
            {
                switch (sOption)
                {
                    case SaveOption.SaveRow:
                        SqlCommand cmd = getSaveRowCmd(sOption);
                        this.Id = SaveItem(cmd, true);
                        break;
                    case SaveOption.MarkAsPaid:
                        SqlCommand markAsPaidCmd = getSaveRowCmd(sOption);
                        this.Id = SaveItem(markAsPaidCmd, true);
                        break;
                    //SaveCase Next1
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        private SqlCommand getSaveRowCmd(SaveOption sOption)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spSetBill");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@SaveOption", SqlDbType.Int);
                param1.Value = sOption;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@Id", SqlDbType.Int);
                param2.Value = this.@Id;
                cmd.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter("@CustomerId", SqlDbType.Int);
                param3.Value = this.@CustomerId;
                cmd.Parameters.Add(param3);

                SqlParameter param4 = new SqlParameter("@BillDate", SqlDbType.DateTime);
                param4.Value = this.@BillDate.Equals(DateTime.Parse("1/1/0001")) ? DateTime.Parse("1/1/1900") : this.BillDate;
                cmd.Parameters.Add(param4);

                SqlParameter param5 = new SqlParameter("@BillAmount", SqlDbType.Float);
                param5.Value = this.@BillAmount;
                cmd.Parameters.Add(param5);

                SqlParameter param6 = new SqlParameter("@PaidAmount", SqlDbType.Float);
                param6.Value = this.@PaidAmount;
                cmd.Parameters.Add(param6);

                SqlParameter param7 = new SqlParameter("@PaidDate", SqlDbType.DateTime);
                param7.Value = this.@PaidDate;
                cmd.Parameters.Add(param7);

                //CmdParameters Next
                SqlParameter paramIdentityValue = new SqlParameter("@IdentityValue", SqlDbType.Int, 0, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, null);
                cmd.Parameters.Add(paramIdentityValue);

                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //SaveCommand Next
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
                    this.BillAmount = ar.Contains("BillAmount") ? (dr["BillAmount"].Equals(System.DBNull.Value) ? this.BillAmount : float.Parse(dr["BillAmount"].ToString())) : this.BillAmount;
                    this.BillDate = ar.Contains("BillDate") ? (dr["BillDate"].Equals(System.DBNull.Value) ? this.BillDate : DateTime.Parse(dr["BillDate"].ToString())) : this.BillDate;
                    this.CustomerId = ar.Contains("CustomerId") ? (dr["CustomerId"].Equals(System.DBNull.Value) ? this.CustomerId : long.Parse(dr["CustomerId"].ToString())) : this.CustomerId;
                    this.Id = ar.Contains("Id") ? (dr["Id"].Equals(System.DBNull.Value) ? this.Id : long.Parse(dr["Id"].ToString())) : this.Id;
                    this.PaidAmount = ar.Contains("PaidAmount") ? (dr["PaidAmount"].Equals(System.DBNull.Value) ? this.PaidAmount : float.Parse(dr["PaidAmount"].ToString())) : this.PaidAmount;
                    this.PaidDate = ar.Contains("PaidDate") ? (dr["PaidDate"].Equals(System.DBNull.Value) ? this.PaidDate : DateTime.Parse(dr["PaidDate"].ToString())) : this.PaidDate;
                   
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
                SqlCommand cmd = new SqlCommand("spGetBill");
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
