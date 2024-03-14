import { Button, Paper, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

const OkStranica=()=>
{
    const email=localStorage.getItem('email');
    const navigate=useNavigate();
    const okHandler=(event)=>{
        event.preventDefault();
        console.log(email);
        if(email.startsWith('korisnik')==true)
        {

            navigate('/');
        }
        else if(email.startsWith('agencija')===true)
        {
            console.log('agencija');
            navigate('/DetaljnaAgencija')
        }
        else if(email.startsWith('admin')===true)
        {
            navigate('/SveAgencije');
        }
    }
    return(
        <>
            <Paper elevation={24}>
                <Typography>
                    <h1>
                        Uspesno obavljena funkcija
                    </h1>
                </Typography>
                <Button onClick={okHandler}>
                    OK
                </Button>
            </Paper>
        
        </>
    );
}
export default OkStranica;