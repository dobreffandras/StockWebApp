import './Dashboard.scss';
import { useEffect, useState } from "react";
import Backendservice from "../services/backendservice";
import { BasicStock } from "../types/types";
import ChangePointDetails from '../components/ChangePointDetails';

function BasicStockListItem({data} : {data: BasicStock}){
    const company = data.company;
    return (
        <a href={company.symbol}>
            <div className="basic-stock-box">
                <header>{company.name}</header>
                <div className='sub-header'>({company.symbol}) @{company.exchange}</div> 
                <div className='stock-details'>
                    <div className='price-details'>
                        {data.stockPrice} {data.currency}
                    </div>
                    <ChangePointDetails 
                        changePoint={data.changePoint}
                        changePercent={data.changePercent}
                        />
                </div>
            </div>
        </a>);
}

function BasicStocks(){
    
    const [stocks, setStocks] = useState<BasicStock[]>([]);

    useEffect(()=> {
        const backendservice = new Backendservice();
        backendservice
            .fetchStocks()
            .then(comps => setStocks(comps))
            .catch(err =>{
                console.log(err);
                setStocks([]);
            });
    }, []);

    return (
    <>
        <h2>Companies</h2>
        <div className='stock-boxes-container'>
            {stocks.map(i => (<BasicStockListItem data={i} key={i.company.symbol} />))}
        </div>
    </>)
}

function Dashboard(){
    return (
    <div className="dashboard">
        <h1>Dashboard</h1>
        <BasicStocks />
    </div>)
}

export default Dashboard;