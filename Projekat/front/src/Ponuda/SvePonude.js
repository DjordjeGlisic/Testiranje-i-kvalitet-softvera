
import { Grid } from "@mui/material";
import Kartica from "./Kartica";
import Item from '@mui/material/ListItem'
import { useEffect, useState } from "react";
import axios from "axios";
import KarticaAgencije from "../Agencija/KarticaAgencije";
import AppBarKorisnik from "../Navigacija/AppBarKorisnik";
import AppBarAgencija from "../Navigacija/AppBarAgencija";
const SvePonude=()=>
{
    const [ponude,setPonude]=useState([]);
    const[value,setValue]=useState(0);
    const[agencije,setAgencije]=useState([]);
    const email=localStorage.getItem('email');
    const [val,setVal]=useState(0);
    useEffect(() => {
      
       
          console.log('Uso sam u fju');
          const response =  axios.get(`https://localhost:7199/Ponuda/PribaviSvePonude`,
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
          const response2=  axios.get(`https://localhost:7199/Administrator/PribaviAgencije`,
          {
            headers:{
              //Authorization: `Bearer ${token}`
            }
          }).then(response=>{
            setAgencije(response.data);
            setVal(1);
            console.log(response.data);
          })
          .catch(error=>{
            console.log(error);
            setVal(0);
          })
        
      
        
      }, []);


    return (
        <>
       {(email.startsWith('korisnik')==true||email===null)&&( <AppBarKorisnik/>)}
       {email.startsWith('agencija')===true&&(<AppBarAgencija/>)}
        <h1 style={{color:'#bccccf'}}>Ponude koje nude agencije na platformi "Kruzeri"</h1>
        <hr/>
        {value===1&&(
        <Grid container spacing={0} sx={{backgroundColor:'#bccccf'}}>
        {ponude.map((element) => (
            <Grid sx={4}>
            <Item>
    <Kartica ponuda={element} />
    </Item>
            </Grid>
    ))}
            

             
        </Grid>
        
        )}
   <h1 style={{color:'#bccccf'}}>Agencije koje svoje ponude stavljaju na platformi "Kruzeri"</h1>
         <hr/>
         {val===1&&(
        <Grid container spacing={0} sx={{backgroundColor:'#bccccf'}}>
        {agencije.map((element) => (
            <Grid sx={4}>
            <Item>
    <KarticaAgencije agencija={element} />
    </Item>
            </Grid>
    ))}
    </Grid>)}
        </>
    );
}
export default SvePonude;