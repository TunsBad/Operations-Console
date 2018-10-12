var allCellApps = [];

Array.prototype.round = function (value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
};

$(function () {
    getAppsStatus();
})

function getAppsStatus() {
    document.getElementById("cellAppName").innerHTML = '';
    document.getElementById('appTitle').innerHTML = 'App Statistics | ACS - OPERATIONS CONSOLE';

    $.ajax({
        url: Api + 'api/AppStatus/GetAllAppsStatus',
        type: 'GET',
        success: function (data) {
            //console.log('data', data);

            allCellApps = data;
            getUniqueIpNames(data);
            displayApps(data);
        }
    });
};

function getUniqueIpNames(response) {
    var uniqueIpAddresses = [];
    var cellappserverInfo = [];
    var totalyellowcount = 0;
    var totalredcount = 0;
    appAtt = {};
   

    for (var i = 0; i < response.length; i += 1) {
        if (!isInArray(response[i].CellAppsName, uniqueIpAddresses)) 
            uniqueIpAddresses.push(response[i].CellAppsName);
    };
 
    function isInArray(value, uniqueIpAddresses) {
        return uniqueIpAddresses.indexOf(value) > -1;
    }

    for (var i = 0; i < uniqueIpAddresses.length; i += 1) {
        var serverName = uniqueIpAddresses[i];
        var appAttributes = {};
        var yellowtotal = 0;
        var redtotal = 0;

        for (var j = 0; j < response.length; j += 1) {
            if (response[j].CellAppsName == serverName) {
                appAttributes.CellAppName = response[j].CellAppsName;
                if (response[j].ColourCode == "yellow") {
                    yellowtotal += 1;
                }
                if (response[j].ColourCode == "red") {
                    redtotal += 1;
                }
            }
        }

        totalyellowcount += yellowtotal;
        totalredcount += redtotal;

        appAttributes.YellowCount = yellowtotal;
        appAttributes.RedCount = redtotal;
        cellappserverInfo.push(appAttributes);
        //console.log(cellappserverInfo);
    }

    cellappserverInfo.unshift({ CellAppName: "All", YellowCount: totalyellowcount, RedCount: totalredcount })

    //console.log(cellappserverInfo);
    listApps(cellappserverInfo);
};

function listApps(data) {
    var buttonoutputs = "";

    $('#appbuttons').replaceWith('<div class="nav navbar-nav" id="appbuttons"></div>')

    buttonoutputs = buttonoutputs + "<a class='appButton' id='appButton' onclick=getAppsStatus()>\
							 <span id='btntext' class='btntext'>All</span><div class='pull-right'><span class='badge warningbadge'>" + renderBadge(data[0].YellowCount) + "</span>&nbsp<span class='badge progress-bar-danger'>" + renderBadge(data[0].RedCount) + "</span></div>\
						   </a>"

    if (data.length > 0) {
        for (var i = 1; i < data.length; i += 1) {
            buttonoutputs = buttonoutputs + "<a class='appButton' id='appButton" + i.toString() + "' onclick=getCellAppsOnServer('" + i + "','" + data[i].CellAppName + "')>\
							 <span id='btntext" + i.toString() + "' class='btntext'>" + data[i].CellAppName + "</span><div class='pull-right'><span class='badge warningbadge'>" + renderBadge(data[i].YellowCount) + "</span>&nbsp<span class='badge progress-bar-danger'>" + renderBadge(data[i].RedCount) + "</span></div>\
						   </a>"
        }

        $("#appbuttons").html(buttonoutputs);
    }

    $('#appButton').css('cssText', 'background-color: #FFF !important;');
    $('#btntext').css('cssText', 'color: #000 !important;');
};

function getCellAppsOnServer(appid, serverName) {
    var sortedApps = [];
    styleCurrentApp(appid);

    document.getElementById("cellAppName").innerHTML = serverName;
    document.getElementById('appTitle').innerHTML = serverName + ' App Statistics | ACS - OPERATIONS CONSOLE';

    for (var i = 0; i < allCellApps.length; i += 1) {
        if (allCellApps[i].CellAppsName === serverName) {
            sortedApps.push(allCellApps[i]);
        };
    };

    displayApps(sortedApps);
};

function styleCurrentApp(appId) {
    $('.appButton').css('background-color', '');
    $('#appButton' + appId).css('cssText', 'background-color: #FFF !important;');
    $('.btntext').css('cssText', 'color: ""');
    $('#btntext' + appId).css('cssText', 'color: #000 !important;');
};

function displayApps(data) {
    var output = '';

    $("#cellappstatus").replaceWith('<div class="container-fluid" id="cellappstatus"></div>')
    for (var i = 0; i < data.length; i++) {
        output = output + "<div class='col-md-3 panel-default'>\
                <div class='content-box-header panel-heading' style='font-style:oblique; background-color:" + data[i].ColourCode + "'>\
                    <div class='row'>\
                        <div class='col-md-2'><img class='img-responsive' src='/Content/tower.png' id='Panel_Image'></div>\
                        <div class='col-md-10'>\
                            <div class='panel-title' style='font-style:oblique; color:" + data[i].TextColor + "'>\
                               <p class='headervalue'>" + data[i].ServiceName + "</p>\
                               <p class='apptype'>" + data[i].ShortCode.toString().replace("%23", "#") + "</p>\
                               <p class='apptype'>" + data[i].IpAddress + "</p>\
                               <p class='ussdtype'>" + data[i].ApplicationType + "</p><br>\
                               <p style:'bold'><span class='headerlabel'>Delay:&nbsp;</span>" + data[i].ExecutionTime.toFixed(2) + "&nbsp;ms</p>\
                               <p class='statserror1'>" + data[i].FailureReason + "</p>\
                            </div>\
                        </div>\
                    </div>\
                </div>\
                </div>";
    }
    //.round(data[i].ExecutionTime, 2)
    $("#cellappstatus").html(output);
};

function openAppStatsNav() {
    document.getElementById("mySidenavApp").style.width = "275px";
};

function closeAppStatsNav() {
    document.getElementById("mySidenavApp").style.width = "0";
};

function renderBadge(badgeCount) {
    if (badgeCount > 0) {
        return badgeCount.toString();
    }
    return "";
};

$(document).click(function (event) {
    if (!$(event.target).closest(".appstats-sidenav, .openside").length) {
        closeAppStatsNav();
    }
});