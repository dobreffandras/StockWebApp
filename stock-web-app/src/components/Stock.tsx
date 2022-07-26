import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Backendservice from "../services/backendservice";
import { BasicStock } from "../types/types";

function Stock(){
    const { symbol } = useParams();
    const backendservice = new Backendservice();
    const initialStock = {
        company: {symbol : symbol!, name: "?", exchange: "?"},
        currency: "?",
        stockPrice: 0,
        changePoint: 0,
        changePercent: 0
    }
    const [stock, setStock] = useState<BasicStock>(initialStock); // TODO change to loading

    useEffect(() => {
        backendservice
            .fetchCompanies()
            .then(stocks => setStock(stocks[0])) //TODO change this dummy call to a specific stock
            .catch(err => {
                console.log(err);
                setStock(initialStock);
            });
    }, []);

    return (
        <div>
            {stock.company.symbol}
            {stock.company.name}
            {stock.company.exchange}
            {stock.stockPrice}
            {stock.currency}
            {stock.changePoint}
            {stock.changePercent}
        </div>);
}

export default Stock;