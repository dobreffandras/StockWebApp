import './App.css';
import Dashboard from './components/Dashboard';
import Menu from './components/Menu';
import Main from './Main';
import { MenuItem } from './types/types';

function App() {

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

  return (
    <div className="App">
      <header className="App-header">
        <Menu items={menuItems}/>
      </header>
      <Main />
    </div>
  );
}

export default App;
