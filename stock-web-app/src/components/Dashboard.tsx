import { useEffect, useState } from "react";
import Backendservice from "../services/backendservice";
import { BasicStock, Company } from "../types/types";

function CompanyListItem({data} : {data: BasicStock}){
    const company = data.company;
    const price = (<div>{data.stockPrice} ${data.currency}</div>);
    const sign = data.changePoint >= 0 ? "+" : "-"; 
    return (<div>
                {company.name}({company.symbol}) @{company.exchange} 
                {price}
                <div>{sign} {data.changePoint} ({data.changePercentage}%)</div>
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
        <div>
            {stocks.map(i => (<li><CompanyListItem data={i} /></li>))}
        </div>
    </>)
}

function Dashboard(){
    return (
    <div>
        <h1>Dashboard</h1>
        <BasicStocks />
    </div>)
}

export default Dashboard;