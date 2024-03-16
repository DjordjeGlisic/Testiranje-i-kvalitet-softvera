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
import { Collapse, FormControl, InputLabel, MenuItem, Paper, Rating, Select, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import axios from 'axios';
import { Label, Output } from '@mui/icons-material';

const CitajAzuriraj=()=>
{
    const [korisnik,setKorisnik]=useState([]);
    const id=localStorage.getItem('id');
    useEffect(() => {
 
        console.log('Uso sam u fju1');
      const response =  axios.get(`https://localhost:7199/Korisnik/ReadUser/${id}`,
      {
        headers:{
          //Authorization: `Bearer ${token}`
        }
      }).then(response=>{
        setKorisnik(response.data);
        setValue(1);
        console.log(response.data);
      })
      .catch(error=>{
        console.log(error);
        setValue(0);
      })
      
      },[])
    const [value,setValue]=useState(1);
    const [ime,setIme]=useState(korisnik.ime);
    const imeHandler=(event)=>{setIme(event.target.value)};
   
    const [prezime,setPrezime]=useState(korisnik.prezime);
    const prezimeHandler=(event)=>{setPrezime(event.target.value)};
    const [mejl,setMejl]=useState(korisnik.email);
    const mejlHandler=(event)=>{setMejl(event.target.value)};
    const [sifra,setSifra]=useState(korisnik.sifra);
    const sifraHandler=(event)=>{setSifra(event.target.value)};
    const [telefon,setTelefon]=useState(korisnik.telefon);
    const telefonHandler=(event)=>{setTelefon(event.target.value)};
    const [datum,setDatum]=useState(korisnik.datum);
    const datumHandler=(event)=>{setDatum(event.target.value)};
    const [grad,setGrad]=useState(korisnik.ime);
    const gradHandler=(event)=>{setGrad(event.target.value)};
    const [adresa,setAdresa]=useState(korisnik.ime);
    const adresaHandler=(event)=>{setAdresa(event.target.value)};
   
    const navigate=useNavigate();
   
   
       const azurirajKorisnikaHandler=(event)=>
       {
        event.preventDefault();
        const response =  axios.put(`https://localhost:7199/Korisnik/UpdateUser/${id}`,
        {
            id: id,
            ime: ime,
            prezime: prezime,
            email: mejl,
            sifra: sifra,
            telefon: telefon,
            datumRodjenja: datum,
            grad: grad,
            adresa: adresa

          
        },
        {
          headers:{
            //Authorization: `Bearer ${token}`
          }
        }).then(response=>{
          
          
          navigate('/Prijava');
          console.log(response.data);
        })
        .catch(error=>{
          console.log(error);
          setValue(0);
        })
       }
    return(
        <>
        {value===1&&(<Grid
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
               Azuriraj korisnika
              </Typography>
              
              <Box component="form" noValidate sx={{ mt: 1 }}>
              <Grid container spacing={2}>
              
                
                    
                    <Grid item xs={6}>
                        <Item>
                          <h1 data-testid='ime'>
                                {korisnik.ime}
                            </h1>
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <input type="text" data-testid='ime' name="firstname" placeholder="Unesite novo ime" onChange={imeHandler}/>
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                        <h1 data-testid='prezime'>{korisnik.prezime}</h1>
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <input type="text" data-testid='prezime' name="firstname" placeholder="Unesite novo prezime" onChange={prezimeHandler}/>
                        
              
                        </Item>
                    </Grid>
                    <Grid xs={6}>
                        <Item>
                        <h1 data-testid='email'>{korisnik.email}</h1>
                    
                    </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <input type="email" data-testid='email' name="firstname" placeholder="Unesite novi email" onChange={mejlHandler}/>
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1 data-testid='sifra'>
                                {"Unesite novu sifru"}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <input type="password" data-testid='sifra' name="firstname" placeholder="Unesite novu sifru" onChange={sifraHandler}/>
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1 data-testid='telefon'>
                                {korisnik.telefon}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <input type="number" data-testid='telefon' name="firstname" placeholder="Unesite novi telefon" onChange={telefonHandler}/>
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1 data-testid='datum'>
                                {korisnik.datumRodjenja}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <input type="text" data-testid='datum' name="firstname" placeholder="Unesite novi datum" onChange={datumHandler}/>
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1 data-testid='grad'>
                                {korisnik.grad}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <input type="text" data-testid='grad' name="firstname" placeholder="Unesite novi grad" onChange={gradHandler}/>
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1 data-testid='adresa'>
                                {korisnik.adresa}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <input type="text" data-testid='adresa' name="firstname" placeholder="Unesite novu adresu" onChange={adresaHandler}/>
              
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
                  onClick={azurirajKorisnikaHandler}
                  data-testid='dugme'
                >
                  Azuriraj korisnika
                </Button>
               
              </Box>
            </Box>
            </Paper>
          </Container>
        </Grid>)}
        </>
    );

}
export default CitajAzuriraj;