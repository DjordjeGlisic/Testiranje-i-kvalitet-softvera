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
const DodajAgenciju=()=>
{
   
   
   
    const [naziv,setNaziv]=useState('');
    const nazivHandler=(event)=>{setNaziv(event.target.value)};
   const id=localStorage.getItem('id');
    const [adresa,setAdresa]=useState('');
    const adresaHandler=(event)=>{setAdresa(event.target.value)};
    const [telefon,setTelefon]=useState('');
    const telefonHandler=(event)=>{setTelefon(event.target.value)};
    const [mejl,setMejl]=useState('');
    const mejlHandler=(event)=>{setMejl(event.target.value)};
    const [sifra,setSifra]=useState('');
    const sifraHandler=(event)=>{setSifra(event.target.value)};
    const navigate=useNavigate();
   
   
       const dodajAgencijuHandler=(event)=>
       {
        event.preventDefault();
        const response =  axios.post(`https://localhost:7199/Administrator/DodajAgenciju`,
        {
            id: "string",
            naziv: naziv,
            adresa: adresa,
            telefon: telefon,
            email: mejl,
            sifra: sifra,
            prosecnaOcena: 0,
            brojKorisnikaKojiSuOcenili: 0
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
               
              <Typography component="h1" variant="h3">
               Dodaj novu agenciju
              </Typography>
              
              <Box component="form" noValidate sx={{ mt: 1 }}>
              <Grid container spacing={2}>
              
                
                    
                 
                    <Grid item xs={12}>
                        <Item>
                      
                        <input type="text" data-testid='naziv' name="firstname" placeholder="Unesite naziv agencije" onChange={nazivHandler}/>
              
                        </Item>
                    </Grid>
                    
                    <Grid item xs={12}>
                        <Item>
                      
                        <input type="text" data-testid='adresa' name="firstname" placeholder="Unesite  adresu agencije" onChange={adresaHandler}/>
              
                        </Item>
                    </Grid>
                    
                    <Grid item xs={12}>
                        <Item>
                        <input type="number" data-testid='telefon' name="firstname" placeholder="Unesite telefon agencije" onChange={telefonHandler}/>
              
                        </Item>
                    </Grid>
                   
                 
                          <Grid item xs={12}>
                        <Item>
                       
                        <input type="email" data-testid='email' name="firstname" placeholder="Unesite email agencije" onChange={mejlHandler}/>
              
                        </Item>
                    </Grid>
                  
                 
                          <Grid item xs={12}>
                        <Item>
                       
                        <input type="password" data-testid='sifra' name="firstname" placeholder="Unesite sifru" onChange={sifraHandler}/>
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
                  onClick={dodajAgencijuHandler}
                  data-testid='dodaj'
                >
                  Dodaj agenciju
                </Button>
               
              </Box>
            </Box>
            </Paper>
          </Container>
        </Grid>
        </>
    );

}
export default DodajAgenciju;