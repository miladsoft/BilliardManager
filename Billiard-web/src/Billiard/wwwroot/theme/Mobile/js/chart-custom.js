$(function () {
 'use strict';

/*=========================================
Area Chart
===========================================*/
//===== Chart1 =====//
var area = document.getElementById("chart1").getContext("2d");

var gradientFill = area.createLinearGradient(0, 120, 0, 0);
gradientFill.addColorStop(0, "rgba(247,147,26,0.42)");
gradientFill.addColorStop(1, "rgba(255,255,255,0)");
 new Chart(area, {
    type: 'line',
    data: {
        labels:["Fri","Sat", "Sun", "Mon", "Tue", "Wed", "Thu"],
        datasets: [{
            responsive: true,
            pointRadius: 0,
            fill: true,
            backgroundColor: gradientFill,
            borderColor: '#f7931a',
            borderWidth: 2,
            data: [7,9,4.3,3.8,5.2,1.8,2,5.3,7,6,6],
        }]
    },
    options: {
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                gridLines: {
                    drawBorder: false,
                    display:false
                },
                ticks: {
                    display:false, // hide main x-axis line
                    beginAtZero:true
                },
                barPercentage: 1.8,
                categoryPercentage: 0.2
            }],
            yAxes: [{
                gridLines: {
                    drawBorder: false, // hide main y-axis line
                    display:false
                },
                ticks: {
                    display:false,
                    beginAtZero:true
                },
            }]
        },
        tooltips: {
            enabled: false
        }
    }
});

    //===== Chart2 =====//
    var area = document.getElementById("chart2").getContext("2d");

    var gradientFill_red = area.createLinearGradient(0, 120, 0, 0);
    gradientFill_red.addColorStop(0, "rgba(185,34,105,0.42)");
    gradientFill_red.addColorStop(1, "rgba(255,255,255,0)");
    new Chart(area, {
        type: 'line',
        data: {
            labels:["Fri","Sat", "Sun", "Mon", "Tue", "Wed", "Thu"],
            datasets: [{
                responsive: true,
                pointRadius: 0,
                fill: true,
                backgroundColor: gradientFill_red,
                borderColor: '#B92269',
                borderWidth: 2,
                data: [1,3,4.3,3.8,5.2,6.8,2,4.3,3.8,5.3,3.8],
            }]
        },
        options: {
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        drawBorder: false,
                        display:false
                    },
                    ticks: {
                        display:false, // hide main x-axis line
                        beginAtZero:true
                    },
                    barPercentage: 1.8,
                    categoryPercentage: 0.2
                }],
                yAxes: [{
                    gridLines: {
                        drawBorder: false, // hide main y-axis line
                        display:false
                    },
                    ticks: {
                        display:false,
                        beginAtZero:true
                    },
                }]
            },
            tooltips: {
                enabled: false
            }
        }
    });

    //===== Chart3 =====//
    var area = document.getElementById("chart3").getContext("2d");

    var gradientFill_green = area.createLinearGradient(0, 120, 0, 0);
    gradientFill_green.addColorStop(0, "rgba(34,185,115,0.42)");
    gradientFill_green.addColorStop(1, "rgba(255,255,255,0)");
    new Chart(area, {
        type: 'line',
        data: {
            labels:["Fri","Sat", "Sun", "Mon", "Tue", "Wed", "Thu"],
            datasets: [{
                responsive: true,
                pointRadius: 0,
                fill: true,
                backgroundColor: gradientFill_green,
                borderColor: '#22B973',
                borderWidth: 2,
                data: [3.8,5.2,6.8,2,4,3,4.3,1.3,6.8,5.3,7.8],
            }]
        },
        options: {
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        drawBorder: false,
                        display:false
                    },
                    ticks: {
                        display:false, // hide main x-axis line
                        beginAtZero:true
                    },
                    barPercentage: 1.8,
                    categoryPercentage: 0.2
                }],
                yAxes: [{
                    gridLines: {
                        drawBorder: false, // hide main y-axis line
                        display:false
                    },
                    ticks: {
                        display:false,
                        beginAtZero:true
                    },
                }]
            },
            tooltips: {
                enabled: false
            }
        }
    });

    //===== Chart3 =====//
    var area = document.getElementById("chart4").getContext("2d");

    var gradientFill3 = area.createLinearGradient(0, 120, 0, 0);
    gradientFill3.addColorStop(0, "rgba(132,94,194,0.42)");
    gradientFill3.addColorStop(1, "rgba(255,255,255,0)");
    new Chart(area, {
        type: 'line',
        data: {
            labels:["Fri","Sat", "Sun", "Mon", "Tue", "Wed", "Thu"],
            datasets: [{
                responsive: true,
                pointRadius: 0,
                fill: true,
                backgroundColor: gradientFill3,
                borderColor: '#845EC2',
                borderWidth: 2,
                data: [4.3,1.3,6.8,5.3,7.8,3.8,5.2,6.8,2,4,3],
            }]
        },
        options: {
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        drawBorder: false,
                        display:false
                    },
                    ticks: {
                        display:false, // hide main x-axis line
                        beginAtZero:true
                    },
                    barPercentage: 1.8,
                    categoryPercentage: 0.2
                }],
                yAxes: [{
                    gridLines: {
                        drawBorder: false, // hide main y-axis line
                        display:false
                    },
                    ticks: {
                        display:false,
                        beginAtZero:true
                    },
                }]
            },
            tooltips: {
                enabled: false
            }
        }
    });

});