using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Sensor.AccelGyro
{
    public class SNO_MPU6050_Record
    {
        #region [ Field Name ]
        public string _AutoKey = "";
        public string _Address = "";
        public readonly DateTime? _TimeDate = null;
        public float _GyroX = 0;
        public float _GyroY= 0;
        public float _GyroZ = 0;
        public float _AccelX = 0;
        public float _AccelY = 0;
        public float _AccelZ = 0;
        private string _Message = "";   
        #endregion

        public SNO_MPU6050_Record(string AutoKey)
        {
            string zSQL = "SELECT * FROM [dbo].[SNO_MPU6050] WHERE AutoKey = @Autokey";
            string zConnectionString = TNS_DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(AutoKey);
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    _AutoKey = zReader["AutoKey"].ToString();
                    _Address = zReader["Address"].ToString();

                    
                    _GyroX = float.Parse(zReader["Gyro_X"].ToString());
                    _GyroY = float.Parse(zReader["Gyro_Y"].ToString());
                    _GyroZ = float.Parse(zReader["Gyro_Z"].ToString());
                    
                    _AccelX = float.Parse(zReader["Accel_X"].ToString());
                    _AccelY = float.Parse(zReader["Accel_Y"].ToString());
                    _AccelZ = float.Parse(zReader["Accel_Z"].ToString());
                    
                    if (zReader["TimeDate"] != DBNull.Value)
                        _TimeDate = (DateTime)zReader["TimeDate"];
                    else
                        _Message = "OK";
                }
                else
                {
                    _Message = "404 Not Found";
                }

                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
        }
    }
}
