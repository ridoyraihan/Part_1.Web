using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Part_1.PrimaryDal
{
    public class PrimaryDalItem
    {
        #region private variables
        private string _strClassName;
        private string _strAppName;
        private bool _blnFound;
        #endregion

        #region public properties
        public string ClassName
        {
            get { return _strClassName; }
            set { _strClassName = value; }
        }
        public string AppName
        {
            get { return _strAppName; }
            set { _strAppName = value; }
        }
        public bool Found
        {
            get { return _blnFound; }
            set { _blnFound = value; }
        }
        #endregion

        #region public constructors
        public PrimaryDalItem()
        {
        }
        #endregion

        #region save code
        protected long SaveItem(SqlCommand cmd, bool returnId)
        {
            try
            {
                AppSettings app = new AppSettings();
                app.LoadSettings();
                SqlConnection cnn = new SqlConnection(app.DBCnnString);
                cnn.Open();
                cmd.Connection = cnn;
                //execute the query
                cmd.ExecuteNonQuery();
                long id;
                if (returnId)
                {
                    id = cmd.Parameters["@IdentityValue"].Value == DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["@IdentityValue"].Value);
                }
                else
                {
                    id = 0;
                }
                //close objects
                cnn.Close();
                cmd.Dispose();
                cmd = null;
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region datasets
        public virtual DataSet GetDataSet(object loption)
        {
            return null;
        }
        #endregion
    }
}
