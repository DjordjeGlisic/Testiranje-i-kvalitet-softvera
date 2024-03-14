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
import ShareIcon from '@mui/icons-material/Share';
import DeleteIcon from '@mui/icons-material/Delete';
import CancelIcon from '@mui/icons-material/Cancel';
import Slika from '../Images/vacation.jpg';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import EditIcon from '@mui/icons-material/Edit';
export default function Kartica(props) {
 const navigate=useNavigate();
 const email=localStorage.getItem('email');
const btnHandler=(event)=>
{
    event.preventDefault();
    localStorage.removeItem('idPonude');
    localStorage.setItem('idPonude',props.ponuda.id);
    navigate('/DetaljnaPonuda');
}
const [korisnici,setKorisnici]=React.useState([]);
const [rezervisana,setRezervisana]=React.useState(false);
const idKorisnika=localStorage.getItem('id');
React.useEffect(() => {
      
       
  console.log('Uso sam u fju');
  const response =  axios.get(`https://localhost:7199/Ponuda/PribaviSveKorisnikePonude/${props.ponuda.id}`,
  {
    headers:{
      //Authorization: `Bearer ${token}`
    }
  }).then(response=>{
    setKorisnici(response.data);
    response.data.forEach(element => {
      if(element.id===idKorisnika)
      {
        setRezervisana(true);
        console.log('rezervisana'+rezervisana);
        
      }
    });
    console.log(response.data);
  })
  .catch(error=>{
    console.log(error);
   
  })
  

}, []);
const rezervacijaHandler=(event,id)=>
{
  event.preventDefault();
  const response =  axios.post(`https://localhost:7199/Korisnik/RezervisiPonudu/${idKorisnika}/${id}`,
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
const otkazHandler=(event,id)=>
{
  event.preventDefault();
  const response =  axios.post(`https://localhost:7199/Korisnik/OtkaziRezervaciju/${idKorisnika}/${id}`,
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
const azurirajHandler=(event)=>
{
  event.preventDefault();
  localStorage.removeItem('idPonude');
  localStorage.setItem('idPonude',props.ponuda.id);
  navigate('/CitajAzurirajPonudu');
}

const brisiHandler=(event)=>
{
  event.preventDefault();
  const response =  axios.delete(`https://localhost:7199/Ponuda/ObrisiPonudu/${props.ponuda.id}`,
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
    <Card sx={{ Width: 345,bgcolor:'#c5c9c6' }} data-testid="add-stock-container">
      <CardHeader
       
      
        title={props.ponuda.nazivPonude}
        subheader={props.ponuda.datumPolaska+"-"+props.ponuda.datumDolaska}
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
    Gradovi koji se posecuju
  </Typography> 
        {props.ponuda.listaGradova.map((element) => (
    <Typography variant="h5" gutterBottom>
    {element}
  </Typography> 
    ))}
        </Typography>
      </CardContent>
      <CardContent>
      
        <Typography variant="body2" color="text.Secondary">
        {props.ponuda.opisPutovanja}
        </Typography>
      </CardContent>
      {email.startsWith('korisnik')===true&&(<CardActions disableSpacing>
        <IconButton aria-label="add to favorites" onClick={(event) => btnHandler(event)}> 
          <MoreIcon />
          <p>Detalji</p>
        </IconButton>

        {(rezervisana===false&&email.startsWith('korisnik')===true)&&(<IconButton aria-label="share" sx={{marginLeft:'100px'}} disabled={email===null?true:false} onClick={(event) => rezervacijaHandler(event, props.ponuda.id)}>
          <VisibilityIcon/>
          <p>Rezervisi</p>
        </IconButton>)}
        {(rezervisana===true&&email.startsWith('korisnik')===true)&&(<IconButton aria-label="share" sx={{marginLeft:'100px'}} disabled={email===null?true:false} onClick={(event) => otkazHandler(event, props.ponuda.id)}>
          <CancelIcon/>
          <p>Otkazi rezervaciju</p>
        </IconButton>)}
      </CardActions>)}
      {email.startsWith('agencija')===true&&(
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
  );
}
