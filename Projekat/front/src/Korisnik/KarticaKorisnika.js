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
import Slika from '../Images/user.png';
import { useNavigate } from 'react-router-dom';
import { Rating } from '@mui/material';
const KarticaKorisnika=(props)=>
{
    const navigate=useNavigate();
    const email=localStorage.getItem('email');
    
    const cetHandler=(event)=>
    {
      event.preventDefault();
      localStorage.removeItem('idKorisnika');
        localStorage.setItem('idKorisnika',props.korisnik.id);
        
      navigate('/Cet');
    }
    
    return (
        <>
       <Card sx={{ Width: 345,bgcolor:'#c7d6ed' }} data-testid="add-stock-container">
      <CardHeader
       
       avatar={
        <Avatar sx={{ bgcolor: red[500] }} aria-label="recipe">
            {props.korisnik.ime[0]+props.korisnik.ime[2]+props.korisnik.ime[3]}
          </Avatar>
      }
        title={props.korisnik.ime}
        subheader={"Telefon-"+props.korisnik.telefon}
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
    {"Adresa-"+props.korisnik.adresa}
  </Typography> 
    
        </Typography>
      </CardContent>
      <CardContent>
      
    
      </CardContent>
      <CardActions disableSpacing>
        

        {email.startsWith('agencija')&&(<IconButton aria-label="share" sx={{marginLeft:'100px'}}  disabled={email===null?true:false} onClick={(event) => cetHandler(event)}>
          <ChatIcon/>
          <p>Poruka</p>
        </IconButton>)}
      </CardActions>
   
    </Card>
        </>
    );

}
export default KarticaKorisnika;