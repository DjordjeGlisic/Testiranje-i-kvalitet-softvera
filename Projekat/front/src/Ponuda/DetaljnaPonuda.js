import { Button, Card, CardContent, Grid, Paper, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import Item from '@mui/material/ListItem'
import AppBarKorisnik from "../Navigacija/AppBarKorisnik";
import axios from "axios";
const DetaljnaPonuda=()=>
{
    const[value,setValue]=useState(0);
    const[ponuda,setPonuda]=useState({});
    const idPonude=localStorage.getItem('idPonude');
    //localStorage.removeItem('idPonude');
    useEffect(() => {
 
        console.log('Uso sam u fjuSobe');
      const response =  axios.get(`https://localhost:7199/Ponuda/PribaviPonuduPoId/${idPonude}`,
      {
        headers:{
          //Authorization: `Bearer ${token}`
        }
      }).then(response=>{
        setPonuda(response.data);
        setValue(1);
       
        console.log(response.data);
      })
      .catch(error=>{
        console.log(error);
        setValue(0);
      })
    
      },[]);
    return(
        <>
       <AppBarKorisnik/>
        {value===1&&(<Card sx={{ marginLeft:'380px',marginTop:'10px',width:'740px' ,height:'1800px',backgroundColor:'#c5c9c6',alignItems:'center',justifyContent:'center',alignContent:'center'}}>
      <CardContent sx={{marginTop:'-70px',padding:'100px'}}>
      <Typography sx={{ fontSize: 48,padding:'30px' }} color="text.primary" gutterBottom>
        Naziv ponude 
        </Typography>
        <Typography sx={{ fontSize: 48,padding:'30px' }} color="#591905" gutterBottom>
        {ponuda.nazivPonude}
        </Typography>
        <hr/>
        <Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
          Grad polaska broda:</Typography>
          <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {ponuda.gradPolaskaBroda}</Typography> 
          <br/> 
          <Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
          Datum pocetka putovanja:</Typography>
          <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {ponuda.datumPolaska}</Typography> 
          <br/> 
          <Typography sx={{ fontSize: 28}} color="text.primary" gutterBottom>
          Datum kraja putovanja:</Typography>
          <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {ponuda.datumDolaska}</Typography> 
          <br/> 
        
        
        <hr/>
        <Typography variant="h3" component="div">
        <Typography component="legend" sx={{ fontSize: 28}}>Aerodrom sa koga polece avion</Typography>
        <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {ponuda.nazivAerodroma}</Typography> 
        </Typography>
        <hr/>
        <Typography sx={{ mb: 1.5 }} color="text.secondary">
          Cena smestaja na brodu bez hrane:{ponuda.cenaSmestajaBezHrane}
        </Typography>
        <hr/>
        <Typography sx={{ mb: 1.5 }} color="text.secondary">
          Cena smestaja na brodu sa hranom:{ponuda.cenaSmestajaSaHranom}
        </Typography>
        <hr/>
        <h1 >Lista gradova koji se nalaze na ovoj ponudi</h1>
         <Grid container spacing={4}>
        {ponuda.listaGradova.map((element) => (
        
        <Grid xs={3}>
        <Item>
              <Typography sx={{fontSize:28}} color="#591905" gutterBottom>
          {element}</Typography> 
        
              </Item>
          
          </Grid>
        ))}
        </Grid> 
        <Grid xs={2}>
            <Item>
                <h1>Opis putovanja</h1>
            </Item>
        </Grid>
        <Grid xs={10}>
            <Item>
            <Typography sx={{fontSize:14}} color="#591905" gutterBottom>
          {ponuda.opisPutovanja}</Typography> 
            </Item>
        </Grid>

         
     
        </CardContent>
        </Card>)}
        </>
    );
}
export default DetaljnaPonuda;