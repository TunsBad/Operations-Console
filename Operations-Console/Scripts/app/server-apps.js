var id = '';
var appName = '';
var appTitle = ' Server Statistics | ACS - OPERATIONS CONSOLE';
var myGetAllDataButtonsInterval = '';
var myServerAppDataButtonsInterval = '';
var stats = {};
var criticalWarningCount = '';
var diskWarningCount = '';

function getApps() {
    var buttonoutputs = "";

    $.ajax({
        url: Api + 'api/Apps/GetApps',
        type: 'GET',
        success: function (data) {
            //console.log("Apps :", data);
            $('#appbuttons').replaceWith('<div class="nav navbar-nav" id="appbuttons"></div>')

            if(data.length > 0){
                buttonoutputs = buttonoutputs + "<a  class='appButton' id='appButton' onclick='callGetData()'><span class='btntext' id='btntext'>All</span><div class='pull-right'><span class='badge progress-bar-danger'>" + renderBadge(data[data.length - 1].CriticalErrorCount) + "</span>&nbsp<span class='badge progress-bar-warning'>" + renderBadge(data[data.length - 1].DiskErrorCount) + "</span>&nbsp<span class='badge warningbadge'>" + renderBadge(data[data.length - 1].WarningErrorCount) + "</span></div></a>"

                for (var i = 0; i < data.length - 1; i++) {
                    //$AppName = data[i].Name;

                    buttonoutputs = buttonoutputs + "<a class='appButton' " +
					(" id='appButton" + data[i].AppId.toString()) + "' onclick='return callGetAppData(" + data[i].AppId + ",\"" + data[i].Name + "\")'>\
							 <span  id='btntext" + data[i].AppId.toString() + "' class='btntext'>" + data[i].Name + "</span><div class='pull-right'><span class='badge progress-bar-danger'>" + renderBadge(data[i].CriticalErrorCount) + "</span>&nbsp<span class='badge progress-bar-warning'>" + renderBadge(data[i].DiskErrorCount) + "</span>&nbsp<span class='badge warningbadge'>" + renderBadge(data[i].WarningErrorCount) + "</span></div>\
						   </a>"
                }
				   
                $("#appbuttons").html(buttonoutputs);

                $('#appButton').css('cssText', 'background-color: #FFF !important;');
                $('#btntext').css('cssText', 'color: #000 !important;');

            }
        }
    });

};

function styleCurrentApp(appId) {
    $('.appButton').css('background-color', '');
    $('#appButton' + appId).css('cssText', 'background-color: #FFF !important;');
    $('.btntext').css('cssText', 'color: ""');
    $('#btntext' + appId).css('cssText', 'color: #000 !important;');
}

function callGetData() {
    if(appName) {
        appName = '';
    }

    document.getElementById('appName').innerHTML = appName;
    document.getElementById('appTitle').innerHTML = appName + appTitle;

    clearInterval(myMainInterval);
    clearInterval(myGetAllDataButtonsInterval);
    clearInterval(myServerAppDataButtonsInterval);

    getData();
    myGetAllDataButtonsInterval = setInterval(function () {
        getData();
    }, 30000);
}

function callGetAppData(appid, name) {
    appName = name
    clearInterval(myGetAllDataButtonsInterval);
    clearInterval(myServerAppDataButtonsInterval);
    clearInterval(myMainInterval);

    id = appid;
    getAppServers(id);

    document.getElementById('appName').innerHTML = appName;
    document.getElementById('appTitle').innerHTML = appName + appTitle;

    myServerAppDataButtonsInterval = setInterval(function () {
        getAppServers(id);
    }, 30000);
    styleCurrentApp(appid);
}

function getAppServers(appid) {
    //console.log("App Id: ", appid);
    var output = "";
    
    $.ajax({
        url: Api + 'api/Apps/GetAppServerStatistic',
        type: 'GET',
        data: { 'appid': appid },
        success: function (data) {
           //console.log("Response :", data);

            stats = data.Serverdetails;
            
            $("#stats").replaceWith('<div class="container-fluid" id="stats"></div>')
            for (var i = 0; i < stats.length; i++) {
                if (i % 4 == 0) {
                    if (i > 0) {
                        output = output + '</div>';
                    }
                    output = output + '<div class="row">';
                }
                output = output + "<div class='col-md-3 panel-default grow' >\
                    <div class='content-box-header panel-heading' style='background-color:" + stats[i].Status.Color + "'>\
                        <div class='row'>\
                            <div class='col-md-2'><img class='img-responsive' src='" + stats[i].Status.DbType + "' id='Panel_Image'></div>\
                            <div class='col-md-10'>\
                            <div class='panel-title' style='font-style:oblique; color:" + stats[i].Status.TextColor + ";'>\
                                <p class='headervalue'>" + stats[i].ServerInfor.IpAddress + "<div class='statstime " + stats[i].ServerStatistic.TimeDisplayClass + "'>&nbsp;@" +
                                stats[i].ServerStatistic.Time.replace("T", " ").substring(0, 16) + "</div></p>\
                                <p class='headerlabel'>" + stats[i].ServerInfor.ServerName + "</p>\
                                <p><span class='headerlabel'>Memory:&nbsp;</span><span class='headervalue'>" + stats[i].ServerStatistic.MemoryUsagePercent + "&nbsp;%</span>&nbsp;-&nbsp;<span class='headerlabel'>CPU:&nbsp;</span><span class='headervalue'>" + stats[i].ServerStatistic.CpuUsagePercent + "&nbsp;%</span></p>\
                                <p class='statserror'>" + stats[i].Status.Errors + "</p>\
                            </div>\
                            </div>\
                        </div>\
                    </div>\
                    <div class='content-box-large box-with-header backgroundcontent'>"
                                 + renderStatsValue("Server Type", stats[i].ServerInfor.ServerType);
                if (stats[i].ServerStatistic.DiskData) {
                    for (var j = 0; j < stats[i].ServerStatistic.DiskData.length; j++) {
                        output = output + renderStatsValue("Disk ", stats[i].ServerStatistic.DiskData[j]);
                    }
                }
                output = output +
                                renderStatsValue("Network", stats[i].ServerStatistic.NetworkUsage + "&nbsp;&nbsp;Mbps")
                                + renderStatsValue("CPU", stats[i].ServerStatistic.CpuUsagePercent + "%") +
                                renderStatsValue("Memory", stats[i].ServerStatistic.MemoryUsage + "&nbsp;&nbsp;GB," + stats[i].ServerStatistic.MemoryUsagePercent + "%")
                                + renderStatsValue("Apps", renderApps(stats[i].ServerInfor.Apps));
                output = output +
                            "<div class='row'>\
                                <div class='col-md-4 col-md-offset-7' style='padding: 6px;'>\
                                    <a class='viewmorebtn' href = '/Home/Viewmore?serverid=" + stats[i].ServerInfor.Id + "&servername=" + stats[i].ServerInfor.ServerName + "'>View More</a>\
                                </div>\
                            </div>\
                    </div>\
                </div>";
            }
            if (stats.length % 4 != 0) {
                output = output + '</div>';
            }

            $("#stats").html(output);
        }
    });
   
}

function renderBadge(badgeCount) {
    if (badgeCount > 0) {
        return badgeCount.toString();
    }
    return "";
}

$(document).click(function (event) {
    if (!$(event.target).closest(".serverstats-sidenav, .openside").length) {
        closeServerStatsNav();
    }
});