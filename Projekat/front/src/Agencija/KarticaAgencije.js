import * as React from 'react';
import { styled } from '@mui/material/styles';
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardMedia from '@mui/material/CardMedia';
import CardContent from '@mui/material/CardContent';
import CardActions from '@mui/material/CardActions';
import Collapse from '@mui/material/Collapse';
import Avatar from '@mui/material/Avatar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import { red } from '@mui/material/colors';
import MoreIcon from '@mui/icons-material/More';
import ChatIcon from '@mui/icons-material/Chat';
import Slika from '../Images/agencija.jpg';
import { useNavigate } from 'react-router-dom';
import { Rating } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import axios from 'axios';
const KarticaAgencije=(props)=>
{
    const navigate=useNavigate();
    const email=localStorage.getItem('email');
    const btnHandler=(event)=>
    {
        event.preventDefault();
        localStorage.removeItem('idAgencije');
        localStorage.setItem('idAgencije',props.agencija.id);
        navigate('/DetaljnaAgencija');
    }
    const cetHandler=(event)=>
    {
      event.preventDefault();
      localStorage.removeItem('idAgencije');
        localStorage.setItem('idAgencije',props.agencija.id);
        
      navigate('/Cet');
    }
    const azurirajHandler=(event)=>
    {
      event.preventDefault();
      localStorage.removeItem('idAgencije');
      localStorage.setItem('idAgencije',props.agencija.id);
      navigate('/CitajAzurirajAgenciju');
    }
    const brisiHandler=(event)=>
    {
      event.preventDefault();
      const response =  axios.delete(`https://localhost:7199/Administrator/ObrisiAgenciju/${props.agencija.id}`,
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
    return (
        <>
       <Card sx={{ Width: 345,bgcolor:'#c7d6ed' }} data-testid="add-stock-container">
      <CardHeader
       
       avatar={
        <Avatar sx={{ bgcolor: red[500] }} aria-label="recipe">
            {props.agencija.naziv[0]}
          </Avatar>
      }
        title={props.agencija.naziv}
        subheader={"Telefon-"+props.agencija.telefon}
      />
      <CardMedia
        component="img"
        height="194"
        image={Slika}
        alt="Paella dish"
      />
       <CardContent>
        <Typography variant="body2" color="text.Primary">
        <Typography variant="h4" gutterBottom>
    {"Adresa-"+props.agencija.adresa}
  </Typography> 
    
        </Typography>
      </CardContent>
      <CardContent>
      
      <Typography component="legend">Prosecna ocena</Typography>
      <Rating name="read-only" value={props.agencija.prosecnaOcena} readOnly />
      </CardContent>
      {(email===null||email.startsWith('korisnik')==true)&&(
      <CardActions disableSpacing>
      <IconButton aria-label="add to favorites" onClick={(event) => btnHandler(event)}> 
          <MoreIcon />
          <p>Detalji</p>
        </IconButton>

        <IconButton aria-label="share" sx={{marginLeft:'100px'}}  disabled={email===null?true:false} onClick={(event) => cetHandler(event)}>
          <ChatIcon/>
          <p>Poruka</p>
        </IconButton>
      </CardActions>)}
      {email.startsWith('admin')===true&&(
        <CardActions disableSpacing>
        <IconButton aria-label="add to favorites" onClick={(event) => azurirajHandler(event)}> 
          <EditIcon />
          <p>Azuriraj</p>
        </IconButton>
        <IconButton aria-label="add to favorites" onClick={(event) => brisiHandler(event)}> 
          <DeleteIcon />
          <p>Obrisi</p>
        </IconButton>
        </CardActions>

      )}
   
    </Card>
        </>
    );

}
export default KarticaAgencije;