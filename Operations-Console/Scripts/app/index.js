﻿var myMainInterval = '';
var stats = {};

$(function () {
    getData();
    myMainInterval = setInterval(function () {
        getData();
    }, 30000);

})

function getData() {
    document.getElementById('appTitle').innerHTML = 'Server Statistics | ACS - OPERATIONS CONSOLE';

    getApps();
    var output = "";

    $.ajax({
        url: Api + 'api/Server/GetServerStatistic',
        type: 'GET',
        success: function (data) {
            //console.log('data', data);

            diskWarningCount = data.DiskWarningCount;
            criticalWarningCount = data.CriticalWarningCount;

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
                                renderStatsValue("Network", stats[i].ServerStatistic.NetworkUsage  + "&nbsp;&nbsp;Mbps")
                                + renderStatsValue("CPU", stats[i].ServerStatistic.CpuUsagePercent + "%" ) + 
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

function renderStatsValue(label, value) {
    return "<div class='row'><div class='col-md-4 statslabel'>" + label
    + "</div><div class='col-md-8 statsvalue'>" + value + "</div></div>"
}

function isTooOld(time) {
    var timeAsDate = new Date(time.replace("T", " ").substring(0, 16));
    if (new Date().getTime() - timeAsDate.getTime() > 10 * 60 * 1000) {
        return true;
    }
    return false;
}

function getPercentageFreeForDiskData(diskdata){
    var indexOfOpeningBracket = diskdata.indexOf("(");
    var indexOfClosingBracket = diskdata.indexOf(")", indexOfOpeningBracket + 1);
    var percentFreeString = diskdata.substring(indexOfOpeningBracket + 1, indexOfClosingBracket - indexOfOpeningBracket - 2);
    return Number(percentFreeString);
};

function doesStatHaveDiskIssue(diskdata) {
    if (!diskdata) return false;
    for (var i = 0; i < diskdata.length; i++) {
        if (getPercentageFreeForDiskData(diskdata[i]) < 20) {
            return true;
        }
    }
    return false;
}

function renderApps(appsList) {
    if (!appsList) return " ";
    var appsListString = "";
    for (var i = 0; i < appsList.length; i++) {
        appsListString += appsList[i] + (i == appsList.length - 1 ? "" : ", ");
    }
    return appsListString;
}

function openServerStatsNav() {
    document.getElementById("mySidenav").style.width = "275px";
}

function closeServerStatsNav() {
    document.getElementById("mySidenav").style.width = "0";
}