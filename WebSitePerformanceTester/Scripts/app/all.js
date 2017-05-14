
function showChartAndTable() {
    $("table.ko-grid").addClass("table table-striped");
    $(".results-table").show("slow");
    $("#chart").show("slow");
}

var urls = new AppViewModel();
ko.applyBindings(urls);
var connection = $.hubConnection();
var siteMapHubProxy = connection.createHubProxy('SiteMapHub');


// hub events handlers
siteMapHubProxy.on("results",
    function (results) {
        onResults(results);
    });
siteMapHubProxy.on("dates",
    function (dates) {
        onDates(dates);

    });
siteMapHubProxy.on("Done",
    function () {
        $(".status").html("Done");
        connection.stop();
        console.log("stoped");

    });
siteMapHubProxy.on('Item',
    function (url, time) {
        showChartAndTable();
        onItem(url, time);
    });


var chart = Highcharts.chart(
    {
        chart: {
            renderTo: "chart",
            height: 400,
            zoomType: 'x'
        },
        boost: {
            useGPUTranslations: true
        },
        title: {
            text: 'Url Response Time'
        },
        subtitle: {
            text: document.ontouchstart === undefined
                ? 'Click and drag in the plot area to zoom in'
                : 'Pinch the chart to zoom in'
        },
        xAxis: {
            lineWidth: 0,
            minorGridLineWidth: 0,
            lineColor: 'transparent',

            labels: {
                enabled: false
            },
            minorTickLength: 0,
            tickLength: 0
        },
        yAxis: {
            title: {
                text: 'Response time, ms'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            area: {
                fillColor: {
                    linearGradient: {
                        x1: 0,
                        y1: 0,
                        x2: 0,
                        y2: 1
                    },
                    stops: [
                        [0, Highcharts.getOptions().colors[0]],
                        [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                    ]
                },
                marker: {
                    radius: 2
                },
                lineWidth: 1,
                states: {
                    hover: {
                        lineWidth: 1
                    }
                },
                threshold: null,
                turboThreshold: 0
            }
        },

        series: [
            {
                type: 'area',
                name: 'Response time, ms',
                data: []
            }
        ]
    });


$(document).ready(function () {

    connection.reconnecting(function () {
        console.log('reconnecting');
    });

    connection.stateChanged(function (change) {
        //console.log(change);
    });

    // request for GetDatesByUrl on server SiteMapHub by signalR
    $("#List").change(function () {
        var selected = $("#List option:selected").val();

        if (selected.startsWith("http")) {
            $.get(window.applicationBaseUrl + "api/url", { Url: selected })
                .done(function (result) {
                    onDates(result);
                })
                .fail(function (e) {
                    console.log(e);
                });
        }
    });

    // request for GetResultsForUrlByDate on server SiteMapHub by signalR
    $("#dates").change(function () {
        var date = $("#dates option:selected").val();
        var url = $("#List option:selected").val();
        //console.log(date, url);
        $.get(window.applicationBaseUrl + "api/url",
            {
                Url: url,
                Date: date
            })
            .done(function (result) {
                onResults(result);
            })
            .fail(function (e) {
                console.log(e);
            });
    });

    $('#process-url-button').click(function (e) {
        onProccessButtonClick(e);
    });
});

function onProccessButtonClick(e) {
    e.preventDefault();
    e.stopPropagation();
    $(".status-group").show();
    connection.start().done(function () {
        console.log('connection');
        $(".status").html("Loading");
        var url = $("input[name = 'urlForProcessing']").val();

        siteMapHubProxy.invoke('processUrl', url);
    });

    chart.series[0].setData([]);
    urls.clear();
    return false;
}

function AppViewModel() {
    var self = this;

    self.urlData = ko.observableArray();

    self.addUrl = function (url, time) {
        self.urlData.push({ Url: url, Time: time });
    };

    self.addSort = function (value, startVal, endVal) {

        var length = self.urlData().length;
        var start = typeof (startVal) != 'undefined' ? startVal : 0;
        var end = typeof (endVal) != 'undefined' ? endVal : length - 1;
        var m = start + Math.floor((end - start) / 2);

        if (length === 0) {
            self.urlData.push(value);
            return;
        }

        if (value.Time <= self.urlData()[end].Time) {
            self.urlData.splice(end + 1, 0, value);

            return;
        }

        if (value.Time >= self.urlData()[start].Time) { //!!
            self.urlData.splice(start, 0, value);
            return;
        }

        if (start >= end) {
            return;
        }

        if (value.Time <= self.urlData()[m].Time) {

            self.addSort(value, m + 1, end);
            return;
        }

        if (value.Time > self.urlData()[m].Time) {
            self.addSort(value, start, m - 1);
            return;
        }

        //we don't insert duplicates
    }


    self.jumpToFirstPage = function () {
        self.gridViewModel.currentPageIndex(0);
    };

    self.gridViewModel = new ko.simpleGrid.viewModel({
        data: self.urlData,
        columns: [
            { headerText: "Url", rowText: "Url" },
            { headerText: "Response time", rowText: "Time" }
        ],
        pageSize: 20
    });
    self.clear = function () {
        self.urlData([]);
    }
}

function onResults(results) {
    showChartAndTable();
    chart.series[0].setData(results.map(x => [x.Url, x.Times]), true, false);
    urls.clear();
    $.each(results,
        function (i, item) {
            urls.addUrl(item.Url, item.Times);
        });
}

function onDates(dates) {

    $('#dates')
        .find('option')
        .remove()
        .end();
    $('#dates').append($('<option>',
        {
            value: "select date",
            text: "select date"
        }));
    $("#dates").show();
    $.each(dates,
        function (i, item) {
            $('#dates').append($('<option>',
                {
                    value: item,
                    text: item
                }));
        });
}

function onItem(url, time) {
    urls.addSort({ Url: url, Time: time });
    chart.series[0].addPoint([url, time], true, false);
}