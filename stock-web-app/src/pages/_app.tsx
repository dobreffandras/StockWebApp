import {AppProps} from 'next/app';
import "../App.scss";
import "../components/ChangePointDetails.scss";
import "../components/Menu.scss"
import "../components/StockCard.scss";
import "./Dashboard.scss";
import "./StockDetails.scss"
import "./_document.scss"
import Menu from '../components/Menu';
import { MenuItem } from '../types/types';

const menuItems : MenuItem[] = [{
  label: "MenuItem1",
  link: "#"
},{
  label: "MenuItem2",
  link: "#"
},{
  label: "MenuItem3",
  link: "#"
}];

function App({Component, pageProps}: AppProps) {
  return (
    <div className="App">
    <header className="App-header">
      <Menu items={menuItems}/>
    </header>
    <Component {...pageProps} />
  </div>);
}
export default App;