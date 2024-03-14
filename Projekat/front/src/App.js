import logo from './logo.svg';
import './App.css';
import SvePonude from './Ponuda/SvePonude';
import {BrowserRouter as Router, Route, Routes } from 'react-router-dom';

import Kartica from './Ponuda/Kartica';

import KarticaAgencije from './Agencija/KarticaAgencije';
import Prijava from './PrijavaRegistracija/Prijava';
import Registracija from './PrijavaRegistracija/Registracija';
import DetaljnaPonuda from './Ponuda/DetaljnaPonuda';
import OkStranica from './PrijavaRegistracija/OkStranica';
import DetaljnaAgencija from './Agencija/DetaljnaAgencija';
import Cet from './Poruka/Cet';
import CitajAzuriraj from './CRUDKorisnik/CitajAzuriraj';
import CitajAzurirajPonudu from './Agencija/CitajAzurirajPonudu';
import DodajPonudu from './Ponuda/DodajPonudu';
import DodajAgenciju from './Administrator/DodajAgenciju';
import SveAgencije from './Administrator/SveAgencije';
import CitajAzurirajAgenciju from './Administrator/CitajAzurirajAgenciju';

function App() {
  return (
    <main className="App">
    <Router>
      <Routes>
        <Route path='/' element={<><SvePonude/></>} />
        <Route path='/Prijava' element={<><Prijava/></>}/>
        <Route path='/Registracija' element={<><Registracija/></>} />
        <Route path='/DetaljnaPonuda' element={<><DetaljnaPonuda/></>} />
        <Route path='/OkStranica' element={<><OkStranica/></>} />
        <Route path='/DetaljnaAgencija' element={<><DetaljnaAgencija/></>}/>
        <Route path='/Cet' element={<><Cet/></>}/>
        <Route path='/CitajAzurirajKorisnika'element={<><CitajAzuriraj/></>}/>
        <Route path='/CitajAzurirajPonudu'element={<><CitajAzurirajPonudu/></>}/>
        <Route path='/DodajPonudu'element={<><DodajPonudu/></>}/> 
        <Route path='/DodajAgenciju'element={<><DodajAgenciju/></>}/> 
        <Route path='/SveAgencije'element={<><SveAgencije/></>}/> 
        <Route path='/CitajAzurirajAgenciju'element={<><CitajAzurirajAgenciju/></>}/> 
      </Routes>
    </Router>
  </main>
  );
}

export default App;
