import styles from "../styles/ChangePointDetails.module.scss"

function ChangePointDetails({ changePoint, changePercent }: { changePoint: number; changePercent: number; }) {
    const indicated_style = signSwitch(changePoint, "positive", "negative", "natural");
    const indicator = signSwitch(changePoint, "\u25B2", "\u25BC", "-");

    return (
        <div className={`${styles["changepoint-details"]} ${styles[indicated_style]}`}>
            <div className={styles['indicator']}>
                {indicator}
            </div>
            <div className={styles['changepoints']}>
                <div className={styles['changepoint']}>{changePoint}</div>
                <div className={styles['changepercent']}>({changePercent}%)</div>
            </div>
        </div>);
}


function signSwitch<T>(n: number, pos: T, neg: T, nat: T) : T{
    var sign = Math.sign(n);

        if(sign === 1){
            return pos
        }

        if(sign === -1){
            return neg;
        }

        return nat;
}

export default ChangePointDetails;