import './Dashboard.css';
import { useEffect, useState } from "react";
import Backendservice from "../services/backendservice";
import { BasicStock } from "../types/types";

function ChangePointDetails({changePoint, changePercent} : {changePoint : number, changePercent: number}){
    const indicated_style = signSwitch(changePoint, "positive", "negative", "natural");
    const indicator = signSwitch(changePoint, "\u25B2", "\u25BC", "-");

    return (
    <div className={`changepoint-details ${indicated_style}`}>
        <div className='indicator'>
            {indicator}
        </div>
        <div className='changepoints'>
            <div className='changepoint'>{changePoint}</div>
            <div className='changepercent'>({changePercent}%)</div>
        </div>
    </div>);
}

function CompanyListItem({data} : {data: BasicStock}){
    const company = data.company;
    return (<div className="basic-stock-box">
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
            </div>);
}

function BasicStocks(){
    
    const [stocks, setStocks] = useState<BasicStock[]>([]);

    useEffect(()=> {
        const backendservice = new Backendservice();
        backendservice
            .fetchCompanies()
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
            {stocks.map(i => (<CompanyListItem data={i} />))}
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

function signSwitch<T>(n: number, pos: T, neg: T, nat: T) : T{
    var sign = Math.sign(n);

        if(sign == 1){
            return pos
        }

        if(sign == -1){
            return neg;
        }

        return nat;
}

export default Dashboard;