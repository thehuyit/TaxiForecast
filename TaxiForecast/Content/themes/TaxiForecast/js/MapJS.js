var map;
var carFlag = 1;
var areaFlag = 1;
var baseIcon = new GIcon(G_DEFAULT_ICON);
baseIcon.shadow = 'http://dpompa.com/images/IconLocations1.png';
baseIcon.iconSize = new GSize(30, 30);
baseIcon.shadowSize = new GSize(37, 34);
baseIcon.iconAnchor = new GPoint(9, 25); //x anchor=1/2 x iconsize, y=y cua iconsize thi marker se nam tren duong ong
baseIcon.infoWindowAnchor = new GPoint(20, 20);
function createMarker(latlng, index, err_icon, carID, id_ward12) {
    var letter = String.fromCharCode('A'.charCodeAt(0) + index);
    var letteredIcon = new GIcon(baseIcon);
    letteredIcon.iconSize = new GSize(30, 50);
    markerOptions = { icon: letteredIcon };
    var marker = new GMarker(latlng, markerOptions);
    
    if (err_icon == 1) {
        letteredIcon.image = getBaseURL() + '/Content/themes/TaxiForecast/img/TaxiIcon.png';
        carArrayState1[index] = marker;
    } else {
    if (err_icon == 2) {
        letteredIcon.image = getBaseURL() + '/Content/themes/TaxiForecast/img/TaxiIcon_red.png';
        carArrayState2[index] = marker;
    } else {
        if (err_icon == 3) {
            letteredIcon.image = getBaseURL() + '/Content/themes/TaxiForecast/img/TaxiIcon_black.png';
            carArrayState3[index] = marker;
        }
    }
    }

    GEvent.addListener(marker, 'click', function () {

        var myHtml = "<div class='loader'></div>";
        map.openInfoWindowHtml(latlng, myHtml, {
            maxTitle: 'Thông tin lái xe',
            maxContent: myHtml
        });

        myHtml = "";
        myHtml += "<div class='card'>";
        $.ajax({
            type: "POST",
            url: "/Home/getDriverInfo?carID=" + carID + "",
            dataType: 'json',
            success: function (msg) {
                myHtml += "     <img src='" + getBaseURL() + "/Content/themes/TaxiForecast/img/DriverAvatars/" + msg.avartar + "' alt='Avatar' style='width:100%'>";
                myHtml += "     <div class='cardContainer'>";
                myHtml += "         <h4><b>"+msg.phoneNumber+"</b></h4>";
                myHtml += "         <p>"+msg.fullName+"</p>";
                myHtml += "     </div>";
                myHtml += "</div>";
                map.openInfoWindowHtml(latlng, myHtml, {
                    maxTitle: 'Thông tin lái xe',
                    maxContent: myHtml
                });
            }
        });

    });
    carArray[index] = marker;
    return marker;
}
function getBaseURL() {
    var url = location.href;
    var baseUrl = "http://" + url.split("/")[2];
    return baseUrl;
}

//
function showCarState1() {
    for (i = 1; i < carArray.length; i++) {
        if (typeof carArray[i] != 'undefined') {
            carArray.forEach(function addNumber(value) { value.hide(); });
        }
    }
    carArrayState1.forEach(
    function addNumber(value) { value.show(); });
}
function showCarState2() {
    for (i = 1; i < carArray.length; i++) {
        if (typeof carArray[i] != 'undefined') {
            carArray.forEach(function addNumber(value) { value.hide(); });
        }
    }
    carArrayState2.forEach(
    function addNumber(value) { value.show(); });
}
function showCarState3() {
    for (i = 1; i < carArray.length; i++) {
        if (typeof carArray[i] != 'undefined') {
            carArray.forEach(function addNumber(value) { value.hide(); });
        }
    }
    carArrayState3.forEach(
    function addNumber(value) { value.show(); });
}
function showHideCars() {
    if (carFlag != 0) {
        for (i = 1; i < carArray.length; i++) {
            if (typeof carArray[i] != 'undefined') {
                carArray.forEach(function addNumber(value) { value.hide(); });
            }
        }
        carFlag = 0;
        $("#carShowControl").html("Hiển thị xe");
    } else {
        carArray.forEach(
        function addNumber(value) { value.show(); });
        carFlag = 1;
        $("#carShowControl").html("Ẩn xe");
    }
}
//
function showHideAreas(title) {
    if (areaFlag != 0) {
        for (i = 1; i < polygonArray.length; i++) {
            if (typeof polygonArray[i] != 'undefined') {
                polygonArray.forEach(function addNumber(value) { value.hide(); });
            }
        }
        areaFlag = 0;
        $("#areaShowControl").html("Hiển thị "+title+"");
    } else {
        polygonArray.forEach(
        function addNumber(value) { value.show(); });
        areaFlag = 1;
        $("#areaShowControl").html("Ẩn "+title+"");
    }
}

function clickroute(rankingID, lati, long) {
    var latLng = new google.maps.LatLng(lati, long); //Makes a latlng
    map.panTo(latLng); //Make map global

    var myHtml = "<div class='loader'></div>";
    map.openInfoWindowHtml(latLng, myHtml, {
        maxTitle: 'Thông tin điểm dự đoán',
        maxContent: myHtml
    });

    myHtml = "";
    myHtml += "<div class='card'>";
    $.ajax({
        type: "POST",
        url: "/Home/getRankingByID?rankingID=" + rankingID + "",
        dataType: 'json',
        success: function (msg) {
            myHtml += "     <div class='cardContainer'>";
            myHtml += "         <h4>Số điểm: <b>" + msg[0].point + "</b></h4>";
            myHtml += "         <p>Kiểu: <b>" + msg[0].type + "</b></p>";
            myHtml += "         <p>Số chuyến đón: <b>" + msg[0].inNum + "</b></p>";
            myHtml += "         <p>Số chuyến trả: <b>" + msg[0].outNum + "</b></p>";
            myHtml += "     </div>";
            myHtml += "</div>";
            map.openInfoWindowHtml(latLng, myHtml, {
                maxTitle: 'Thông tin Điểm dự đoán',
                maxContent: myHtml
            });
        }
    });
}

function createScoreBoard() {
    var url = "/Home/getScoreBoard";
    $.get(url, null, function (data) {
        if (data) {
            $("#ScoreBoardTbl").html(data);
            for (var i = 0; i < scoreBoardCount; i++) {
                getGEOCode($("#latLngTd" + i + "").text().split(",")[0], $("#latLngTd" + i + "").text().split(",")[1], i);
            }
        }
    });
}

function getGEOCode(lat, lng, dem) {
    var geoCodeUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lng + "&key=AIzaSyDUBkw_YP930CJGPTr-Dq2lspcqzI9g7UA";
    var rankingID = $("#rankingID" + dem + "").text();
    $.ajax({
        type: "GET",
        url: geoCodeUrl,
        dataType: 'json',
        success: function (msg) {
            $("#addressTd" + dem + "").html("<a href='javascript:clickroute("+rankingID+","+lat+","+lng+");'>" + msg.results[0].formatted_address+"</a>")
        }
    });
}


function checkStatusCheckingTbl() {
    count++;
    var url = "/Home/getStatusChecking";
    $.get(url, null, function (data1) {
        if (data1 == "False") {
            if (count < 10) {
                setTimeout(function () {
                    checkStatusCheckingTbl();
                }, 1000);
            } else {
                count = 0;
                $("#modalAlert").html("Thời gian quá lớn ...");
                setTimeout(function () {
                    $('#myModal').modal('hide');
                    $("#modalAlert").html("Đang xử lý ...");
                }, 1000);
                return;
            }
        }
        else {
            $("#modalAlert").html("Step 3/3 ...");
            setTimeout(function () {
                $('#myModal').modal('hide');
                $("#modalAlert").html("Đang xử lý ...");
                window.location = getBaseURL() + "/Home/ScoreBoard";
            }, 1000);
            return;
        }
    });
}
function getSelectionArea() {
    $("#modalAlert").html(" Step 1/3...");
    $('#myModal').modal('show');
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();
    if (fromDate != "" && toDate != "") {
        var url = "/Home/getSelectionAreas?fromDate=" + fromDate + "&toDate=" + toDate + "";
        $.get(url, null, function (data) {
            if (data) {
                $("#modalAlert").html(" Step 2/3...");
                checkStatusCheckingTbl();
            } else {
                $("#modalAlert").html(" Error");
            }
        });
    }
    else {
        $("#modalAlert").html(" Vui lòng điền thông tin tài khoản");
        setTimeout(function () {
            $('#myModal').modal('hide');
            $("#modalAlert").html(" Đang xử lý ...");
        }, 1000);
    }
}

/*Module: DirectionSettingBoard*/
function changeConfiguration(type) {
    $('#myModal').modal('show');
    if (type == "select") {
        var url = "/Home/changeConfiguration?type=" + type + "&";
        $.getJSON(url, function (data) {
            $.each(data, function (key, val) {
                if (key == "MapView") {
                    $("#leftDownPoint").val(val.toString().split("-")[0]);
                    $("#rightTopPoint").val(val.toString().split("-")[1]);
                } else {
                    $("#" + key).val(val);
                }
            });
        });
    } else {
        var MapView = $("#leftDownPoint").val() + "-" + $("#rightTopPoint").val();
        var MapViewCellColor = $("#MapViewCellColor").val();
        var MapViewCellTransparent = $("#MapViewCellTransparent").val();
        var MapViewCellBorder = $("#MapViewCellBorder").val();
        var MapViewCellBorderTransparent = $("#MapViewCellBorderTransparent").val();
        var NumberOfCells = $("#NumberOfCells").val();
        var url = "/Home/changeConfiguration?type=" + type + "&MapView=" + MapView + "&MapViewCellColor=" + MapViewCellColor + "&MapViewCellTransparent=" + MapViewCellTransparent + "&MapViewCellBorder=" + MapViewCellBorder + "&MapViewCellBorderTransparent=" + MapViewCellBorderTransparent + "&NumberOfCells=" + NumberOfCells + "";
        $.get(url, null, function (data) {
            window.location = "/Home/DirectionSettingBoard/";
        });
    }
}
function changeCell(matrixID, lat, lng) {
    $("#cellMatrixID").text(matrixID);
    $('#cellDetailModal').modal('show');
    var geoCodeUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lng + "&key=AIzaSyDUBkw_YP930CJGPTr-Dq2lspcqzI9g7UA";
    $.ajax({
        type: "GET",
        url: geoCodeUrl,
        dataType: 'json',
        success: function (msg) {
            $("#cellAddress").html(msg.results[0].formatted_address + "</a>")
        }
    });
}

/**/