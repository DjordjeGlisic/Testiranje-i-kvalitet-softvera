import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Stack from '@mui/material/Stack';
import { styled } from '@mui/material/styles';
import Typography from '@mui/material/Typography';
import { Button, Grid, TextField } from '@mui/material';
import { TextFieldsRounded } from '@mui/icons-material';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: 'center',
    color: theme.palette.text.secondary,
    maxWidth: 400,
  }));
  
const Poruka=(props)=>{
  const navigate=useNavigate();
  const id=localStorage.getItem('id');
  const email=localStorage.getItem('email');
  const idKorisnika=localStorage.getItem('idKorisnika');
  const idAgencije=localStorage.getItem('idAgencije');
  
  const obrisiHandler=(event)=>
  {
    event.preventDefault();
    const response = axios.delete(
      `https://localhost:7199/Poruka/ObrisiPoruku/${props.m.id}`,
      {
      },
      {
        headers: {
          // Ovde možete dodati header informacije ako su potrebne
          // Authorization: `Bearer ${token}`
        },
      }
    )
      .then((response) => {
        props.ponovo(1);
        
      
    })
      .catch((error) => {
        // Obrada greške
        console.log(error);
        
      });
  }
  const azurirajHandler=(event)=>
  {
    event.preventDefault();
    const currentDate = new Date();
    const dateString = currentDate.toLocaleDateString();
    const timeString = currentDate.toLocaleTimeString();
    const dateTimeString = `${dateString} ${timeString}`;
   
    const response = axios.put(
      `https://localhost:7199/Poruka/AzurirajPoruku/${props.m.id}`,
      {
        id: props.m.id,
      idKorisnika: props.m.idKorisnika,
      idAgencije: props.m.idAgencije,
      datum: dateTimeString,
      sadrzaj: props.sadrzaj,
      poslataOdStraneAgencije: props.m.poslataOdStraneAgencije,
      poslataOdStraneKorisnika: props.m.poslataOdStraneKorisnika
      },
      {
        headers: {
          // Ovde možete dodati header informacije ako su potrebne
          // Authorization: `Bearer ${token}`
        },
      }
    )
      .then((response) => {
      props.ponovo(1);
        
      
    })
      .catch((error) => {
        // Obrada greške
        console.log(error);
        
      });
  }
   
    return(
      <>
      {(props.m.poslataOdStraneKorisnika===true&&id===idKorisnika)&&(<>
      <Grid xs={6}>
       
        <Item sx={{backgroundColor:'green'}}>
           <Avatar>{"K"}</Avatar>
            <Paper elevation={24} data-testid={props.m.sadrzaj}>
              {props.m.sadrzaj}
            </Paper>

        </Item>
       
    </Grid>
    <Grid xs={3}>
          <Button variant='contained' data-testid={'Azuriraj'+props.m.sadrzaj} disabled={id===props.m.idKorisnika?false:true} onClick={azurirajHandler}>Azuriraj</Button>
    </Grid>
    <Grid xs={3}>
          <Button variant='contained' data-testid={'Obrisi'+props.m.sadrzaj} disabled={id===props.m.idKorisnika?false:true} onClick={obrisiHandler}>Obrisi</Button>
    </Grid>

     </>)}
     {(props.m.poslataOdStraneAgencije===true&&id!==idAgencije)&&(<>
      <Grid xs={6} sx={{marginLeft:'1000px'}}>
       
        <Item sx={{backgroundColor:'gray'}}>
           <Avatar>{"A"}</Avatar>
            <Paper elevation={24}>
              {props.m.sadrzaj}
            </Paper>

        </Item>
       
    </Grid>
    

     </>)}



     {(props.m.poslataOdStraneAgencije===true&&id===idAgencije)&&(<>
      <Grid xs={6}>
       
        <Item sx={{backgroundColor:'green'}}>
           <Avatar>{"A"}</Avatar>
            <Paper elevation={24} data-testid={props.m.sadrzaj} >
              {props.m.sadrzaj}
            </Paper>

        </Item>
       
    </Grid>
    <Grid xs={3}>
          <Button variant='contained' onClick={azurirajHandler} data-testid={'Azuriraj'+props.m.sadrzaj}>Azuriraj</Button>
    </Grid>
    <Grid xs={3}>
          <Button variant='contained'  onClick={obrisiHandler} data-testid={'Obrisi'+props.m.sadrzaj}>Obrisi</Button>
    </Grid>

     </>)}
     {(props.m.poslataOdStraneKorisnika===true&&id!==idKorisnika)&&(<>
      <Grid xs={6} sx={{marginLeft:'1000px'}}>
       
        <Item sx={{backgroundColor:'gray'}}>
           <Avatar>{"K"}</Avatar>
            <Paper elevation={24}>
              {props.m.sadrzaj}
            </Paper>

        </Item>
       
    </Grid>
    

     </>)}
    </>
    );
}
export default Poruka;