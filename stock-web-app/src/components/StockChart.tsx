import 'chart.js/auto';
import 'chartjs-adapter-moment';
import { ChartData, ChartDataset, ChartDatasetProperties } from 'chart.js/auto';
import { Line } from "react-chartjs-2";
import { useEffect, useState } from 'react';

function StockChart() {
    const [dataPoints , setDataPoints] = useState<{x: Date, y: number}[]>([]);

    useEffect(()=>{
        setDataPoints([
            {
                x: new Date('2016-12-25'),
                y: 20
            },
            {
                x: new Date('2016-12-26 8:00:00'),
                y: 11
            },
            {
                x: new Date('2016-12-26 10:00:00'),
                y: 12
            },
            {
                x: new Date('2016-12-26 11:00:00'),
                y: 9
            },
            {
                x: new Date('2016-12-26 12:00:00'),
                y: 12
            },
            {
                x: new Date('2016-12-30'),
                y: 10
            }]);
    }, []);

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
                    unit: 'hour',
                    stepSize: 6
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