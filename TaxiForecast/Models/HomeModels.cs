using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TaxiForecast.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace TaxiForecast.Models
{
    public class HomeModels
    {
        public static SqlConnection getConnectionString()
        {
            SqlConnection sqlcon = new SqlConnection("Data Source=104.197.17.237;Initial Catalog=ReportServer;uid=sa;pwd=owater@2016");
            return sqlcon;
        }
        public static List<Ranking> GetRanking(int rankingID)
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            List<Ranking> list = new List<Ranking>();
            Ranking ls = null;
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_GetRanking", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    sqlcmd.Parameters.Add("@rankingID", SqlDbType.Int).Value = rankingID;
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        ls = new Ranking();
                        ls.id = (sqldr["id"] == null) ? 0 : int.Parse(sqldr["id"].ToString());
                        ls.leftLat = (sqldr["leftLat"] == null) ? "" : sqldr["leftLat"].ToString();
                        ls.leftLong = (sqldr["leftLong"] == null) ? "" : sqldr["leftLong"].ToString();
                        ls.rightDownLat = (sqldr["rightDownLat"] == null) ? "" : sqldr["rightDownLat"].ToString();
                        ls.rightDownLong = (sqldr["rightDownLong"] == null) ? "" : sqldr["rightDownLong"].ToString();
                        ls.point = (sqldr["point"] == null) ? 0 : double.Parse(sqldr["point"].ToString());
                        ls.type = (sqldr["type"] == null) ? "-" : sqldr["type"].ToString();
                        ls.inNum = (sqldr["inNum"] == null) ? -1 : int.Parse(sqldr["inNum"].ToString());
                        ls.outNum = (sqldr["outNum"] == null) ? -1 : int.Parse(sqldr["outNum"].ToString());
                        list.Add(ls);
                    }
                    sqldr.Close();
                    sqlcon.Close();
                }
            }
            catch { }
            return list;
        }
        public static List<Cars> GetCarLocations()
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            List<Cars> list = new List<Cars>();
            Cars ls = null;
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_GetCarLoactions", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        ls = new Cars();
                        ls.id = (sqldr["_carID"] == null) ? 0 : int.Parse(sqldr["_carID"].ToString());
                        ls.latlng = (sqldr["_latlng"] == null) ? "" : sqldr["_latlng"].ToString();
                        ls.status = (sqldr["_status"] == null) ? 0 : int.Parse(sqldr["_status"].ToString());
                        ls.timeStamp = (sqldr["_timeStamp"] == null) ? DateTime.Now : Convert.ToDateTime(sqldr["_timeStamp"].ToString());
                        list.Add(ls);
                    }
                    sqldr.Close();
                    sqlcon.Close();
                }
            }
            catch { }
            return list;
        }
        public static List<Cars1> GetCarLocations1()
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            List<Cars1> list = new List<Cars1>();
            Cars1 ls = null;
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_CarLoactions1", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        ls = new Cars1();
                        ls._id = (sqldr["_id"] == null) ? "" : sqldr["_id"].ToString();
                        ls.phone = (sqldr["phone"] == null) ? "" : sqldr["phone"].ToString();
                        ls.time = (sqldr["time"] == null) ? "" : sqldr["time"].ToString();
                        ls.lat = (sqldr["lat"] == null) ? "" : sqldr["lat"].ToString();
                        ls.lng = (sqldr["lng"] == null) ? "" : sqldr["lng"].ToString();
                        ls.state = (sqldr["state"] == null) ? 0 : int.Parse(sqldr["state"].ToString());
                        ls.direction = (sqldr["direction"] == null) ? 0 : int.Parse(sqldr["direction"].ToString());
                        list.Add(ls);
                    }
                    sqldr.Close();
                    sqlcon.Close();
                }
            }
            catch { }
            return list;
        }
        public static List<ScordBoard> getScoreBoard()
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            List<ScordBoard> list = new List<ScordBoard>();
            ScordBoard ls = null;
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_getScoreBoard", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    int count = 0;
                    while (sqldr.Read())
                    {
                        count++;
                        ls = new ScordBoard();
                        ls.id = (sqldr["id"] == null) ? 0 : int.Parse(sqldr["id"].ToString());
                        ls.order = (sqldr["order"] == null) ? count : count;
                        ls.latLng = (sqldr["latLng"] == null) ? "" : sqldr["latLng"].ToString();
                        ls.address = (sqldr["address"] == null) ? "" : sqldr["address"].ToString();
                        list.Add(ls);
                    }
                    sqldr.Close();
                    sqlcon.Close();
                }
            }
            catch { }
            return list;
        }
        public static User checkLogin(string username, string password)
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            User li = new User();
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_checkLogin", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    sqlcmd.Parameters.Add("@username", SqlDbType.NVarChar, 150).Value = username.Trim();
                    if(username!="admin@haui.vn")
                        sqlcmd.Parameters.Add("@password", SqlDbType.NVarChar, 150).Value = password;
                    else
                        sqlcmd.Parameters.Add("@password", SqlDbType.NVarChar, 150).Value = MD5Hash(password.Trim().Substring(0, password.Length - 1));
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        li.username = sqldr["username"].ToString();
                        li.password = sqldr["password"].ToString();
                        li.role = sqldr["role"].ToString();
                    }
                    sqldr.Close();
                    sqlcon.Close();
                }
            }
            catch { }
            //For dev
            //li.username = "1";
            //li.password = "1";
            //li.role = "1";
            //\\

            return li;
        }
        public static Driver getDriverInfo(string carID)
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            Driver li = new Driver();
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_getDriverInfo", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    sqlcmd.Parameters.Add("@carID", SqlDbType.NVarChar, 150).Value = carID.Trim();
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        li.fullName = sqldr["fullName"].ToString();
                        li.phoneNumber = sqldr["phoneNumber"].ToString();
                        li.avartar = sqldr["avartar"].ToString();
                    }
                    sqldr.Close();
                    sqlcon.Close();
                }
            }
            catch { }
            return li;
        }
        public static string getStatusCheckingTbl()
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_getStatusChecking", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        return sqldr["status"].ToString();
                    }
                    sqldr.Close();
                    sqlcon.Close();
                }
            }
            catch {
                return "False";
            }
            return "False";
        }
        public static bool insertCheckingTbl(string sqlInsertText)
        {
            SqlConnection sqlcon;
            SqlCommand sqlcmd;
            try
            {
                using (sqlcon = getConnectionString())
                {
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("stor_insertChecking", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 180;
                    sqlcmd.Parameters.Add("@sqlInsertText", SqlDbType.NVarChar).Value = sqlInsertText.Trim();
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    sqldr.Close();
                    sqlcon.Close();
                }
                return true;
            }
            catch {
                return false;
            }
        }
        public static string MD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString(hashBytes[i].ToString()));
            }
            return sb.ToString();
        }
        public class Ranking
        {
            public int id { get; set; }
            public string leftLat { get; set; }
            public string leftLong { get; set; }
            public string rightDownLat { get; set; }
            public string rightDownLong { get; set; }
            public double point { get; set; }
            public string type { get; set; }
            public int inNum { get; set; }
            public int outNum { get; set; }
        }
        public class User
        {
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string role { get; set; }
        }
        public class Cars
        {
            public int id { get; set; }
            public int idType { get; set; }
            public string latlng { get; set; }
            public DateTime timeStamp { get; set; }
            public int status { get; set; }
        }
        public class Cars1
        {
            public string _id { get; set; }
            public int radioId { get; set; }
            public string phone { get; set; }
            public string time { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public int state { get; set; }
            public int direction { get; set; }
        }
        public class Driver
        {
            public string carID { get; set; }
            public string fullName { get; set; }
            public string phoneNumber { get; set; }
            public string avartar { get; set; }
        }
        public class ScordBoard
        {
            public int id { get; set; }
            public int order { get; set; }
            public string latLng { get; set; }
            public string address { get; set; }
        }
        ////-------------
        
        public class PointLatLng
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
        public class MapView
        {
            
            public PointLatLng leftDownPoint { get; set; }
            public PointLatLng leftTopPoint { get; set; }
            public PointLatLng rightDownPoint { get; set; }
            public PointLatLng rightTopPoint { get; set; }
            public PointLatLng centerPoint { get; set; }
        }
        public class Cells
        {
            public int id { get; set; }
            public PointLatLng leftDownPoint { get; set; }
            public PointLatLng leftTopPoint { get; set; }
            public PointLatLng rightDownPoint { get; set; }
            public PointLatLng rightTopPoint { get; set; }
            public PointLatLng centerPoint { get; set; }
            public string color { get; set; }
            public int score { get; set; }
            public string ArrDesc { get; set; }
        }
    }
}