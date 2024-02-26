using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TNS.Sensor.AccelGyro
{
    public class SNO_MPU6050_AccessData
    {
        public static DataTable List()
        {
            DataTable zTable = new DataTable();
            string zSQL = "exec SP_SNPO_MPU6050_Search null,null,null,null";
            string zConnectionString = TNS_DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            return zTable;
        }
        public static DataTable Search(string fromDate, string toDate)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [TNS_VietTech].[dbo].[SNO_MPU6050] WHERE TimeDate BETWEEN @TuNgay AND @DenNgay";
            string zConnectionString = TNS_DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                using (SqlConnection zConnect = new SqlConnection(zConnectionString))
                {
                    zConnect.Open();
                    SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                    zCommand.CommandType = CommandType.Text;
                    zCommand.Parameters.Add("@TuNgay", SqlDbType.DateTime).Value = DateTime.Parse(fromDate);
                    zCommand.Parameters.Add("@DenNgay", SqlDbType.DateTime).Value = DateTime.Parse(toDate);
                    SqlDataAdapter adapter = new SqlDataAdapter(zCommand);
                    adapter.Fill(zTable);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
          
            return zTable;
        }
        public static DataTable Search2(string Date)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [TNS_VietTech].[dbo].[SNO_MPU6050] WHERE TimeDate = @TimeDate ";
            string zConnectionString = TNS_DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                using (SqlConnection zConnect = new SqlConnection(zConnectionString))
                {
                    zConnect.Open();
                    SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                    zCommand.CommandType = CommandType.Text;
                    zCommand.Parameters.Add("@TimeDate", SqlDbType.DateTime).Value = DateTime.Parse(Date);
                    SqlDataAdapter adapter = new SqlDataAdapter(zCommand);
                    adapter.Fill(zTable);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return zTable;
        }
    }
}
