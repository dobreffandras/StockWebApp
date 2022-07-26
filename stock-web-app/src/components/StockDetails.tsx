import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Backendservice from "../services/backendservice";
import { Stock } from "../types/types";

function StockDetails(){
    const { symbol } = useParams();
    const backendservice = new Backendservice();
    const initialStock : Stock = {
        company: {symbol : symbol!, name: "?", exchange: "?"},
        currency: "?",
        stockPrice: 0,
        changePoint: 0,
        changePercent: 0,
        previousClose: 0,
        open: 0,
        dailyRange: { low: 0, high: 0},
        yearlyRange: { low: 0, high: 0},
        dividend: 0,
        dividendYield: 0,
        marketCap: 0
    }
    const [stock, setStock] = useState<Stock>(initialStock); // TODO change to loading

    useEffect(() => {
        backendservice
            .fetchStock(symbol!)
            .then(s => setStock(s))
            .catch(err => {
                console.log(err);
                setStock(initialStock);
            });
    }, []);

    return (
        <div>
            <div>{stock.company.symbol}</div>
            <div>{stock.company.name}</div>
            <div>{stock.company.exchange}</div>
            <div>{stock.stockPrice}</div>
            <div>{stock.currency}</div>
            <div>{stock.changePoint}</div>
            <div>{stock.changePercent}</div>
            <div>{stock.previousClose}</div>
            <div>{stock.open}</div>
            <div>{stock.dailyRange.low}-{stock.dailyRange.high}</div>
            <div>{stock.yearlyRange.low}-{stock.yearlyRange.high}</div>
            <div>{stock.dividend}</div>
            <div>{stock.dividendYield}</div>
            <div>{stock.marketCap}</div>
        </div>);
}

export default StockDetails;