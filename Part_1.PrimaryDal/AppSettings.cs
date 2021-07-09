using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Part_1.PrimaryDal
{
    public class AppSettings
    {
		#region private variables
		private string _strDBCnnString;
		#endregion

		#region public properties
		public string DBCnnString
		{
			get { return _strDBCnnString; }
			set { _strDBCnnString = value; }
		}
		#endregion

		public AppSettings()
		{
		}

		public void LoadSettings()
		{
			try
			{
				this.DBCnnString = ConfigurationManager.ConnectionStrings["DbCnnString"].ToString();
			}
			catch (Exception e)
			{
				this.DBCnnString = ConfigurationManager.AppSettings["DbCnnString"].ToString();
			}
		}
	}
}
