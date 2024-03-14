using System.Security.Claims;
using System.Text.RegularExpressions;
using Kruzeri.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kruzeri.Controllers;

[ApiController]
[Route("[controller]")]
public class KorisnikController : ControllerBase
{

    private readonly ILogger<KorisnikController> _logger;
    private readonly INeo4jService _neo4jService;
    private readonly IConfiguration _configuration;
    public KorisnikController(ILogger<KorisnikController> logger, INeo4jService neo4jService, IConfiguration configuration)
    {
        _logger = logger;
        _neo4jService = neo4jService;
        _configuration = configuration;
    }
    //create
    [HttpPost("RegisterUser")]
    public async Task<ActionResult<Korisnik>> RegisterUser([FromBody] Korisnik korisnik )
    {
        try
        {
            if (korisnik.Sifra.Length < 8)
                return BadRequest("Sifra mora imati bar 8 karaktera");
            if (!korisnik.Email.Contains("@gmail"))
                return BadRequest("Email mora sadrzati @gmail");
            if (korisnik.Telefon.ToString().Length < 10)
                return BadRequest("Broj telefona treba da se sastoji od 10 cifara");
            var result=await _neo4jService.RegisterAsync(korisnik);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class KorisnikData
    {
        public string Email { get; set; }
        public string Sifra { get; set; }
    }
    //read
    [HttpPost("LoginUser")]
    public async Task<ActionResult<Korisnik>> LoginUser([FromBody] KorisnikData data)
    {
        try
        {
            if (data.Sifra.Length < 8)
                return BadRequest("Sifra mora imati bar 8 karaktera");
            if (!data.Email.Contains("@gmail"))
                return BadRequest("Email mora sadrzati @gmail");

            var user = await _neo4jService.LoginAsync(data);
            if (user == null)
            {
                return BadRequest("Ne postoji korisnik u bazi!");
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("ReadUser/{KorisnikID}")]
    public async Task<ActionResult<Korisnik>> ReadUser([FromRoute] string KorisnikID)
    {
        try
        {
            if (KorisnikID.Length < 30)
                return BadRequest("ID mora biti veci od 30 karaktera");
            var user = await _neo4jService.FindUserAsync(KorisnikID);
            if (user == null)
            {
                return BadRequest("Ne postoji korisnik u bazi!");
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //update
    [HttpPut("UpdateUser/{id}")]
    public async Task<ActionResult<Korisnik>> UpdateUser([FromRoute] string id, [FromBody] Korisnik korisnik)
    {
        try
        {
            if (id.Length < 30)
                return BadRequest("ID korisnika mora biti veci od 30 karaktera");
            if (korisnik.Sifra.Length < 8)
                return BadRequest("Sifra mora imati bar 8 karaktera");
            if (!korisnik.Email.Contains("@gmail"))
                return BadRequest("Email mora sadrzati @gmail");
            if (korisnik.Telefon.ToString().Length < 10)
                return BadRequest("Broj telefona treba da se sastoji od 10 cifara");

           var user= await _neo4jService.FindUserAsync(id);
            if (user == null)
                return BadRequest("Korisnik nije pronadjen u bazi");
            var update = await _neo4jService.UpdateUserAsync(id, korisnik);
            return Ok(update);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //delete
    [HttpDelete("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] string id)
    {
        try
        {
            if (id.Length < 30)
                return BadRequest("ID mora biti veci od 30 karaktera");
            var result = await _neo4jService.FindUserAsync(id);
            if (result == null)
                return BadRequest("Ne postoji korisnik u bazi");
            await _neo4jService.DeleteUserAsync(id);
            return Ok("Uspesno obrisan korisnik.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("RezervisiPonudu/{KorisnikID}/{PonudaID}")]
    public async Task<ActionResult> RezervisiPonudu([FromRoute]string KorisnikID, [FromRoute]string PonudaID)
    {
        try
        {
            if (KorisnikID.Length < 30)
                return BadRequest("ID korisnika mora biti veci od 30 karaktera");
            if (PonudaID.Length <30)
                return BadRequest("ID ponude mora biti veci od 30 karaktera");
            if (KorisnikID == PonudaID)
                return BadRequest("ID korisnika ne moze biti jednak ID ponude");
            await _neo4jService.RezervacijaAsync(KorisnikID, PonudaID);

            return Ok("Uspesno rezervisana ponuda");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("OtkaziRezervaciju/{KorisnikID}/{PonudaID}")]
    public async Task<ActionResult> OtkaziRezervaciu([FromRoute] string KorisnikID, [FromRoute] string PonudaID)
    {
        try
        {
            if (KorisnikID.Length < 30)
                return BadRequest("ID korisnika mora biti veci od 30 karaktera");
            if (PonudaID.Length < 30)
                return BadRequest("ID ponude mora biti veci od 30 karaktera");
            if (KorisnikID == PonudaID)
                return BadRequest("ID korisnika ne moze biti jednak ID ponude");
            await _neo4jService.OtkazRezervacijaAsync(KorisnikID, PonudaID);

            return Ok("Uspesno otkazana rezervacija");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }









}





