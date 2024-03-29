import 'chart.js/auto';
import 'chartjs-adapter-moment';
import { ChartData, TimeUnit } from 'chart.js/auto';
import { Line } from "react-chartjs-2";
import { useEffect, useState } from 'react';
import Backendservice from '../services/backendservice';
import { StockPrice, StockPriceInterval } from '../types/types';

type StockChartState = {
    chartConfig: {
        unit: TimeUnit
    },
    dataPoints: { x: Date, y: number }[],
}

function StockChart({ symbol, interval, livePrice }: { symbol: string, interval: StockPriceInterval, livePrice: StockPrice }) {
    const [state, setState] = useState<StockChartState>({
        chartConfig: { unit: "day" },
        dataPoints: []
    });
    

    useEffect(() => {
        const backendservice = new Backendservice();
        switch(interval){
            case StockPriceInterval.day:
                backendservice
                    .fetchStockDailyPrices(symbol)
                    .then(prices => {
                        setState({
                            chartConfig: { unit: "hour" },
                            dataPoints: prices.map(p => ({ x: p.date, y: p.value }))
                        })
                    });
                break;
            case StockPriceInterval.year:
                backendservice
                    .fetchStockYearlyPrices(symbol)
                    .then(prices => {
                        setState({
                            chartConfig: { unit: "day" },
                            dataPoints: prices.map(p => ({ x: p.date, y: p.value }))
                        })
                    });
                break;
            case StockPriceInterval.live:
                setState({
                    chartConfig: { unit: "second" },
                    dataPoints: []
                });
        }

    }, [symbol, interval]);

    useEffect(() => {
        if(interval === StockPriceInterval.live){
            let p = {x: livePrice.date, y: livePrice.value};
            setState(s => ({...s, dataPoints: [...s.dataPoints, p]}));
        }
    }, [interval, livePrice]);

    const data: ChartData<"line", { x: Date, y: number }[]> = {
        datasets: [{
            backgroundColor: '#067194',
            borderColor: '#067194',
            data: state.dataPoints,
        }]
    };

    return (<Line data={data} options={{
        scales: {
            xAxis: {
                type: "time",
                time: {
                    unit: state.chartConfig.unit,
                    stepSize: 1
                },
            }
        },
        animation: false,
        plugins: {
            legend: {
                display: false,
            }
        }
    }} />);
}

export default StockChart;