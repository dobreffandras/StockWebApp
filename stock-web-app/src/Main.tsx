import { Routes, Route } from 'react-router-dom';

import Dashboard from './components/Dashboard';
import Stock from './components/Stock';

const Main = () => {
  return (
    <Routes>
      <Route path='/' element={<Dashboard />}></Route>
      <Route path='/:symbol' element={<Stock />}></Route>
    </Routes>
  );
}

export default Main;