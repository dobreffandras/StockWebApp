import './StockDetails.scss';
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Backendservice from "../services/backendservice";
import { Stock } from "../types/types";
import ChangePointDetails from './ChangePointDetails';

function StockDetails(){
    const { symbol } = useParams();
    const backendservice = new Backendservice();
    const initialStock : Stock = {
        company: {symbol : symbol!, name: "?", exchange: "?"},
        currency: "?",
        price: 0,
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
    const company = stock.company;

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