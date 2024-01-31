import { MenuItem } from "../types/types";
import styles from "../styles/Menu.module.scss"

function Menu({items} : {items: MenuItem[]}){
    return (
    <div className={styles["Menu"]}>
        {items.map(i => (<div className={styles['Menu-item']} key={i.label}><a href={i.link}>{i.label}</a></div>))}
    </div>)
}

export default Menu;