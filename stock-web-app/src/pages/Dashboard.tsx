import './Dashboard.scss';
import { useEffect, useState } from "react";
import Backendservice from "../services/backendservice";
import { BasicStock, Loadable, LoadingInProgress, NotLoaded, Loaded, LoadingFailed, SwitchLoadable } from "../types/types";
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
    
    const [stocks, setStocks] = useState<Loadable<BasicStock[]>>(NotLoaded);

    useEffect(()=> {
        setStocks(LoadingInProgress)
        const backendservice = new Backendservice();
        backendservice
            .fetchStocks()
            .then(comps => setStocks(Loaded<BasicStock[]>(comps)))
            .catch(err =>{
                setStocks(LoadingFailed(err));
            });
    }, []);

    return SwitchLoadable(
        stocks,
        _ => (<>Not loaded.</>),
        _ => (<>Loading...</>),
        l => (
            <>
                <h2>Companies</h2>
                <div className='stock-boxes-container'>
                    {l.data.map(i => (<BasicStockListItem data={i} key={i.company.symbol} />))}
                </div>
            </>),
        _ => (<>Loading failed. Please refresh the page.</>))
}

function Dashboard(){
    return (
    <div className="dashboard">
        <h1>Dashboard</h1>
        <BasicStocks />
    </div>)
}

export default Dashboard;