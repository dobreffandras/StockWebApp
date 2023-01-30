import { StockBasicInfo, Stock, StockPrice } from "../types/types";
const host = process.env.REACT_APP_BACKEND_HOST;
const wsHost = process.env.REACT_APP_BACKEND_HOST_WS;


function fetchJson<T>(url: string, searchParams: {[key:string] : string} = {}) : Promise<T> {
    var u = new URL(url);
    for(var paramKey in searchParams){
        u.searchParams.append(paramKey, searchParams[paramKey]);
    }
    
    return fetch(u).then(res => res.json());
}

class Backendservice {
    fetchStocks() : Promise<StockBasicInfo[]> {
        return fetchJson<StockBasicInfo[]>(`${host}/stocks`);
    }

    fetchStock(symbol: string) : Promise<Stock> {
        return fetchJson<Stock>(`${host}/stocks/${symbol}`);
    }

    fetchStockYearlyPrices(symbol: string) {
        return fetchJson<StockPrice[]>(`${host}/stocks/${symbol}/prices`, {interval: "year"});
    }    

    fetchStockDailyPrices(symbol: string) {
        return fetchJson<StockPrice[]>(`${host}/stocks/${symbol}/prices`, {interval: "day"});
    }

    subscribeToLivePrices(symbol: string, onPriceChanged: (price: StockPrice) => void) : () => void {
        const ws = new WebSocket(`${wsHost}/stocks/${symbol}/prices/live`);

        ws.onmessage = function (event) {
            const json : StockPrice = JSON.parse(event.data);
            let date = json.date
            let price = Math.round((json.value + Number.EPSILON) * 100) / 100;
            onPriceChanged({date: date, value: price});
        };

        return () => { ws.close() };
    }
}

export default Backendservice;