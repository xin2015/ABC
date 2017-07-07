function PieHighcharts(container) {
    this.chartType = 'pie';
    this.container = container;
    this.useHTML = false;
    this.colors = ['#7cb5ec', '#434348', '#90ed7d', '#f7a35c', '#8085e9', '#f15c80', '#e4d354', '#2b908f', '#f45b5b', '#91e8e1'];
    this.creditsEnabled = false;
    this.exportingAllowHTML = false;
    this.exportingEnabled = true;
    this.exportingFilename = "chart";
    this.exportingSourceHeight = container.height();
    this.exportingSourceWidth = container.width();
    this.legendAlign = 'center';//left、center、right
    this.legendEnabled = true;
    this.legendLayout = 'horizontal';//horizontal、vertical
    this.legendUseHTML = this.useHTML;
    this.legendVerticalAlign = 'bottom';//top、middle、bottom
    this.plotOptions = {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            dataLabels: {
                enabled: true,
                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                style: {
                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                }
            }
        }
    };
    this.series = [];
    this.subtitleText = null;
    this.subtitleUseHTML = this.useHTML;
    this.titleText = null;
    this.titleUseHTML = this.useHTML;
    this.tooltipEnabled = true;
    this.tooltipFormatter = null;
    this.tooltipHeaderFormat = '{series.name}<br>';
    this.tooltipPointFormat = '{point.name}: <b>{point.percentage:.1f}%</b>';
    this.tooltipUseHTML = this.useHTML;
    this.tooltipValuePrefix = null;
    this.tooltipValueSuffix = null;
    this.option = {};
    this.DrawChart = function () {
        this.option = Highcharts.merge(this.option, {
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
                align: this.legendAlign,
                enabled: this.legendEnabled,
                layout: this.legendLayout,
                useHTML: this.legendUseHTML,
                verticalAlign: this.legendVerticalAlign
            },
            plotOptions: this.plotOptions,
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
                headerFormat: this.tooltipHeaderFormat,
                pointFormat: this.tooltipPointFormat,
                useHTML: this.tooltipUseHTML,
                valuePrefix: this.tooltipValuePrefix,
                valueSuffix: this.tooltipValueSuffix
            }
        });
        this.container.highcharts(this.option);
    };
    this.SetData = function (title, seriesName, jsonData) {
        this.titleText = title;
        this.exportingFilename = title;
        var data = [];
        for (var i in jsonData) {
            data.push({
                name: i,
                y: jsonData[i]
            });
        }
        this.series = [{
            name: seriesName,
            data: data
        }];
        this.DrawChart();
    }
}