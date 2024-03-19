using System.Security.Claims;
using System.Text.RegularExpressions;
using Kruzeri.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kruzeri.Controllers;

[ApiController]
[Route("[controller]")]
public class AdministratorController : ControllerBase
{

    private readonly ILogger<KorisnikController> _logger;
    private readonly INeo4jService _neo4jService;
    private readonly IConfiguration _configuration;
    public AdministratorController(ILogger<KorisnikController> logger, INeo4jService neo4jService, IConfiguration configuration)
    {
        _logger = logger;
        _neo4jService = neo4jService;
        _configuration = configuration;
    }
    //create
    [HttpPost("DodajAgenciju")]
    public async Task<ActionResult<TuristickaAgencija>> DodajAgenciju([FromBody] TuristickaAgencija agencija)
    {
        try
        {
            if (agencija.Sifra.Length < 8)
                return BadRequest("Sifra mora imati najmanje 8 karaktera");
            if (agencija.Telefon.ToString().Length != 10)
                return BadRequest("Telefon mora sadrzati tacno 10 cifre");
            if (!agencija.Email.Contains("@gmail.com"))
                return BadRequest("Email mora sadrzati karaktere @gmail.com");
            if (!agencija.Email.Contains("agencija"))
                return BadRequest("Email agencije koja se dodaje mora da sadrzi rec agencija");
            var result=await _neo4jService.DodajAgencijuAsync(agencija);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
  
    //read
    [HttpGet("PribaviAgencije")]
    public async Task<ActionResult<List<TuristickaAgencija>>> PribaviAgencije()
    {
        try
        {

            var agencije = await _neo4jService.PribaviAgencijeAsync();
         
            return Ok(agencije);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class AgencijaData
    {
        public string Email { get; set; }
        public string Sifra { get; set; }
    }
    [HttpPost("PribaviAgenciju")]
    public async Task<ActionResult<TuristickaAgencija>> PribaviAgenciju([FromBody]AgencijaData data)
    {
        try
        {
            if (data.Sifra.Length < 8)
                return BadRequest("Sifra mora imati najmanje 8 karaktera");
        
            if (!data.Email.Contains("@gmail.com"))
                return BadRequest("Email mora sadrzati karaktere @gmail.com");
            var agencija = await _neo4jService.LoginAgencijaAsync(data);
            if (agencija == null)
            {
                return BadRequest("Ne postoji agencija u bazi!");
            }
            return Ok(agencija);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("PribaviAgencijuPoId/{AgencijaID}")]
    public async Task<ActionResult<TuristickaAgencija>> PribaviAgencijuPoID([FromRoute] string AgencijaID)
    {
        try
        {
            if (AgencijaID.Length<30)
                return BadRequest("Identifikator mora biti veci od 30 karaktera");

            
            var agencija = await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (agencija == null)
            {
                return BadRequest("Ne postoji agencija sa prosledjenim IDem u bazi!");
            }
            return Ok(agencija);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //update
    public class TuristickaAgencijaAzur
    {
        public required string Id { get; set; }
        public required string Naziv { get; set; }
        public required string Adresa { get; set; }
        public required long Telefon { get; set; }
        public required string Email { get; set; }
        public required string Sifra { get; set; }
    }
    [HttpPut("AzurirajAgenciju/{AgencijaID}")]
    public async Task<ActionResult<TuristickaAgencija>> AzurirajAgenciju([FromRoute] string AgencijaID, [FromBody] TuristickaAgencijaAzur agencija)
    {
        try
        {
            if (AgencijaID.Length < 30)
                return BadRequest("ID agencije mora biti veci od 30 karaktera");
            if (!agencija.Email.Contains("@gmail.com"))
                return BadRequest("Novi email agencije mora sadrzati karaktere @gmail.com");
            if (agencija.Sifra.Length < 8)
                return BadRequest("Nova sifra agencije mora sadrzati makar 8 karaktera");
            if (agencija.Telefon.ToString().Length != 10)
                return BadRequest("Novi broj telefona agencije mora sadrzati tacno 10 cifara");
            if (!agencija.Email.Contains("agencija"))
                return BadRequest("Email agencije koja se azurira mora da sadrzi rec agencija");
            var Agencija=await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (Agencija == null)
                return BadRequest("Agencija sa zadatim ID ne postoji u bazi");
            
            await _neo4jService.AzurirajAgencijuAsync(AgencijaID,agencija);
            var result=await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //delete
    [HttpDelete("ObrisiAgenciju/{AgencijaID}")]
    public async Task<ActionResult> ObrisiAgenciju([FromRoute] string AgencijaID)
    {
        try
        {
            if (AgencijaID.Length<30)
                return BadRequest("Identifikator agencije koja se brise mora biti veci od 30 karaktera");
            var provera =await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (provera == null)
                return BadRequest("Nije pronadjenja agencija za brisanje");
            await _neo4jService.ObrisiAgencijuAsync(AgencijaID);
            return Ok("Uspesno obrisana agencija.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //KORISNIK DODAJE OCENU AGENCIJI
    public class OcenaNovo
    {
        public required string AgencijaID { get; set; }
        
        public required int ProsecnaOcena { get; set; }
        public required int Korisnici { get; set; }
        public List<int> Ocene { get; set; }=new List<int>();
    }
    public class OcenaPosle
    {
        public required string AgencijaID { get; set; }

        public required int ProsecnaOcenaPre { get; set; }
        public required int KorisniciPre { get; set; }
        public List<int> OcenePre { get; set; } = new List<int>();

        public required int ProsecnaOcenaPosle { get; set; }
        public required int KorisniciPosle { get; set; }
        public List<int> OcenePosle { get; set; } = new List<int>();
    }
    [HttpPost("DodajOcenuAgenciji/{AgencijaID}/{Ocena}")]
    public async Task<ActionResult<OcenaPosle>> KorisnikDajeOcenuAgenciji([FromRoute ]string AgencijaID, [FromRoute]int Ocena)
    {
        try
        {
            if (AgencijaID.Length < 30)
                return BadRequest("ID agencije mora biti veci od 30 karaktera");
            if (Ocena < 1 || Ocena > 5)
                return BadRequest("Ocena koju korisnik daje agenciji mora biti izmedju 1 i 5");
            var provera = await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (provera == null)
                return BadRequest("Nije pronadjena agencija u bazi");

            var ocenaPosle=await _neo4jService.DodajOcenuAsync(AgencijaID,Ocena);
            var result = new OcenaPosle
            {
                AgencijaID = AgencijaID,
                ProsecnaOcenaPre = provera.ProsecnaOcena,
                KorisniciPre = provera.BrojKorisnikaKojiSuOcenili,
                OcenePre = provera.ListaOcenaKorisnika,
                ProsecnaOcenaPosle = ocenaPosle.ProsecnaOcena,
                KorisniciPosle = ocenaPosle.Korisnici,
                OcenePosle = ocenaPosle.Ocene
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
  








}





