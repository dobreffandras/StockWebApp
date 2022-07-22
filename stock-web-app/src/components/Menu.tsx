import './Menu.css';
import { MenuItem } from "../types/types";

function Menu({items} : {items: MenuItem[]}){
    return (
    <div className="Menu">
        {items.map(i => (<div className='Menu-item'><a href={i.link}>{i.label}</a></div>))}
    </div>)
}

export default Menu;