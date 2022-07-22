import { useState } from "react";
import './Menu.css';
import { MenuItem } from "../types/types";

function Menu(){
    const [items, setItems] = useState<MenuItem[]>([{Label: "Menu1", Link: "#"},{Label: "Menu2", Link: "#"}])

    return (
    <div className="Menu">
        {items.map(i => (<div className='Menu-item'><a href={i.Link}>{i.Label}</a></div>))}
    </div>)
}

export default Menu;