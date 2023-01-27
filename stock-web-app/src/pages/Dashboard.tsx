import './Dashboard.scss';
import { useEffect, useState } from "react";
import Backendservice from "../services/backendservice";
import { BasicStock, Loadable, LoadingInProgress, NotLoaded, Loaded, LoadingFailed, SwitchLoadable } from "../types/types";
import BasicStockListItem from '../components/BasicStockListItem'

function BasicStocks(){
    
    const [stocks, setStocks] = useState<Loadable<BasicStock[]>>(NotLoaded);

    useEffect(()=> {
        setStocks(LoadingInProgress)
        const backendservice = new Backendservice();
        backendservice
            .fetchStocks()
            .then(comps => setStocks(Loaded<BasicStock[]>(comps)))
            .catch(err => {
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