import './StockDetails.scss';
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Backendservice from "../services/backendservice";
import { Loadable, Loaded, LoadingFailed, LoadingInProgress, NotLoaded, Stock } from "../types/types";
import ChangePointDetails from '../components/ChangePointDetails';

function StockDetails(){
    const { symbol } = useParams();
    const backendservice = new Backendservice();
    
    const [stock, setStock] = useState<Loadable<Stock>>({type: "notloaded"} as NotLoaded);

    useEffect(() => {
        setStock({type: "loadinginprogress"} as LoadingInProgress);

        backendservice
            .fetchStock(symbol!)
            .then(s => setStock({type: "loaded", data: s} as Loaded<Stock>))
            .catch(err => {
                console.log(err);
                setStock({type: "loadingfailed", error: err} as LoadingFailed);
            });
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    switch(stock.type){
        case "loaded":
            return (<StockDetailsLoaded stock={stock.data} />);
        case "loadingfailed":
            return (<>Loading failed. Please refresh the page.</>);
        case "loadinginprogress":
            return (<>Loading...</>);
        case "notloaded":
            return (<>Not loaded.</>);
    }
}

function StockDetailsLoaded({stock} : {stock: Stock}){
    const company = stock.company;
    return (
        <div className="stockPage">
            <header>
                <h1 className='company-name'>{stock.company.name}</h1>
                <div className='sub-header'>({company.symbol}) @{company.exchange}</div> 
            </header>
            <div className="content">
                <div className="left-sidebar">
                    <div className="sidebar-header">
                        <div className='price'>{stock.price} {stock.currency}</div>
                        <div className='changepont-container'>
                            <ChangePointDetails
                                changePoint={stock.changePoint}
                                changePercent={stock.changePercent} />
                        </div>
                    </div>
                    <div className="additional-data">
                        <table>
                            <tr>
                                <td className='property-name'>Previous close:</td>
                                <td>{stock.previousClose}</td>
                            </tr>
                            <tr>
                                <td className='property-name'>Open:</td>
                                <td>{stock.open}</td>
                            </tr>
                            <tr>
                                <td className='property-name'>Daily range:</td>
                                <td>{stock.dailyRange.low}-{stock.dailyRange.high}</td>
                            </tr>
                            <tr>
                                <td className='property-name'>Yearly range:</td>
                                <td>{stock.yearlyRange.low}-{stock.yearlyRange.high}</td>
                            </tr>
                            {   
                                stock.dividend &&
                                (<tr>
                                    <td className='property-name'>Dividend (Yield):</td>
                                    <td>{stock.dividend} ({stock.dividendYield}%)</td>
                                </tr>)
                            }
                            <tr>
                                <td className='property-name'>Market cap:</td>
                                <td>{toBillion(stock.marketCap)}</td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div className="chart-container">
                    <div className="chart"></div>
                </div>
            </div>
        </div>);
}

function toBillion(n: number){
    var converted = n / 1_000_000_000;
    return `${converted} B`;
}

export default StockDetails;