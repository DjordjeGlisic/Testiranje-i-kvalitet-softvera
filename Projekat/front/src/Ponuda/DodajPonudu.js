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
const DodajPonudu=()=>
{
   
   
   
    const [nazivPonude,setNazivPonude]=useState('');
    const nazivPonudeHandler=(event)=>{setNazivPonude(event.target.value)};
   const id=localStorage.getItem('id');
    const [gradPolaskaBroda,setGradPolskaBroda]=useState('');
    const gradPolaskaBrodaHandler=(event)=>{setGradPolskaBroda(event.target.value)};
    const [nazivAerodroma,setNazivAerodroma]=useState('');
    const nazivAerodromaHandler=(event)=>{setNazivAerodroma(event.target.value)};
    const [datumPolaska,setDatumPolaska]=useState('');
    const datumPolaskaHandler=(event)=>{setDatumPolaska(event.target.value)};
    const [datumDolaska,setDatumDolaska]=useState('');
    const datumDolaskaHandler=(event)=>{setDatumDolaska(event.target.value)};
    const [cenaSmestajaBezHrane,setCenaSmestajaBezHrane]=useState('');
    const cenaSmestajaBezHraneHandler=(event)=>{setCenaSmestajaBezHrane(event.target.value)};
    const [cenaSmestajaSaHranom,setCenaSmestajaSaHranom]=useState('');
    const cenaSmestajaSaHranomHandler=(event)=>{setCenaSmestajaSaHranom(event.target.value)};
    const [opisPutovanja,setOpisPutovanja]=useState('');
    const opisPutovanjaHandler=(event)=>{setOpisPutovanja(event.target.value)};
    const[listaGradova,setListaGradova]=useState([]);
    const[unos,setUnos]=useState('');
 const gradoviPressHandler = (event) => {
   event.preventDefault();
   
   
   setListaGradova((prevGradovi) => [...prevGradovi, unos]);
   
 }
 const [tip, setTip] = useState();
     
 const handleChangeTip = (event) => {
  // setTip(event.target.value);
   console.log(event.target.value);
   setUnos(event.target.value);
   
 };
 const  btnHandler=(event, element)=>
 {
   event.preventDefault();
  
  setListaGradova(listaGradova.filter(e=>e!==element));
 }
 const ListItem = styled('li')(({ theme }) => ({
   
 }));
    const navigate=useNavigate();
   
   
       const dodajPonuduHandler=(event)=>
       {
        event.preventDefault();
        const response =  axios.post(`https://localhost:7199/Ponuda/DodajPonudu/${id}`,
        {
            id: '',
            nazivPonude: nazivPonude,
            gradPolaskaBroda: gradPolaskaBroda,
            nazivAerodroma: nazivAerodroma,
            datumPolaska: datumPolaska,
            datumDolaska: datumDolaska,
            cenaSmestajaBezHrane: cenaSmestajaBezHrane,
            cenaSmestajaSaHranom: cenaSmestajaSaHranom,
            listaGradova: listaGradova,
            opisPutovanja: opisPutovanja
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
               Dodaj novu ponudu
              </Typography>
              
              <Box component="form" noValidate sx={{ mt: 1 }}>
              <Grid container spacing={2}>
              
                
                    
                 
                    <Grid item xs={12}>
                        <Item>
                      
                        <input type="text" data-testid='naziv' name="firstname" placeholder="Unesite naziv ponude" onChange={nazivPonudeHandler}/>
              
                        </Item>
                    </Grid>
                    
                    <Grid item xs={12}>
                        <Item>
                      
                        <input type="text" data-testid='aerodrom' name="firstname" placeholder="Unesite naziv aerodroma" onChange={nazivAerodromaHandler}/>
                        </Item>
                    </Grid>
                    
                    <Grid item xs={12}>
                        <Item>
                      
                        <input type="text" data-testid='grad' name="firstname" placeholder="Unesite grad iz koga poalzi brod" onChange={gradPolaskaBrodaHandler}/>
                        </Item>
                    </Grid>
                   
                 
                          <Grid item xs={12}>
                        <Item>
                       
                        <input type="text" data-testid='polazak' name="firstname" placeholder="Unesite datum pocetka putovanja" onChange={datumPolaskaHandler}/>
              
                        </Item>
                    </Grid>
                  
                 
                          <Grid item xs={12}>
                        <Item>
                       
                        <input type="text" data-testid='dolazak' name="firstname" placeholder="Unesite datum kraja putovanja" onChange={datumDolaskaHandler}/>
              
                        </Item>
                    </Grid>
                    
                 
                          <Grid item xs={12}>
                        <Item>
                       
                        <input type="number" data-testid='ne-hrana' name="firstname" placeholder="Unesite cenu smestaja bez hrane u evrima" onChange={cenaSmestajaBezHraneHandler}/>
              
                        </Item>
                    </Grid>
                 
                 
                          <Grid item xs={12}>
                        <Item>
                       
                        <input type="number" data-testid='hrana' name="firstname" placeholder="Unesite cenu smestaja sa hranom u evrima" onChange={cenaSmestajaSaHranomHandler}/>
              
                        </Item>
                    </Grid>
                 
                 
                          <Grid item xs={12}>
                        <Item>
                       
                        <input type="text" data-testid='opis' name="firstname" placeholder="Unesite kratak opis putovanja sa najmanje 10 slova" onChange={opisPutovanjaHandler}/>
              
                        </Item>
                    </Grid>
                   
                 



                  <Grid xs={12}>
            <Item>
              <h1>Unesite  destinacije </h1>
            </Item>
          </Grid>
        <Grid xs={10}>
              <Item>
                
              <FormControl variant="filled" sx={{width:'400px'}} >
                     
              <input type="text" data-testid='gradovi' name="firstname" placeholder="Unesite grad koji se posecuje" onChange={handleChangeTip}/>
                  </FormControl>
                  </Item>
          
          </Grid>
          <Grid xs={2}>
            <Item>
              <Button variant='contained' data-testid='potvrda' onClick={gradoviPressHandler}>Unesi grad</Button>
            </Item>
          </Grid>
        <Grid xs={12}>
          <Item>
                {listaGradova.map((data,ind) => {
        
        return (
          <li key={ind}>
            <Chip
              data-testid={'grad'+ind}
              label={data}
              onDelete={(event) => btnHandler(event,data)}
            />
          </li>
        );
      })}
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
                  onClick={dodajPonuduHandler}
                >
                  Dodaj ponudu
                </Button>
               
              </Box>
            </Box>
            </Paper>
          </Container>
        </Grid>
        </>
    );

}
export default DodajPonudu;