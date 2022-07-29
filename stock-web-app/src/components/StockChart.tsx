import 'chart.js/auto';
import 'chartjs-adapter-moment';
import { ChartData, ChartDataset, ChartDatasetProperties, TimeUnit } from 'chart.js/auto';
import { Line } from "react-chartjs-2";
import { useEffect, useState } from 'react';
import Backendservice from '../services/backendservice';
import { StockPriceInterval } from '../types/types';

type StockChartState = {
    chartConfig: {
        unit: TimeUnit
    },
    dataPoints: {x: Date, y: number}[],
}

function StockChart({symbol, interval} : {symbol: string, interval: StockPriceInterval}) {
    const [state, setState] = useState<StockChartState>({
        chartConfig: {unit: "day"},
        dataPoints: []
    });
    const backendservice = new Backendservice();

    useEffect(()=>{
        console.log(interval);
        if(interval === StockPriceInterval.day){
            backendservice
            .fetchStockDailyPrices(symbol)
            .then(prices =>{
                setState({
                    chartConfig: {unit: "hour"},
                    dataPoints: prices.map(p => ({x: p.date, y: p.value}))
                })
            });
        } else {
            backendservice
            .fetchStockYearlyPrices(symbol)
            .then(prices =>{
                setState({
                    chartConfig: {unit: "day"},
                    dataPoints: prices.map(p => ({x: p.date, y: p.value}))
                })
            });
        }
        
    }, [interval]);

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
        plugins: {
            legend: {
                display: false,
            }
        }
    }} />);
}

export default StockChart;