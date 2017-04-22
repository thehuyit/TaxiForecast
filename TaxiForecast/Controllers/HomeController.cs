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
using System.IO;
using System.Globalization;


namespace TaxiForecast.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                ViewBag.Message = "HaUI - Taxi Problem Forecast";
                ViewBag.carState1Num = 0;
                ViewBag.carState2Num = 0;
                ViewBag.carState3Num = 0;
                string polygonStr = "";
                int dem = 0;
                List<HomeModels.Ranking> rankingList = Models.HomeModels.GetRanking(0);
                dem = 0;
                foreach (HomeModels.Ranking i in rankingList)
                {
                    if (i.type == "out")
                    {
                        polygonStr += "var polygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.leftLong + ")], 'blue', 0.6, 0.6, 'blue', 0.4) ; map.addOverlay(polygon" + i.id + "); var areapolygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "),new GLatLng(" + i.leftLat + "," + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.leftLong + ")], '#f33f00', 5, 1, '#ff0000', 0.2); var areacenterpoint" + i.id + " = new GLatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng()); var myPoint_v3_" + i.id + " = new google.maps.LatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng()); GEvent.addListener(polygon" + i.id + ", 'click', function () {  map.openInfoWindow(myPoint_v3_" + i.id + ", '<b>Thông tin khu vực</b><br>Điểm: " + i.point + " <br>Kiểu: " + i.type + " <br> Số chuyến đón: " + i.inNum + " <br> Số chuyến trả: " + i.outNum + "'); });" + Environment.NewLine;
                        polygonStr += "polygonArray[" + dem + "] = polygon" + i.id + "" + Environment.NewLine; 
                    }
                    else
                    {
                        polygonStr += "var polygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.leftLong + ")], 'blue', 0.6, 0.6, 'red', 0.4);  map.addOverlay(polygon" + i.id + "); var areapolygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "),new GLatLng(" + i.leftLat + "," + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.leftLong + ")], '#f33f00', 5, 1, '#ff0000', 0.2); var areacenterpoint" + i.id + " = new GLatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng());  var myPoint_v3_" + i.id + " = new google.maps.LatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng());  GEvent.addListener(polygon" + i.id + ", 'click', function () { map.openInfoWindow(myPoint_v3_" + i.id + ", '<b>Thông tin khu vực</b><br>Điểm: " + i.point + " <br>Kiểu: " + i.type + " <br> Số chuyến đón: " + i.inNum + " <br> Số chuyến trả: " + i.outNum + "'); });" + Environment.NewLine;
                        polygonStr += "polygonArray[" + dem + "] = polygon" + i.id + "" + Environment.NewLine; 
                    }
                    dem++;
                }
                string carsStr = "";
                List<HomeModels.Cars> carList = HomeModels.GetCarLocations();
                dem = 0;
                foreach (HomeModels.Cars i in carList)
                {
                    carsStr += Environment.NewLine + "var car" + i.id + " = createMarker(new GLatLng(" + i.latlng + "),"+dem+",1,1,1);map.addOverlay(car" + i.id + "); ";
                    dem++;
                }

                string carsStr1 = "";
                List<HomeModels.Cars1> carList1 = HomeModels.GetCarLocations1();
                dem = 0;
                foreach (HomeModels.Cars1 i in carList1)
                {
                    carsStr1 += Environment.NewLine + "var car" + i._id + " = createMarker(new GLatLng(" + i.lat + "," + i.lng + ")," + dem + ","+i.state+",'"+i._id+"',1);map.addOverlay(car" + i._id + "); ";
                    if (i.state == 1)
                        ViewBag.carState1Num += 1;
                    if (i.state == 2)
                        ViewBag.carState2Num += 1;
                    if (i.state == 3)
                        ViewBag.carState3Num += 1;
                    dem++;
                }

                string JSdefine = @"
                            <script type='text/javascript'>;
                                var polygonArray = [];
                                var areapolygon = [];
                                var areacenterpoint = [];
                                var carArrayState1 = [];
                                var carArrayState2 = [];
                                var carArrayState3 = [];
                                var carArray = [];
                            </script>
                        ";
                string JSMapInitialize = @"
                            <script type='text/javascript'>
                                function initialize() {
                                    if (GBrowserIsCompatible()) {
                                        map = new GMap2(document.getElementById('map_canvas'));
                                        map.setCenter(new GLatLng(data[0].leftLat, data[0].leftLong), 13);
                                        " + polygonStr + @"
                                        " + carsStr1 + @"
                                        map.setUIToDefault();
                                    }
                                }
                            </script>
                            ";
                Response.Write(JSdefine);
                Response.Write("<script>var data = " + JsonConvert.SerializeObject(rankingList) + " </script>");
                Response.Write(JSMapInitialize);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult Member()
        {
            if (Session["username"] != null)
            {
                ViewBag.Message = "HaUI - Taxi Problem Forecast";
                ViewBag.carState1Num = 0;
                ViewBag.carState2Num = 0;
                ViewBag.carState3Num = 0;
                string polygonStr = "";
                int dem = 0;
                List<HomeModels.Ranking> rankingList = Models.HomeModels.GetRanking(0);
                dem = 0;
                foreach (HomeModels.Ranking i in rankingList)
                {
                    if (i.type == "out")
                    {
                        polygonStr += "var polygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.leftLong + ")], 'blue', 0.6, 0.6, 'blue', 0.4) ; map.addOverlay(polygon" + i.id + "); var areapolygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "),new GLatLng(" + i.leftLat + "," + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.leftLong + ")], '#f33f00', 5, 1, '#ff0000', 0.2); var areacenterpoint" + i.id + " = new GLatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng()); var myPoint_v3_" + i.id + " = new google.maps.LatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng()); GEvent.addListener(polygon" + i.id + ", 'mouseover', function () {  map.openInfoWindow(myPoint_v3_" + i.id + ", '<b>Thông tin khu vực</b><br>Điểm: " + i.point + " <br>Kiểu: " + i.type + " <br> Số chuyến đón: " + i.inNum + " <br> Số chuyến trả: " + i.outNum + "'); });" + Environment.NewLine;
                        polygonStr += "polygonArray[" + dem + "] = polygon" + i.id + "" + Environment.NewLine;
                    }
                    else
                    {
                        polygonStr += "var polygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.leftLong + ")], 'blue', 0.6, 0.6, 'red', 0.4);  map.addOverlay(polygon" + i.id + "); var areapolygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "),new GLatLng(" + i.leftLat + "," + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.leftLong + ")], '#f33f00', 5, 1, '#ff0000', 0.2); var areacenterpoint" + i.id + " = new GLatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng());  var myPoint_v3_" + i.id + " = new google.maps.LatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng());  GEvent.addListener(polygon" + i.id + ", 'mouseover', function () { map.openInfoWindow(myPoint_v3_" + i.id + ", '<b>Thông tin khu vực</b><br>Điểm: " + i.point + " <br>Kiểu: " + i.type + " <br> Số chuyến đón: " + i.inNum + " <br> Số chuyến trả: " + i.outNum + "'); });" + Environment.NewLine;
                        polygonStr += "polygonArray[" + dem + "] = polygon" + i.id + "" + Environment.NewLine;
                    }
                    dem++;
                }
                string carsStr = "";
                List<HomeModels.Cars> carList = HomeModels.GetCarLocations();
                dem = 0;
                foreach (HomeModels.Cars i in carList)
                {
                    carsStr += Environment.NewLine + "var car" + i.id + " = createMarker(new GLatLng(" + i.latlng + ")," + dem + ",1,1,1);map.addOverlay(car" + i.id + "); ";
                    dem++;
                }
                string carsStr1 = "";
                List<HomeModels.Cars1> carList1 = HomeModels.GetCarLocations1();
                dem = 0;
                foreach (HomeModels.Cars1 i in carList1)
                {
                    carsStr1 += Environment.NewLine + "var car" + i._id + " = createMarker(new GLatLng(" + i.lat + "," + i.lng + ")," + dem + "," + i.state + ",1,1);map.addOverlay(car" + i._id + "); ";
                    if (i.state == 1)
                        ViewBag.carState1Num += 1;
                    if (i.state == 2)
                        ViewBag.carState2Num += 1;
                    if (i.state == 3)
                        ViewBag.carState3Num += 1;
                    dem++;
                }
                string JSdefine = @"
                            <script type='text/javascript'>;
                                var polygonArray = [];
                                var areapolygon = [];
                                var areacenterpoint = [];
                                var carArrayState1 = [];
                                var carArrayState2 = [];
                                var carArrayState3 = [];
                                var carArray = [];
                            </script>
                        ";
                string JSMapInitialize = @"
                            <script type='text/javascript'>
                                function initialize() {
                                    if (GBrowserIsCompatible()) {
                                        map = new GMap2(document.getElementById('map_canvas'));
                                        map.setCenter(new GLatLng(data[0].leftLat, data[0].leftLong), 13);
                                        " + polygonStr + @"
                                        " + carsStr1 + @"
                                        map.setUIToDefault();
                                        $('#datetimepicker').datetimepicker();
                                    }
                                }
                            </script>
                            ";
                Response.Write(JSdefine);
                Response.Write("<script>var data = " + JsonConvert.SerializeObject(rankingList) + " </script>");
                Response.Write(JSMapInitialize);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult ViewPage1()
        {
            return View();
        }
        public ActionResult ForecastChart()
        {
            return View();
        }
        public string checkLogin(string username,string password)
        {
            Models.HomeModels.User user = Models.HomeModels.checkLogin(username, password);
            //string usernameGet = Models.HomeModels.checkLogin(username, password).username;
            if (user.username != "")
            {
                Session["username"] = user.username;
                return user.role;
            }
            else
            {
                return "";
            }
        }
        public string getDriverInfo(string carID)
        {
            Models.HomeModels.Driver driver = Models.HomeModels.getDriverInfo(carID);
            return JsonConvert.SerializeObject(driver);
        }
        public bool getSelectionAreas(string fromDate,string toDate)
        {
            string sqlInsertText = "INSERT INTO checkingTbl([status],[msgRes],[dateTimeFrom],[dateTimeTo],[dateTimeStartExe],[dateTimeFinishExe],[state]) VALUES('False','Not','"+fromDate+"','"+toDate+"','02/01/2017 00:00:00','02/01/2017 00:00:00','False')";
            return Models.HomeModels.insertCheckingTbl(sqlInsertText);
        }
        public string getStatusChecking()
        {
            return Models.HomeModels.getStatusCheckingTbl();
        }
        public ActionResult ScoreBoard()
        {
            if (Session["username"] != null)
            {
                ViewBag.Message = "HaUI - Taxi Problem Forecast";
                ViewBag.carState1Num = 0;
                ViewBag.carState2Num = 0;
                ViewBag.carState3Num = 0;
                string polygonStr = "";
                int dem = 0;
                List<HomeModels.Ranking> rankingList = Models.HomeModels.GetRanking(0);
                dem = 0;
                foreach (HomeModels.Ranking i in rankingList)
                {
                    if (i.type == "out")
                    {
                        polygonStr += "var polygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.leftLong + ")], 'blue', 0.6, 0.6, 'blue', 0.4) ; map.addOverlay(polygon" + i.id + "); var areapolygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "),new GLatLng(" + i.leftLat + "," + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.leftLong + ")], '#f33f00', 5, 1, '#ff0000', 0.2); var areacenterpoint" + i.id + " = new GLatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng()); var myPoint_v3_" + i.id + " = new google.maps.LatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng()); GEvent.addListener(polygon" + i.id + ", 'click', function () {  map.openInfoWindow(myPoint_v3_" + i.id + ", '<b>Thông tin khu vực</b><br>Điểm: " + i.point + " <br>Kiểu: " + i.type + " <br> Số chuyến đón: " + i.inNum + " <br> Số chuyến trả: " + i.outNum + "'); });" + Environment.NewLine;
                        polygonStr += "polygonArray[" + dem + "] = polygon" + i.id + "" + Environment.NewLine;
                    }
                    else
                    {
                        polygonStr += "var polygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "), new GLatLng(" + i.rightDownLat + ", " + i.leftLong + "), new GLatLng(" + i.leftLat + ", " + i.leftLong + ")], 'blue', 0.6, 0.6, 'red', 0.4);  map.addOverlay(polygon" + i.id + "); var areapolygon" + i.id + " = new GPolygon([new GLatLng(" + i.leftLat + ", " + i.leftLong + "),new GLatLng(" + i.leftLat + "," + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.rightDownLong + "),new GLatLng(" + i.rightDownLat + ", " + i.leftLong + ")], '#f33f00', 5, 1, '#ff0000', 0.2); var areacenterpoint" + i.id + " = new GLatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng());  var myPoint_v3_" + i.id + " = new google.maps.LatLng(areapolygon" + i.id + ".getBounds().getCenter().lat(), areapolygon" + i.id + ".getBounds().getCenter().lng());  GEvent.addListener(polygon" + i.id + ", 'click', function () { map.openInfoWindow(myPoint_v3_" + i.id + ", '<b>Thông tin khu vực</b><br>Điểm: " + i.point + " <br>Kiểu: " + i.type + " <br> Số chuyến đón: " + i.inNum + " <br> Số chuyến trả: " + i.outNum + "'); });" + Environment.NewLine;
                        polygonStr += "polygonArray[" + dem + "] = polygon" + i.id + "" + Environment.NewLine;
                    }
                    dem++;
                }
                string carsStr = "";
                List<HomeModels.Cars> carList = HomeModels.GetCarLocations();
                dem = 0;
                foreach (HomeModels.Cars i in carList)
                {
                    carsStr += Environment.NewLine + "var car" + i.id + " = createMarker(new GLatLng(" + i.latlng + ")," + dem + ",1,1,1);map.addOverlay(car" + i.id + "); ";
                    dem++;
                }

                string carsStr1 = "";
                List<HomeModels.Cars1> carList1 = HomeModels.GetCarLocations1();
                dem = 0;
                foreach (HomeModels.Cars1 i in carList1)
                {
                    carsStr1 += Environment.NewLine + "var car" + i._id + " = createMarker(new GLatLng(" + i.lat + "," + i.lng + ")," + dem + "," + i.state + ",'" + i._id + "',1);map.addOverlay(car" + i._id + "); ";
                    if (i.state == 1)
                        ViewBag.carState1Num += 1;
                    if (i.state == 2)
                        ViewBag.carState2Num += 1;
                    if (i.state == 3)
                        ViewBag.carState3Num += 1;
                    dem++;
                }

                string JSdefine = @"
                            <script type='text/javascript'>;
                                var polygonArray = [];
                                var areapolygon = [];
                                var areacenterpoint = [];
                                var carArrayState1 = [];
                                var carArrayState2 = [];
                                var carArrayState3 = [];
                                var carArray = [];
                                var scoreBoardCount;
                            </script>
                        ";
                string JSMapInitialize = @"
                            <script type='text/javascript'>
                                function initialize() {
                                    if (GBrowserIsCompatible()) {
                                        map = new GMap2(document.getElementById('map_canvas'));
                                        map.setCenter(new GLatLng(data[0].leftLat, data[0].leftLong), 13);
                                        " + polygonStr + @"
                                        " + carsStr1 + @"
                                        map.setUIToDefault();
                                    }
                                }
                            </script>
                            ";
                Response.Write(JSdefine);
                Response.Write("<script>var data = " + JsonConvert.SerializeObject(rankingList) + " </script>");
                Response.Write(JSMapInitialize);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public string getRankingByID(int rankingID)
        {
            List<HomeModels.Ranking> rankingItem = Models.HomeModels.GetRanking(rankingID);
            var sds = JsonConvert.SerializeObject(rankingItem);
            return JsonConvert.SerializeObject(rankingItem);
        }
        public string getScoreBoard()
        {
            int dem = 0;
            string ret = "<table class='table'  style='font-size:12px;'><thead><tr><th>#</th><th></th><th>Địa chỉ</th><th></th></tr></thead><tbody>";
            List<HomeModels.ScordBoard> scoreBoardList = Models.HomeModels.getScoreBoard();
            foreach (Models.HomeModels.ScordBoard ls in scoreBoardList)
            {
                ret += "<tr><td>" + ls.order + "</td><td id='latLngTd" + dem.ToString() + "'><span style='visibility:hidden;display:none;'>" + ls.latLng + "</span></td><td id='addressTd" + dem.ToString() + "'>" + ls.address + "</td><td ><span id='rankingID" + dem.ToString() + "' style='visibility:hidden;display:none;'>" + ls.id.ToString() + "</span></td></tr>";
                dem++;
            }
            ret += "</tbody></table><script>scoreBoardCount=" + dem.ToString() + ";</script>";
            return ret;
        }
        public ActionResult DirectionSettingBoard()
        {
            //if (Session["username"] != null)
            //{
            string [] mapViewConfig= ConfigurationManager.AppSettings["MapView"].ToString().Split('-');
            HomeModels.MapView mapView = new HomeModels.MapView();
            mapView.leftDownPoint= new HomeModels.PointLatLng();
            mapView.leftTopPoint = new HomeModels.PointLatLng();
            mapView.rightDownPoint = new HomeModels.PointLatLng();
            mapView.rightTopPoint = new HomeModels.PointLatLng();
            mapView.centerPoint = new HomeModels.PointLatLng();

            mapView.leftDownPoint.lat = double.Parse(mapViewConfig[0].ToString().Split(',')[0].ToString());
            mapView.leftDownPoint.lng = double.Parse(mapViewConfig[0].Split(',')[1].ToString());
            mapView.rightTopPoint.lat = double.Parse(mapViewConfig[1].Split(',')[0].ToString());
            mapView.rightTopPoint.lng = double.Parse(mapViewConfig[1].Split(',')[1].ToString());

            mapView.leftTopPoint.lat = mapView.leftDownPoint.lat;
            mapView.leftTopPoint.lng = mapView.rightTopPoint.lng;

            mapView.rightDownPoint.lat = mapView.rightTopPoint.lat;
            mapView.rightDownPoint.lng = mapView.leftDownPoint.lng;

            mapView.centerPoint.lat = (mapView.rightDownPoint.lat - mapView.leftDownPoint.lat) / 2 + mapView.leftDownPoint.lat;
            mapView.centerPoint.lng = (mapView.leftTopPoint.lng- mapView.leftDownPoint.lng)/2 + mapView.leftDownPoint.lng;

            int numberOfCellOnLat = int.Parse(ConfigurationManager.AppSettings["NumberOfCells"].Split('x')[0].ToString());
            int numberOfCellOnLng = int.Parse(ConfigurationManager.AppSettings["NumberOfCells"].Split('x')[1].ToString());
            double latSegment = (mapView.rightDownPoint.lat - mapView.leftDownPoint.lat) / numberOfCellOnLat;
            double lngSegment = (mapView.leftTopPoint.lng - mapView.leftDownPoint.lng) / numberOfCellOnLng / 2; // Không hiểu vì sao chia cho 2
            HomeModels.PointLatLng[] pointOnLatArr = new HomeModels.PointLatLng[numberOfCellOnLat+1];
            for (int i = 0; i < numberOfCellOnLat + 1; i++)
            {
                pointOnLatArr[i] = new HomeModels.PointLatLng();
                pointOnLatArr[i].lat = mapView.leftDownPoint.lat + i * latSegment;
                pointOnLatArr[i].lng = mapView.leftDownPoint.lng;
            }
            HomeModels.PointLatLng[] pointOnLngArr = new HomeModels.PointLatLng[numberOfCellOnLng+1];
            for (int i = 0; i < numberOfCellOnLng+1; i++)
            {
                pointOnLngArr[i] = new HomeModels.PointLatLng();
                pointOnLngArr[i].lat = mapView.leftDownPoint.lat;
                pointOnLngArr[i].lng = mapView.leftDownPoint.lng + i * lngSegment;
            }
            HomeModels.PointLatLng [,] pointArr = new HomeModels.PointLatLng[numberOfCellOnLat+1,numberOfCellOnLng+1];
            for (int i = 0; i < numberOfCellOnLat + 1; i++)
            {
                for (int j = 0; j < numberOfCellOnLng+1; j++)
                {
                    pointArr[i,j] = new HomeModels.PointLatLng();
                    pointArr[i, j].lat = pointOnLatArr[i].lat;
                    pointArr[i, j].lng = pointOnLngArr[j].lng + j * lngSegment;
                }
            }
            List<HomeModels.Cells> cellList = new List<HomeModels.Cells>();
            int dem = 0;
            string cellPolygonJS = String.Empty;
            string MapViewCellColor = ConfigurationManager.AppSettings["MapViewCellColor"].ToString();
            string MapViewCellTransparent = ConfigurationManager.AppSettings["MapViewCellTransparent"].ToString();
            string MapViewCellBorder = ConfigurationManager.AppSettings["MapViewCellBorder"].ToString();
            string MapViewCellBorderTransparent = ConfigurationManager.AppSettings["MapViewCellBorderTransparent"].ToString();
            for (int i = 0; i < numberOfCellOnLat; i++)
            {
                for (int j = 0; j < numberOfCellOnLng; j++)
                {
                    HomeModels.Cells celli = new HomeModels.Cells();
                    celli = cellBuilder(dem, pointArr[i, j], pointArr[i + 1, j], pointArr[i + 1, j + 1], pointArr[i, j + 1]);
                    cellList.Add(celli);
                    cellPolygonJS += "var cellPolygon" + dem + " = new GPolygon([new GLatLng(" + pointArr[i, j].lat + ", " + pointArr[i, j].lng + "), new GLatLng(" + pointArr[i + 1, j].lat + ", " + pointArr[i + 1, j].lng + "), new GLatLng(" + pointArr[i + 1, j + 1].lat + ", " + pointArr[i + 1, j + 1].lng + "), new GLatLng(" + pointArr[i, j + 1].lat + ", " + pointArr[i, j + 1].lng + "), new GLatLng(" + pointArr[i, j].lat + ", " + pointArr[i, j].lng + ")], '" + MapViewCellBorder + "', " + MapViewCellBorderTransparent + ", 0.4, '" + MapViewCellColor + "', " + MapViewCellTransparent + ");  map.addOverlay(cellPolygon" + dem + ");  GEvent.addListener(cellPolygon" + dem + ", 'click', function () {changeCell('" + "[" + i.ToString() + "," + j.ToString() + "]" + "'," + celli.centerPoint.lat + "," + celli.centerPoint.lng + ") ; });" + Environment.NewLine;
                    cellPolygonJS += "polygonArray[" + dem + "] = cellPolygon" + dem + ";" + Environment.NewLine;
                    cellPolygonJS += "var cellLable" + dem + " = new ELabel(new GLatLng(" + celli.centerPoint.lat + "," + celli.centerPoint.lng + "), '<a href=javascript:changeCell(&#39;" + "[" + i.ToString() + "," + j.ToString() + "]" + "&#39;," + celli.centerPoint.lat + "," + celli.centerPoint.lng + ")><div class=numberCircle id=txtHint" + dem + ">" + celli.id + "</div></a>', '', new GSize(-13,-6), 100 ); map.addOverlay(cellLable" + dem + ");" + Environment.NewLine;
                    dem++;
                }
            }
            
            dem = 0;
            for (int i = 0; i < numberOfCellOnLat; i++)
            {
                for (int j = 0; j < numberOfCellOnLng; j++)
                {
                    dem++;
                    HomeModels.Cells celli = new HomeModels.Cells();
                    celli.id = dem;
                    celli.ArrDesc = "[" + i.ToString() + "," + j.ToString() + "]";
                    celli.leftDownPoint = new HomeModels.PointLatLng();
                    celli.leftDownPoint.lat = pointOnLatArr[i].lat;
                    celli.leftDownPoint.lng = (j * latSegment) + pointOnLngArr[j].lng;
                }
            }
            Response.Write("<script>var mapView = " + JsonConvert.SerializeObject(mapView) + " </script>"+Environment.NewLine);
            Response.Write("<script>var cellArray = " + JsonConvert.SerializeObject(cellList) + " </script>");

            string mapViewPolygonJS = "var mapViewPolygon = new GPolygon([new GLatLng(" + mapView.leftDownPoint.lat + ", " + mapView.leftDownPoint.lng + "), new GLatLng(" + mapView.leftTopPoint.lat + ", " + mapView.leftTopPoint.lng + "), new GLatLng(" + mapView.rightTopPoint.lat + ", " + mapView.rightTopPoint.lng + "), new GLatLng(" + mapView.rightDownPoint.lat + ", " + mapView.rightDownPoint.lng + "), new GLatLng(" + mapView.leftDownPoint.lat + ", " + mapView.leftDownPoint.lng + ")], 'blue', 0.6, 0.6, 'black', 1);  map.addOverlay(mapViewPolygon);  GEvent.addListener(mapViewPolygon, 'click', function () {alert('h'); });" + Environment.NewLine;

            ViewBag.Message = "";
                string JSdefine = @"
                            <script type='text/javascript'>;
                                var polygonArray = [];
                                var areapolygon = [];
                                var areacenterpoint = [];
                                var carArrayState1 = [];
                                var carArrayState2 = [];
                                var carArrayState3 = [];
                                var carArray = [];
                            </script>
                        ";
                string JSMapInitialize = @"
                            <script type='text/javascript'>
                                function initialize() {
                                    if (GBrowserIsCompatible()) {
                                        map = new GMap2(document.getElementById('map_canvas'));
                                        map.setCenter(new GLatLng("+mapView.centerPoint.lat+@", "+mapView.centerPoint.lng+@"), 13);
                                        " + cellPolygonJS + @"
                                        
                                        map.setUIToDefault();
                                    }
                                }
                            </script>
                            ";
                Response.Write(JSdefine);
                //Response.Write("<script>var data = " + JsonConvert.SerializeObject(rankingList) + " </script>");
                Response.Write(JSMapInitialize);
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            return View();
        }
        public string changeConfiguration(string type, string MapView, string MapViewCellColor, string MapViewCellTransparent, string MapViewCellBorder, string MapViewCellBorderTransparent, string NumberOfCells)
        {
            if(type == "update")
            {
               
                    ConfigurationManager.AppSettings.Set("MapView", MapView);
                
                    ConfigurationManager.AppSettings.Set("MapViewCellColor", MapViewCellColor);
              
                    ConfigurationManager.AppSettings.Set("MapViewCellTransparent", MapViewCellTransparent);
               
                    ConfigurationManager.AppSettings.Set("MapViewCellBorder", MapViewCellBorder);
              
                    ConfigurationManager.AppSettings.Set("MapViewCellBorderTransparent", MapViewCellBorderTransparent);
              
                    ConfigurationManager.AppSettings.Set("NumberOfCells", NumberOfCells);
                return "updated";
            }else
            {

                return "{\"MapView\":\"" + ConfigurationManager.AppSettings["MapView"].ToString() + "\",\"MapViewCellColor\":\"" + ConfigurationManager.AppSettings["MapViewCellColor"].ToString() + "\",\"MapViewCellTransparent\":\"" + ConfigurationManager.AppSettings["MapViewCellTransparent"].ToString() + "\",\"MapViewCellBorder\":\"" + ConfigurationManager.AppSettings["MapViewCellBorder"].ToString() + "\",\"MapViewCellBorderTransparent\":\"" + ConfigurationManager.AppSettings["MapViewCellBorderTransparent"].ToString() + "\",\"NumberOfCells\":\"" + ConfigurationManager.AppSettings["NumberOfCells"].ToString() + "\"}";
            }
        }
        public HomeModels.Cells cellBuilder(int id, HomeModels.PointLatLng A, HomeModels.PointLatLng B, HomeModels.PointLatLng C, HomeModels.PointLatLng D)
        {
            HomeModels.Cells celli = new HomeModels.Cells();
            celli.id = id;
            celli.leftDownPoint = A;
            celli.rightDownPoint = B;
            celli.rightTopPoint = C;
            celli.leftTopPoint = D;
            celli.centerPoint = new HomeModels.PointLatLng();
            celli.centerPoint.lat = ((B.lat - A.lat) / 2) + A.lat;
            celli.centerPoint.lng = ((D.lng - A.lng) / 2) + A.lng;
            return celli;
        }

    }
    
}
