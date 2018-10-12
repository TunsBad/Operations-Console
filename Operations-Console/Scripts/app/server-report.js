"use strict";

$(function () {
    var ui = new transferUI();
    ui.renderReportGrid();
});

$("#detailsbutton").click(function () {
    window.location.href = "";
});

var transferUI = (function () {
    function transferUI() { }
    transferUI.prototype.renderReportGrid = function () {

        var grid = $("#serverReportGrid").kendoGrid({
            editable: false,
            toolbar: ["excel"],
            excel: {
                fileName: "server-report.xlsx",
                allPages: true,
                filterable: true
            },
            dataSource: {

                transport: {
                    read: {
                        url: Api + 'api/Server/GetServerReport?serverid=' + serverid,
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json"
                    },
                    parameterMap: function (data, type) {
                        return kendo.stringify(data);
                    }
                },
                pageSize: 10,
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "Time",
                        fields: {
                            Time: { editable: true, validation: { required: true } },
                            ServerName: { editable: false, validation: { required: true } },
                            IpAddress: { editable: false, validation: { required: true } },
                            DiskUsage: { editable: false, validation: { required: true } },
                            CpuUsage: { editable: false, validation: { required: true } },
                            NetworkUsage: { editable: false, validation: { required: true } },
                            MemoryUsage: { editable: false, validation: { required: true } }
                        }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: false
            },
            filterable: false,
            selectable: true,
            editable: true,
            columns: [
                {
                    field: 'Time', title: 'Time', format: "{0:dd-MMM-yyyy}",
                    template: "#= kendo.toString(kendo.parseDate(Time, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMMM yyyy') #"
                },
                { field: 'ServerName', title: 'Server Name' },
                { field: 'IpAddress', title: 'Ip Address' },
                { field: 'DiskUsage', title: 'Disk Usage (%)' },
                { field: 'CpuUsage', title: 'Cpu Usage (%)' },
                { field: 'NetworkUsage', title: 'Network Usage (%)' },
                { field: 'MemoryUsage', title: 'Memory Usage (%)' }
            ],
            editable: { mode: 'inline' },
            sortable: { mode: 'multiple' },
            pageable: {
                pageSize: 10,
                pageSizes: [10, 25, 40, 70, 100],
                previousNext: true,
                buttonCount: 5

            }
        });

    };

    return transferUI;
})();
