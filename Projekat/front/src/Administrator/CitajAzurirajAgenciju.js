import { Navigate, useNavigate } from 'react-router-dom';
import React, { useEffect, useState } from 'react';
import Button from "@mui/material/Button";
import Item from '@mui/material/ListItem'
import TextField from "@mui/material/TextField";
import FormControlLabel from "@mui/material/FormControlLabel";
import Checkbox from "@mui/material/Checkbox";
import Link from "@mui/material/Link";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Container from "@mui/material/Container";
import { Chip, Collapse, FormControl, InputLabel, MenuItem, Paper, Rating, Select, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import axios from 'axios';
import { Label, Output } from '@mui/icons-material';
import styled from '@emotion/styled';
import AppBarAdmin from '../Navigacija/AppBarAdmin';
const CitajAzurirajAgenciju=()=>
{
   
    const idAgencije=localStorage.getItem('idAgencije');
    const[value,setValue]=useState(0);
    const[agencija,setAgencija]=useState({});
   
    const [naziv,setNaziv]=useState(agencija.naziv);
    const nazivHandler=(event)=>{setNaziv(event.target.value)};
   const id=localStorage.getItem('id');
    const [adresa,setAdresa]=useState(agencija.adresa);
    const adresaHandler=(event)=>{setAdresa(event.target.value)};
    const [telefon,setTelefon]=useState(agencija.telefon);
    const telefonHandler=(event)=>{setTelefon(event.target.value)};
    const [mejl,setMejl]=useState(agencija.mejl);
    const mejlHandler=(event)=>{setMejl(event.target.value)};
    const [sifra,setSifra]=useState(agencija.sifra);
    const sifraHandler=(event)=>{setSifra(event.target.value)};
    const navigate=useNavigate();

       const azurirajAgencijuHandler=(event)=>
       {
        event.preventDefault();
        const response =  axios.put(`https://localhost:7199/Administrator/AzurirajAgenciju/${idAgencije}`,
        {
            id: idAgencije,
            naziv: naziv,
            adresa: adresa,
            telefon: telefon,
            email: mejl,
            sifra: sifra
            
        },
        {
          headers:{
            //Authorization: `Bearer ${token}`
          }
        }).then(response=>{
          
          
          navigate('/OkStranica');
          console.log(response.data);
        })
        .catch(error=>{
          console.log(error);
         
        })
       }
       useEffect(() => {
      
       
        console.log('Uso sam u fju');
       
        const response=  axios.post(`https://localhost:7199/Administrator/PribaviAgencijuPoId/${idAgencije}`,
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
      
    
      
    }, []);

    return(
        <>
        <AppBarAdmin/>
       <Grid
            container
            spacing={0}
            direction="column"
            alignItems="center"
            justifyContent="center"
            style={{ minHeight: '100vh' }}
          >
            <Container component="main" maxWidth="xs" alignItems="center" justifyContent="center"  >
            <Paper elevation={24} sx={{width:'1300px',marginLeft:'-400px'}}>
            <Box
              sx={{  
                marginTop: 8,
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
              }}
            >
               
              <Typography component="h1" variant="h3" data-testid='Azuriraj-agenciju'>
               Azuriraj agenciju
              </Typography>
              
              <Box component="form" noValidate sx={{ mt: 1 }}>
              <Grid container spacing={2}>
              
                
                    <Grid xs={6}>
                        <Item>
                            <h1 data-testid='naziv'>{agencija.naziv}</h1>
                        </Item>
                    </Grid>
                 
                    <Grid item xs={6}>
                        <Item>
                      
                        <input type="text" data-testid='nov-naziv' name="firstname" placeholder="Unesite novi naziv agencije" onChange={nazivHandler}/>
              
                        </Item>
                    </Grid>
                    <Grid xs={6}>
                        <Item>
                            <h1 data-testid='adresa'>{agencija.adresa}</h1>
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <input type="text" data-testid='nova-adresa' name="firstname" placeholder="Unesite novu adresu" onChange={adresaHandler}/>
              
                        </Item>
                    </Grid>
                    <Grid xs={6}>
                        <Item>
                            <h1 data-testid='telefon'>{agencija.telefon}</h1>
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <input type="number" data-testid='nov-telefon' name="firstname" placeholder="Unesite novi broj telefona" onChange={telefonHandler}/>
              
                        </Item>
                    </Grid>
                   
                    <Grid xs={6}>
                        <Item>
                            <h1 data-testid='email'>{agencija.email}</h1>
                        </Item>
                    </Grid>
                          <Grid item xs={6}>
                        <Item>
                       
                        <input type="email" data-testid='novi-email' name="firstname" placeholder="Unesite novi email" onChange={mejlHandler}/>
              
                        </Item>
                    </Grid>
                  
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <input type="password" data-testid='nova-sifra' name="firstname" placeholder="Unesite novu sifru" onChange={sifraHandler}/>
              
                        </Item>
                    </Grid>
                    
                 
                   
                  
              </Grid>
              
                
               
               <div>
    
               </div>
                <Button
                  type="submit"
                  fullWidth
                  variant="contained"
                  sx={{ mt: 3, mb: 2 }}
                  onClick={azurirajAgencijuHandler}
                  data-testid='azuriraj'
                >
                  Potvrdi azuriranje
                </Button>
               
              </Box>
            </Box>
            </Paper>
          </Container>
        </Grid>
        </>
    );

}
export default CitajAzurirajAgenciju;