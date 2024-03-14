import { Button, Card, CardContent, Grid, Paper, Rating, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import Item from '@mui/material/ListItem'
import AppBarKorisnik from "../Navigacija/AppBarKorisnik";
import axios from "axios";
import Kartica from "../Ponuda/Kartica";
import KarticaKorisnika from "../Korisnik/KarticaKorisnika";
import AppBarAgencija from '../Navigacija/AppBarAgencija';
const DetaljnaAgencija=()=>
{
    const[value,setValue]=useState(0);
    const[agencija,setAgencija]=useState({});
    const idAgencije=localStorage.getItem('idAgencije');
    const id=localStorage.getItem('id');
    //localStorage.removeItem('idAgencije');
    const email=localStorage.getItem('email');
    const[ocena,setOcena]=useState(0);
    const[ucitaj,setUcitaj]=useState(false);
    //localStorage.removeItem('idPonude');
    const [ponude,setPonude]=useState([]);
    const[korisnici,setKorisnici]=useState([]);
    useEffect(() => {
 
        console.log('Uso sam u fjuSobe');
      const response =  axios.post(`https://localhost:7199/Administrator/PribaviAgencijuPoId/${idAgencije}`,
      {
        headers:{
          //Authorization: `Bearer ${token}`
        }
      }).then(response=>{
        setAgencija(response.data);
        setValue(1);
       
        console.log(response.data);
      })
      .catch(error=>{
        console.log(error);
        setValue(0);
      })
      const respons2 =  axios.get(`https://localhost:7199/Ponuda/PribaviSvePonudeAgencije/${idAgencije}`,
      {
        headers:{
          //Authorization: `Bearer ${token}`
        }
      }).then(response=>{
        setPonude(response.data);
        setValue(1);
        console.log(response.data);
      })
      .catch(error=>{
        console.log(error);
        setValue(0);
      })


     
        const respons3 =  axios.get(`https://localhost:7199/Poruka/PribaviKorisnikeAgencije/${idAgencije}`,
            {
              headers:{
                //Authorization: `Bearer ${token}`
              }
            }).then(response=>{
              setKorisnici(response.data);
              setValue(1);
              console.log(response.data);
            })
            .catch(error=>{
              console.log(error);
              setValue(0);
            })

      
       

        
        
    
      },[ucitaj]);
      const oceniHandler=(event)=>
      {
        event.preventDefault();
        const response =  axios.post(`https://localhost:7199/Administrator/DodajOcenuAgenciji/${idAgencije}/${ocena}`,
        {
            
        },
        {
          headers:{
            //Authorization: `Bearer ${token}`
          }
        }).then(response=>{
          if(ucitaj===false)
          {
                setUcitaj(true);
          }
          else
          {
            setUcitaj(false);
          }
          
         
          console.log(response.data);
        })
        .catch(error=>{
          console.log(error);
          setValue(0);
        })

      }
    return(
        <>
       {(email==null||email.startsWith('korisnik'))&&(<AppBarKorisnik/>)}
       {email.startsWith('agencija')&&(<AppBarAgencija/>)}
        
        {value===1&&(<Card sx={{ marginLeft:'380px',marginTop:'10px',width:'740px' ,height:'7800px',backgroundColor:'#c7d6ed',alignItems:'center',justifyContent:'center',alignContent:'center'}}>
      <CardContent sx={{marginTop:'-70px',padding:'100px'}}>
      <Typography sx={{ fontSize: 48,padding:'30px' }} color="text.primary" gutterBottom>
        Naziv Agencije 
        </Typography>
        <Typography sx={{ fontSize: 48,padding:'30px' }} color="#591905" gutterBottom>
        {agencija.naziv}
        </Typography>
        <hr/>
        <Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
         Telefon:</Typography>
          <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {agencija.telefon}</Typography> 
          <br/> 
          <Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
          Adresa:</Typography>
          <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {agencija.adresa}</Typography> 
          <br/> 
          <Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
          Email:</Typography>
          <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {agencija.email}</Typography> 
          <br/> 
          <CardContent>
      
      <Typography component="legend">Prosecna ocena</Typography>
      <Rating name="read-only" value={agencija.prosecnaOcena} readOnly />
      </CardContent>
      <Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
          Broj korisnika koji su agenciju ocenili:</Typography>
          <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {agencija.brojKorisnikaKojiSuOcenili}</Typography> 
          <br/> 
        
        
        <hr/>
        {email.startsWith('agencija')===false&&(<><Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
          Dajte ocenu agenciji:</Typography>
        <Rating name="size-large" defaultValue={ocena} size="large"  onChange={(event, newValue) => {
    setOcena(newValue);
  }}/>
  <Button variant='contained' disabled={email===null?true:false} onClick={oceniHandler}>Oceni</Button>
  </>)}
  <hr/>
  <h1>Ponude koja agencija nudi</h1>
  <hr/>
        {ponude.map((element) => (
            <Grid sx={4}>
            <Item>
    <Kartica ponuda={element} />
    </Item>
            </Grid>))}
            <hr/>
  <h1>Korisnici si u kontaktu sa agencijom</h1>
  <hr/>
            {korisnici.map((element) => (
            <Grid sx={4}>
            <Item>
    <KarticaKorisnika korisnik={element} />
    </Item>
            </Grid>))}
     
        </CardContent>
        </Card>)}
        </>
    );
}
export default DetaljnaAgencija;