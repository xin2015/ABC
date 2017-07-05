function LBHighcharts(type, container) {
    this.chartType = type;
    this.container = container;
    this.useHTML = false;
    this.colors = ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066', '#eeaaee', '#55BF3B', '#DF5353'];
    this.creditsEnabled = false;
    this.exportingAllowHTML = false;
    this.exportingEnabled = true;
    this.exportingFilename = "chart";
    this.exportingSourceHeight = container.height();
    this.exportingSourceWidth = container.width();
    this.legendAligh = 'center';//left、center、right
    this.legendEnabled = true;
    this.legendLayout = 'horizontal';//horizontal、vertical
    this.legendUseHTML = this.useHTML;
    this.legendVerticalAlign = 'bottom';//top、middle、bottom
    this.series = [];
    this.subtitleText = null;
    this.subtitleUseHTML = this.useHTML;
    this.titleText = null;
    this.titleUseHTML = this.useHTML;
    this.tooltipEnabled = true;
    this.tooltipFormatter = null;
    this.tooltipShared = true;
    this.tooltipUseHTML = this.useHTML;
    this.tooltipValuePrefix = null;
    this.tooltipValueSuffix = null;
    this.xAxisCategories = [];
    this.xAxisTickInterval = null;
    this.yAxisMin = null;
    this.yAxisPlotLines = [];
    this.yAxisTitleText = null;
    this.DrawChart = function () {
        this.container.highcharts({
            lang: {
                contextButtonTitle: '导出图片',
                loading: '加载中...',
                noData: '没有数据'
            },
            chart: {
                type: this.chartType
            },
            colors: this.colors,
            credits: {
                enabled: this.creditsEnabled
            },
            exporting: {
                allowHTML: this.exportingAllowHTML,
                buttons: {
                    contextButton: {
                        menuItems: null,
                        onclick: function () {
                            this.exportChartLocal();
                        }
                    }
                },
                enabled: this.exportingEnabled,
                filename: this.exportingFilename,
                sourceHeight: this.exportingSourceHeight,
                sourceWidth: this.exportingSourceWidth
            },
            legend: {
                aligh: this.legendAligh,
                enabled: this.legendEnabled,
                layout: this.legendLayout,
                useHTML: this.legendUseHTML,
                verticalAligh: this.legendVerticalAlign
            },
            series: this.series,
            subtitle: {
                text: this.subtitleText,
                useHTML: this.subtitleUseHTML,
            },
            title: {
                text: this.titleText,
                useHTML: this.titleUseHTML
            },
            tooltip: {
                enabled: this.tooltipEnabled,
                formatter: this.tooltipFormatter,
                shared: this.tooltipShared,
                useHTML: this.tooltipUseHTML,
                valuePrefix: this.tooltipValuePrefix,
                valueSuffix: this.tooltipValueSuffix
            },
            xAxis: {
                categories: this.xAxisCategories,
                tickInterval: this.xAxisTickInterval
            },
            yAxis: {
                min: this.yAxisMin,
                plotLines: this.yAxisPlotLines,
                title: {
                    text: this.yAxisTitleText
                }
            }
        });
    };
    this.SetData = function (jsonData, xAxisProperty, yAxisProperty) {
        var xAxis = true;
        for (var i in jsonData) {
            var array = jsonData[i];
            if (xAxis) {
                for (var j in array) {
                    this.xAxisCategories.push(array[j][xAxisProperty]);
                }
                xAxis = false;
            }
            var data = [];
            for (var j in array) {
                data.push(array[j][yAxisProperty]);
            }
            this.series.push({
                name: i,
                data: data
            });
        }
        this.DrawChart();
    };
}