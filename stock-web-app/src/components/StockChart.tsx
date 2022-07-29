import 'chart.js/auto';
import 'chartjs-adapter-moment';
import { ChartData, ChartDataset, ChartDatasetProperties, TimeUnit } from 'chart.js/auto';
import { Line } from "react-chartjs-2";
import { useEffect, useState } from 'react';
import Backendservice from '../services/backendservice';
import { StockPriceInterval } from '../types/types';

function StockChart({symbol, interval} : {symbol: string, interval: StockPriceInterval}) {
    const [chartConfig, setChartConfig] = useState<{unit: TimeUnit}>({unit: "day"});
    const [dataPoints , setDataPoints] = useState<{x: Date, y: number}[]>([]);
    const backendservice = new Backendservice();

    useEffect(()=>{
        console.log(interval);
        if(interval === StockPriceInterval.day){
            backendservice
            .fetchStockDailyPrices(symbol)
            .then(prices =>{
                setChartConfig({unit: "hour"});
                setDataPoints(prices.map(p => ({x: p.date, y: p.value})));
            });
        } else {
            backendservice
            .fetchStockYearlyPrices(symbol)
            .then(prices =>{
                setChartConfig({unit: "day"});
                setDataPoints(prices.map(p => ({x: p.date, y: p.value})));
            });
        }
        
    }, [interval]);

    const data: ChartData<"line", { x: Date, y: number }[]> = {
        datasets: [{
            backgroundColor: '#067194',
            borderColor: '#067194',
            data: dataPoints,
        }]
    };

    return (<Line data={data} options={{
        scales: {
            xAxis: {
                type: "time",
                time: {
                    unit: chartConfig.unit,
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