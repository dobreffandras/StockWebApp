import './ChangePointDetails.scss';

function ChangePointDetails({ changePoint, changePercent }: { changePoint: number; changePercent: number; }) {
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

export default ChangePointDetails;