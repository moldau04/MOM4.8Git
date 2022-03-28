function sum(data) {
    var total = 0;
    for (i = 0; i < data.length; i++) {
        total += data[i].value;
    }
    return total;
}

function createDonutChart(div, data, theme, title, name) {
    var total = sum(data.d);
    div.kendoChart({
        theme: theme,
        title: {
            //position: "bottom",
            //text: "Total " + total + '\n' + title
        },
        legend: {
            labels: {
                template: "#= text #: #= value#",
                font: "14px Lato",
                padding: 2,
                spacing: 100
            },
            visible: true,
            position: "bottom",
            align: "center",
            spacing: 30

        },
        chartArea: {
            background: ""
        },
        seriesDefaults: {
            type: "donut",
            startAngle: 150,
            holesize: 100,
            labels: {
                template: "#= category # - #= value#",
                position: "outsideEnd",
                visible: false,
                background: "transparent"
            }
        },
        series: [{
            name: name,
            data: data.d,
            aggregate: "sum"
        }],
        tooltip: {
            visible: true,
            template: "#= category# - #= value #"
        },

    });

}


function createBarChart(div, data, title) {
    div.kendoChart({
        theme: "material",
        title: {
            text: title,
            position: "bottom",
            visible: false
        },
        legend: {
            labels: {
                font: "14px Lato",
                padding: 2,
                spacing: 100
            },
            visible: true,
            position: "bottom",
            align: "center",
            spacing: 60
        },
        seriesDefaults: {
            type: "bar",
            style: "smooth"
        },
        series: [{
            name: data.d.LocationStatus[0].StatusName,
            data: data.d.LocationStatus[0].Count,
            type: "column"
        }, {
            name: data.d.LocationStatus[1].StatusName,
            data: data.d.LocationStatus[1].Count,
            type: "column"
        }],
        valueAxis: {
            max: data.d.Max,
            title: { text: "Hours" },
            line: {
                visible: false
            },
            majorGridLines: {
                visible: false
            },
            minorGridLines: {
                visible: true
            },
            labels: {
                rotation: "auto"
            },
        },
        categoryAxis: {
            categories: data.d.Categories,
            majorGridLines: {
                visible: false
            },
            position: "bottom"
        },
        tooltip: {
            visible: true,
            template: "#= series.name #: #= value #"
        }
    });
}

function toCurrencyFormat(valueToFormat) {
    
        var value = valueToFormat.toFixed(2).replace(/./g, function (c, i, a) {
            return i && c !== "." && ((a.length - i) % 3 === 0) ? ',' + c : c;
        });
        return value;
}

function createLineChart(div, data, title) {
   
    div.kendoChart({
        theme: "material",
        title: {
            text: title,
            position: "bottom",
            visible: false
        },
        legend: {
            visible: true,
            position: "bottom",
            font: "14px Lato",
            spacing: 60
        },
        seriesDefaults: {
            type: "line",
            style: "smooth"
        },
        series: [{
            name: data.d.LocationStatus[0].StatusName,
            data: formatArray(data.d.LocationStatus[0].Count),            
        }, {
            name: data.d.LocationStatus[1].StatusName,
            data: formatArray(data.d.LocationStatus[1].Count),
        }],
        valueAxis: {
            max: data.d.Max,
            line: {
                visible: false
            },
            majorGridLines: {
                visible: false
            },
            minorGridLines: {
                visible: false
            },
            labels: {
                rotation: "auto"
            }
        },
        categoryAxis: {
            categories: data.d.Categories,
            majorGridLines: {
                visible: false
            },
            position: "bottom"
        },
        tooltip: {
            visible: true,
            template: "#= series.name #: #= value #"
        }
    });
}

function createLineChartLines(div, data, title) {    
    div.kendoChart({
        theme: "material",
        title: {
            text: title,
            position: "bottom",
            visible: false
        },
        legend: {
            visible: true,
            position: "bottom",
            font: "14px Lato",
            spacing: 60
        },
        seriesDefaults: {
            type: "line",
            style: "smooth"
        },
        series: [{
            name: "Actual Revenues",
            data: data.d.LocationStatus[0].Count,
            tooltip: {
                visible: true,
                template: "$#= toCurrencyFormat(value) #"
            },
        }, {
            name: "Budgeted Revenues",
            data: data.d.LocationStatus[1].Count,
            tooltip: {
                visible: true,
                template: "$#= toCurrencyFormat(value) #"
            },
        }],
        valueAxis: {
            max: data.d.Max,
            line: {
                visible: false
            },
            majorGridLines: {
                visible: true
            },
            minorGridLines: {
                visible: true
            },
            labels: {
                rotation: "auto",
            },
            axisCrossingValue: '-' + data.d.Max 
        },
        categoryAxis: {
            categories: data.d.Categories,
            majorGridLines: {
                visible: false
            },
            position: "bottom"
        },
        tooltip: {
            visible: true,
            template: "#= series.name #: #= value #"
        }
    });
}

function createBarChart3Bars(div, data, title) {
    div.kendoChart({
        theme: "material",
        title: {
            text: title,
            position: "bottom",
            visible: false,
        },
        legend: {
            visible: true,
            position: "bottom",
            font: "14px Lato",
            spacing: 60
        },
        seriesDefaults: {
            type: "bar",
            style: "smooth"
        },
        series: [{
            name: data.d.LocationStatus[0].StatusName,
            data: data.d.LocationStatus[0].Count,
            type: "column"
        }, {
            name: data.d.LocationStatus[1].StatusName,
            data: data.d.LocationStatus[1].Count,
            type: "column"
            },
        {
            name: data.d.LocationStatus[2].StatusName,
            data: data.d.LocationStatus[2].Count,
            type: "column"
        }
        ],
        valueAxis: {   
            title: { text: "Hours", font: "12px Arial, Helvetica, sans-serif"},            
            max: data.d.Max,
            line: {
                visible: true
            },
            majorGridLines: {
                visible: true
            },
            minorGridLines: {
                visible: false
            },
            labels: {
                rotation: "auto"               
            },

        },
        categoryAxis: {
            categories: data.d.Categories,
            majorGridLines: {
                visible: true
            },
            position: "bottom"
        },
        tooltip: {
            visible: true,
            template: "#= series.name #: #= value #"
        }
    });
}
function createBarChart1Bars(div, data, title) {
    div.kendoChart({
        theme: "material",
        title: {
            text: title,
            position: "bottom",
            visible: false
        },
        legend: {
            labels: {
                font: "14px Lato",
                padding: 2,
                spacing: 100
            },
            visible: true,
            position: "bottom",
            align: "center",
            spacing: 60
        },
        seriesDefaults: {
            type: "bar",
            style: "smooth"
        },
        series: [{
            name: data.d.LocationStatus[0].StatusName,
            data: data.d.LocationStatus[0].Count,
            type: "column"
        }],
        valueAxis: {
            max: data.d.Max,
            title: { text: "Hours" },
            line: {
                visible: false
            },
            majorGridLines: {
                visible: false
            },
            minorGridLines: {
                visible: true
            },
            labels: {
                rotation: "auto"
            },
        },
        categoryAxis: {
            categories: data.d.Categories,
            majorGridLines: {
                visible: false
            },
            position: "bottom"
        },
        tooltip: {
            visible: true,
            template: "#= series.name #: #= value #"
        }
    });
}