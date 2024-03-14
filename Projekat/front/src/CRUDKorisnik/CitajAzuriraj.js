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
                          <h1>
                                {korisnik.ime}
                            </h1>
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Novo ime"
                            type="text"
                            id="minut"
                            defaultValue={korisnik.ime}
                            autoComplete="minut"
                            onChange={imeHandler}
                        />
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                        <h1>{korisnik.prezime}</h1>
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Novo prezime"
                            type="text"
                            id="minut"
                            defaultValue={korisnik.prezime}
                            autoComplete="minut"
                            onChange={prezimeHandler}
                        />
              
                        </Item>
                    </Grid>
                    <Grid xs={6}>
                        <Item>
                        <h1>{korisnik.email}</h1>
                    
                    </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                      
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Novo prezime"
                            type="email"
                            id="minut"
                            defaultValue={korisnik.email}
                            autoComplete="minut"
                            onChange={mejlHandler}
                        />
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1>
                                {"Unesite novu sifru"}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Unesite novu sifru"
                            type="password"
                            id="minut"
                            autoComplete="minut"
                            onChange={sifraHandler}
                            
                        />
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1>
                                {korisnik.telefon}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Unesite nov broj telefona"
                            type="number"
                            id="minut"
                            autoComplete="minut"
                            onChange={telefonHandler}
                            
                        />
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1>
                                {korisnik.datumRodjenja}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Unesite nov datum rodjenja"
                            type="text"
                            id="minut"
                            autoComplete="minut"
                            onChange={datumHandler}
                            
                        />
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1>
                                {korisnik.grad}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Unesite nov grad"
                            type="text"
                            id="minut"
                            autoComplete="minut"
                            onChange={gradHandler}
                            
                        />
              
                        </Item>
                    </Grid>
                    <Grid item xs={6}>
                        <Item>
                          <h1>
                                {korisnik.adresa}
                            </h1>
                        </Item>
                    </Grid>
                 
                          <Grid item xs={6}>
                        <Item>
                       
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="minut"
                            label="Unesite novu adresu"
                            type="text"
                            id="minut"
                            autoComplete="minut"
                            onChange={adresaHandler}
                            
                        />
              
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