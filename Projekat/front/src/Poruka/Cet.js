import { Alert, Avatar, Button, Grid, Paper, TextField } from "@mui/material";
import React, { useEffect, useState } from "react";
import Item from '@mui/material/ListItem'

import Poruka from "./Poruka";
import axios from "axios";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
const Cet=()=>
{
    const idAgencije=localStorage.getItem('idAgencije');
    const idKorisnika= localStorage.getItem('idKorisnika');
    const [message,setMessage]=useState([]);
    const[komentar,setKomentar]=useState('');
    const [value, setValue] = useState(0);
    const [conn,setConn]=React.useState();
    const[baza,setBaza]=useState(1);
    const[ponovo,setPonovo]=useState(false);
    const email=localStorage.getItem('email');
    useEffect(() => {
     
      const response =  axios.get(`https://localhost:7199/Poruka/PribaviCet/${idKorisnika}/${idAgencije}`,
      {
        headers:{
          //Authorization: `Bearer ${token}`
        }
      }).then(response=>{
        setMessage(response.data);
        setValue(1);
       
        console.log(response.data);
      })
      .catch(error=>{
        console.log(error);
      });
    
    
      
    }, [ponovo]);
    const komentarHandler=(event)=>{
     
        const novoKomentar = event.target.value;
    
        // Ažuriramo stanje sa novom vrednošću
        setKomentar(novoKomentar); 
    }
    const saljiHandler=(event)=>{
        event.preventDefault();
        const currentDate = new Date();
        const dateString = currentDate.toLocaleDateString();
        const timeString = currentDate.toLocaleTimeString();
        const dateTimeString = `${dateString} ${timeString}`;
        let procitanaK=email.startsWith('korisnik')?true:false;
        const response = axios.post(
          `https://localhost:7199/Poruka/PosaljiPoruku`,
          {
            id: "string",
            idKorisnika: idKorisnika,
            idAgencije: idAgencije,
            datum: dateTimeString,
            sadrzaj: komentar,
            poslataOdStraneAgencije: !procitanaK,
            poslataOdStraneKorisnika: procitanaK
                    },
          {
            headers: {
              // Ovde možete dodati header informacije ako su potrebne
              // Authorization: `Bearer ${token}`
            },
          }
        )
          .then((response) => {
            // Obrada uspešnog odgovora
           if(ponovo===false)
           {
            setPonovo(true);
           }
           else
           {
            setPonovo(false);
           }
            
          
        })
          .catch((error) => {
            // Obrada greške
            console.log(error);
            
          });

    }
   
        const fjaPonovo=(value)=>
        {
          if(ponovo===true)
          {
            setPonovo(false);
          }
          else
          {
            setPonovo(true);
          }
        }
      
   
    const kola=message.map((element,index) => (
       <>
         {value==1&&(
            
                <Grid xs={12}>
                    <Item>
                        <Poruka m={element} indeks={index} sadrzaj={komentar} ponovo={fjaPonovo}/>
                    
                    </Item>
                </Grid>
            
         )}
       
        </>
        ));

    return(
        <>
        <Grid container spacing={0}>
        <Paper elevation={10} sx={{backgroundColor:'#b8e5e6',width:'2000px'}}>
                        <h1>Poruke koje ste razmenili sa datim korisnikom aplikacije</h1>
                       
                    
            {kola}
        
           </Paper>
           <Grid xs={10}>
           <input type="text" style={{width:'1400px'}} data-testid='prezime' name="firstname"  placeholder="Napisite komentar" onChange={komentarHandler}/>
           
           </Grid>
           <Grid xs={2}>
           <Button variant="contained" sx={{width:'200px',marginBottom:'0px',height:'60px'}} onClick={saljiHandler} >Posalji</Button>
           </Grid>
        </Grid>
        </>
    );

}
export default Cet;