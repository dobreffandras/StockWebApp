import { BasicStock, Stock, StockPrice } from "../types/types";
const host = process.env.REACT_APP_BACKEND_HOST;


function fetchJson<T>(url: string) : Promise<T> {
    return fetch(url).then(res => res.json());
}

class Backendservice {
    fetchStocks() : Promise<BasicStock[]> {
        return fetchJson<BasicStock[]>(`${host}/stocks`);
    }

    fetchStock(symbol: string) : Promise<Stock> {
        return fetchJson<Stock>(`${host}/stocks/${symbol}`);
    }

    fetchStockPrices(symbol: string) {
        return fetchJson<StockPrice[]>(`${host}/stocks/${symbol}/prices`);
    }    
}

export default Backendservice;