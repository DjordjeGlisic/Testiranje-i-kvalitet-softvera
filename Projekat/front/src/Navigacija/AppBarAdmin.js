import { AppBar, Avatar, Box, Button, CssBaseline, IconButton, Toolbar, Typography,Drawer } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import { useNavigate } from "react-router-dom";
import axios from "axios";
const AppBarAdmin=()=>
{
    const navigate=useNavigate();
    const email=localStorage.getItem('email');
    const id=localStorage.getItem('id');
    
    const odjavaHandler=(event)=>
    {
        event.preventDefault();
        localStorage.removeItem('email');
        localStorage.removeItem('id');
        localStorage.removeItem('idKorisnika');
        localStorage.removeItem('idAgencije');
     
        navigate('/Prijava');
    }
    const dodajHandler=(event)=>
    {
      event.preventDefault();
      navigate('/DodajAgenciju');
    }
    const pocetnaHandler=(event)=>
    {
      event.preventDefault();
      navigate('/SveAgencije');
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
       
       
         
           
            {email!=null&&(<>
            <Avatar sx={{ color: '#fff',marginTop:'0px',marginLeft:'-40px'}} >{email[0]+email[1]}</Avatar>
            <Button sx={{color:'white',marginTop:'-70px'}} onClick={odjavaHandler}> Odjavi se</Button>
            <Button sx={{color:'white',marginTop:'-70px'}} onClick={dodajHandler}> DodajAgenciju</Button>
            <Button sx={{color:'white',marginTop:'-70px'}} onClick={pocetnaHandler}>Pocetna</Button>
            
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
export default AppBarAdmin;