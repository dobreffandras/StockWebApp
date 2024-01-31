import { useEffect, useState } from "react";
import { StockBasicInfo } from "../types/types";
import Backendservice from "../services/backendservice";
import ChangePointDetails from './ChangePointDetails';
import styles from "../styles/StockCard.module.scss"

function StockCard({data} : {data: StockBasicInfo}) {
    const [state, setState] = useState<StockBasicInfo>(data);
    const company = data.company;

    useEffect(() => {
        const backendservice = new Backendservice();
        let unsubscribe = backendservice.subscribeToLivePrices(
            company.symbol,
            price => setState(state => ({...state, stockPrice: price.value })));
        return unsubscribe;
    }, [company.symbol]);
    
    return (
        <a href={company.symbol}>
            <div className={styles["basic-stock-box"]}>
                <header>{company.name}</header>
                <div className={styles["sub-header"]}>({company.symbol}) @{company.exchange}</div> 
                <div className={styles["stock-details"]}>
                    <div className={styles["price-details"]}>
                        {state.stockPrice} {state.currency}
                    </div>
                    <ChangePointDetails 
                        changePoint={state.changePoint}
                        changePercent={state.changePercent}
                        />
                </div>
            </div>
        </a>);
}

export default StockCard