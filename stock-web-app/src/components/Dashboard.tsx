import { useEffect, useState } from "react";
import Backendservice from "../services/backendservice";
import { Company } from "../types/types";

function CompanyListItem({data} : {data: Company}){
    return (<div>{data.name}({data.symbol}) @{data.exchange} </div>);
}

function Companies(){
    
    const [companies, setCompanies] = useState<Company[]>([]);

    useEffect(()=> {
        const backendservice = new Backendservice();
        backendservice
            .fetchCompanies()
            .then(comps => setCompanies(comps))
            .catch(err =>{
                console.log(err);
                setCompanies([]);
            });
    }, []);

    return (
    <>
        <h2>Companies</h2>
        <div>
            {companies.map(i => (<li><CompanyListItem data={i} /></li>))}
        </div>
    </>)
}

function Dashboard(){
    return (
    <div>
        <h1>Dashboard</h1>
        <Companies />
    </div>)
}

export default Dashboard;