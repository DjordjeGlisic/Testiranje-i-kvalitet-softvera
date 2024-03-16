import { Navigate, useNavigate } from 'react-router-dom';
import React, { useState } from 'react';
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
import { Paper } from '@mui/material';
import axios from 'axios';
import '../App.css';
function Registracija()
{
    const navigate = useNavigate();
    const [mejl, setMejl] = useState('');
    const [sifra,setSifra]=useState('');
    const[ime,setIme]=useState('');
    const[prezime,setPrezime]=useState('');
    const [telefon,setTelefon]=useState(0);
    const[datum,setDatum]=useState('');
    const[grad,setGrad]=useState('');
    const[adresa,setAdresa]=useState('');
    const registracijaHandler=(event)=>
    {
        event.preventDefault();
        const response = axios.post(
          `https://localhost:7199/Korisnik/RegisterUser`,
          {
            id: "string",
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
            headers: {
              // Ovde možete dodati header informacije ako su potrebne
              // Authorization: `Bearer ${token}`
            },
          }
        )
          .then((response) => {
            // Obrada uspešnog odgovora
            
            
            navigate('/Prijava');
          })
          .catch((error) => {
            // Obrada greške
            console.log(error);
            
          });
        
        


    }
    const linkHandler=()=>
    {
        navigate('/Prijava');
    }
    const imeHandler=(event)=>{
     
        const novoIme = event.target.value;
  
        // Ažuriramo stanje sa novom vrednošću
        setIme(novoIme); 
    }
    const prezimeHandler=(event)=>{
     
      const novoPrezime = event.target.value;

      // Ažuriramo stanje sa novom vrednošću
      setPrezime(novoPrezime); 
  }
  const emailHandler=(event)=>{
     
    const novoEmail = event.target.value;

    // Ažuriramo stanje sa novom vrednošću
    setMejl(novoEmail); 
}
const passwordHandler=(event)=>{
     
  const novoPassword = event.target.value;

  // Ažuriramo stanje sa novom vrednošću
  setSifra(novoPassword); 
}
const telefonHandler=(event)=>{
     
  
  // Ažuriramo stanje sa novom vrednošću
  setTelefon(event.target.value); 
}
const datumHandler=(event)=>
{
    setDatum(event.target.value);
}
const adresaHandler=(event)=>
{
    setAdresa(event.target.value);
}
const gradHandler=(event)=>
{
    setGrad(event.target.value);
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
        <Paper elevation={24}>
        <Box
          sx={{  
            marginTop: 8,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <Typography component="h1" variant="h3" data-testid='registracija'>
           Registracija
          </Typography>
         
          <Box component="form" noValidate sx={{ mt: 1 }}>
            
          <Grid container spacing={2}>
            <Grid item xs={6}>
                <Item>
                 
                  <input type="text" data-testid='ime' name="firstname" placeholder="Unesite ime" onChange={imeHandler}/>
                
                </Item>
            </Grid>
                <Grid item xs={6}>
                    <Item>
                    <input type="text" data-testid='prezime' name="firstname" placeholder="Unesite prezime" onChange={prezimeHandler}/>
                    </Item>
                </Grid>
                <Grid item xs={12}>
                    <Item>
                    <input type="email" data-testid='email' name="firstname" placeholder="Unesite email" onChange={emailHandler}/>
          
                    </Item>
                </Grid>
                <Grid item xs={12}>
                    <Item>
                    <input type="password" data-testid='sifra' name="firstname" placeholder="Unesite sifru" onChange={passwordHandler}/>
          
                    </Item>
                </Grid>
                <Grid item xs={12}>
                    <Item>
                    <input type="number" data-testid='telefon' name="firstname" placeholder="Unesite broj telefona" onChange={telefonHandler}/>
                    </Item>
                </Grid>
                <Grid item xs={12}>
                    <Item>
                    <input type="text" data-testid='datum' name="firstname" placeholder="Unesite datum rodjenja" onChange={datumHandler}/>
          
                    </Item>
                </Grid>
                <Grid item xs={12}>
                    <Item>
                    <input type="text" data-testid='grad' name="firstname" placeholder="Unesite grad u kome zivite" onChange={gradHandler}/>
          
                    </Item>
                </Grid>
                <Grid item xs={12}>
                    <Item>
                    <input type="text" data-testid='adresa' name="firstname" placeholder="Unesite adresu" onChange={adresaHandler}/>
          
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
              onClick={registracijaHandler}
            >
              Registrujte se
            </Button>
            <Grid container sx={{ mt: 5, mb: 5 }}
             justifyContent="center"
             alignItems="center">
              
                <Link href="#" variant="body2" onClick={linkHandler}>
                  {"Imate nalog? Prijavite se"}
                </Link>
            </Grid>
          </Box>
        </Box>
        </Paper>
      </Container>
    </Grid>
    </>
    
    );
}
export default Registracija;