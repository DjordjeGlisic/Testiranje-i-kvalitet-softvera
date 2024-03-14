import { AppBar, Avatar, Box, Button, CssBaseline, IconButton, Toolbar, Typography,Drawer } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import { useNavigate } from "react-router-dom";
import axios from "axios";
const AppBarKorisnik=()=>
{
    const navigate=useNavigate();
    const email=localStorage.getItem('email');
    const id=localStorage.getItem('id');
    const prijavaHandler=(event)=>
    {
        event.preventDefault();
        navigate('/Prijava');
    }
    const odjavaHandler=(event)=>
    {
        event.preventDefault();
        localStorage.removeItem('email');
        localStorage.removeItem('id');
        localStorage.removeItem('idKorisnika');
        localStorage.removeItem('idAgencije');
     
        navigate('/Prijava');
    }
    const azuriranjeHandler=(event)=>
    {
      event.preventDefault();
      navigate('/CitajAzurirajKorisnika');
    }
    const obrisiHandler=(event)=>
    {
      event.preventDefault();
      const response =  axios.delete(`https://localhost:7199/Korisnik/DeleteUser/${id}`,
      
      {
        headers:{
          //Authorization: `Bearer ${token}`
        }
      }).then(response=>{
        
        
        localStorage.removeItem('email');
        localStorage.removeItem('id');
        localStorage.removeItem('idKorisnika');
        localStorage.removeItem('idAgencije');
        navigate('/Prijava');
        console.log(response.data);
      })
      .catch(error=>{
        console.log(error);
       
      })
    }
    return(
        <>
           <Box sx={{ display: 'flex' }}>
    <CssBaseline />
    <AppBar component="nav">
      <Toolbar>
        <IconButton
          color="inherit"
          aria-label="open drawer"
          edge="start"
          
          sx={{ mr: 2, display: { sm: 'none' } }}
        >
          <MenuIcon />
        </IconButton>
        <Typography
          variant="h3"
          component="div"
          sx={{ flexGrow: 1, display: { xs: 'none', sm: 'block' },marginLeft:'120px' }}
        >
          KRUZERI
         
        </Typography>
        
        <Box sx={{ display: { xs: 'none', sm: 'block' } }}>
       
       
         
            {email===null&&(<Button key="Prijava" sx={{ color: '#fff',marginBottom:'10px' }} onClick={prijavaHandler}>
            <Typography
          variant="h5"
          component="div"
          sx={{ flexGrow: 1, display: { xs: 'none', sm: 'block' },marginLeft:'120px' }}
        >
          PRIJAVI SE
         
        </Typography>
            </Button>)}
            {email!=null&&(<>
            <Avatar sx={{ color: '#fff',marginTop:'0px',marginLeft:'-40px'}} >{email[0]+email[1]}</Avatar>
            <Button sx={{color:'white',marginTop:'-70px'}} onClick={odjavaHandler}> Odjavi se</Button>
            <Button sx={{color:'white',marginTop:'-70px'}} onClick={azuriranjeHandler}> Azuriraj informacije</Button>
            <Button sx={{color:'white',marginTop:'-70px'}} onClick={obrisiHandler}> Obrisi nalog</Button>
            
            </>
            )}
        </Box>
      </Toolbar>
    </AppBar>
    <nav>
      <Drawer
        
        variant="temporary"
        ModalProps={{
          keepMounted: true, // Better open performance on mobile.
        }}
        sx={{
          display: { xs: 'block', sm: 'none' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: "100px" },
        }}
      >
       
      </Drawer>
    </nav>
    <Box component="main" sx={{ p: 3 }}>
      <Toolbar />
    
    </Box>
  </Box>
        </>
    );
}
export default AppBarKorisnik;