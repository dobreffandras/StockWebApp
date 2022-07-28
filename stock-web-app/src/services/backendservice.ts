import { BasicStock, Stock, StockPrice } from "../types/types";
const host = process.env.REACT_APP_BACKEND_HOST;


function fetchJson<T>(url: string, searchParams: {[key:string] : string} = {}) : Promise<T> {
    var u = new URL(url);
    for(var paramKey in searchParams){
        u.searchParams.append(paramKey, searchParams[paramKey]);
    }
    
    return fetch(u).then(res => res.json());
}

class Backendservice {
    fetchStocks() : Promise<BasicStock[]> {
        return fetchJson<BasicStock[]>(`${host}/stocks`);
    }

    fetchStock(symbol: string) : Promise<Stock> {
        return fetchJson<Stock>(`${host}/stocks/${symbol}`);
    }

    fetchStockPrices(symbol: string, interval : string = "year") {
        return fetchJson<StockPrice[]>(`${host}/stocks/${symbol}/prices`, {interval: interval});
    }    
}

export default Backendservice;