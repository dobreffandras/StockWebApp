import './StockDetails.scss';
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Backendservice from "../services/backendservice";
import { Loadable, Loaded, LoadingFailed, LoadingInProgress, NotLoaded, Stock, StockPrice, StockPriceInterval, SwitchLoadable } from "../types/types";
import ChangePointDetails from '../components/ChangePointDetails';
import StockChart from '../components/StockChart';

function StockDetails() {
    const { symbol } = useParams();
    const backendservice = new Backendservice();

    const [stock, setStock] = useState<Loadable<Stock>>(NotLoaded);

    useEffect(() => {
        setStock(LoadingInProgress);

        backendservice
            .fetchStock(symbol!)
            .then(s => setStock(Loaded(s)))
            .catch(err => {
                console.log(err);
                setStock(LoadingFailed(err));
            });
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return SwitchLoadable(
        stock,
        _ => (<>Not loaded.</>),
        _ => (<>Loading...</>),
        l => (<StockDetailsLoaded stock={l.data} />),
        _ => (<>Loading failed. Please refresh the page.</>));
}

function StockDetailsLoaded({ stock }: { stock: Stock }) {
    const [selectedInterval, setSelectedInterval] = useState("year");
    const [stockState, setStockState] = useState(stock);
    const [livePrice, setLivePrice] = useState<StockPrice>({date: new Date(), value: stock.price});

    const radioHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSelectedInterval(event.target.value);
    };
    
    useEffect(()=> {
        const backendservice = new Backendservice();
        let unsubscribe = backendservice.subscribeToLivePrices(stock.company.symbol, price => {
            setLivePrice(price);
            setStockState(s => ({...s, price: price.value}));
        });

        return unsubscribe;
    }, [stock.company.symbol]);

    const company = stockState.company;
    return (
        <div className="stockPage">
            <header>
                <a className='back-to-dashboard' href='/'>← Dashboard</a>
                <h1 className='company-name'>{company.name}</h1>
                <div className='sub-header'>({company.symbol}) @{company.exchange}</div>
            </header>
            <div className="content">
                <div className="left-sidebar">
                    <div className="sidebar-header">
                        <div className='price'>{stockState.price} {stockState.currency}</div>
                        <div className='changepont-container'>
                            <ChangePointDetails
                                changePoint={stockState.changePoint}
                                changePercent={stockState.changePercent} />
                        </div>
                    </div>
                    <div className="additional-data">
                        <table>
                            <tbody>
                                <tr>
                                    <td className='property-name'>Previous close:</td>
                                    <td>{stockState.previousClose}</td>
                                </tr>
                                <tr>
                                    <td className='property-name'>Open:</td>
                                    <td>{stockState.open}</td>
                                </tr>
                                <tr>
                                    <td className='property-name'>Daily range:</td>
                                    <td>{stockState.dailyRange.low}-{stockState.dailyRange.high}</td>
                                </tr>
                                <tr>
                                    <td className='property-name'>Yearly range:</td>
                                    <td>{stockState.yearlyRange.low}-{stockState.yearlyRange.high}</td>
                                </tr>
                                {
                                    stockState.dividend &&
                                    (<tr>
                                        <td className='property-name'>Dividend (Yield):</td>
                                        <td>{stockState.dividend} ({stockState.dividendYield}%)</td>
                                    </tr>)
                                }
                                <tr>
                                    <td className='property-name'>Market cap:</td>
                                    <td>{toBillion(stockState.marketCap)}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div className="chart-container">
                    <div className='interval-selector'>
                        <div className='interval-element'>
                            <input
                                type="radio"
                                id="live"
                                name="stockPriceInterval"
                                value={"live"}
                                checked={selectedInterval === "live"}
                                onChange={radioHandler} />
                            <label htmlFor="live">Live</label>
                        </div>
                        <div className='interval-element'>
                            <input
                                type="radio"
                                id="day"
                                name="stockPriceInterval"
                                value={"day"}
                                checked={selectedInterval === "day"}
                                onChange={radioHandler} />
                            <label htmlFor="day">1 Day</label>
                        </div>
                        <div className='interval-element'>
                            <input
                                type="radio"
                                id="year"
                                name="stockPriceInterval"
                                value={"year"}
                                checked={selectedInterval === "year"}
                                onChange={radioHandler} />
                            <label htmlFor="year">1 Year</label>
                        </div>
                    </div>
                    <div className="chart">
                        <StockChart
                            symbol={company.symbol}
                            interval={switchInterval(selectedInterval)}
                            livePrice={livePrice} />
                    </div>
                </div>
            </div>
        </div>);
}

function toBillion(n: number) {
    var converted = n / 1_000_000_000;
    return `${converted} B`;
}

function switchInterval(interval : string) : StockPriceInterval {
    switch(interval){
        case "year":
            return StockPriceInterval.year;
        case "day":
            return StockPriceInterval.day;
        case "live":
            return StockPriceInterval.live;
        default:
            return StockPriceInterval.year;
    }
}

export default StockDetails;