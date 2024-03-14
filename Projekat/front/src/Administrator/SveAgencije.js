
import { Grid } from "@mui/material";

import Item from '@mui/material/ListItem'
import { useEffect, useState } from "react";
import axios from "axios";
import KarticaAgencije from "../Agencija/KarticaAgencije";
import AppBarKorisnik from "../Navigacija/AppBarKorisnik";
import AppBarAgencija from "../Navigacija/AppBarAgencija";
import AppBarAdmin from "../Navigacija/AppBarAdmin";
const SveAgencije=()=>
{
   
    const[value,setValue]=useState(0);
    const[agencije,setAgencije]=useState([]);
    const email=localStorage.getItem('email');
    const [val,setVal]=useState(0);
    useEffect(() => {
      
       
          console.log('Uso sam u fju');
         
          const response=  axios.get(`https://localhost:7199/Administrator/PribaviAgencije`,
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
       <AppBarAdmin/>
       
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
export default SveAgencije;