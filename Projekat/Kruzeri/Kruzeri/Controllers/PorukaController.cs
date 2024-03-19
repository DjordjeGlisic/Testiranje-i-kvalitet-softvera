using System.Security.Claims;
using System.Text.RegularExpressions;
using Kruzeri.Hubs;
using Kruzeri.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Kruzeri.Controllers;

[ApiController]
[Route("[controller]")]
public class PorukaController : ControllerBase
{

    private readonly ILogger<KorisnikController> _logger;
    private readonly INeo4jService _neo4jService;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<ChatHub> _hubContext;
    public PorukaController(ILogger<KorisnikController> logger, INeo4jService neo4jService, IConfiguration configuration,IHubContext<ChatHub> hubContext)
    {
        _logger = logger;
        _neo4jService = neo4jService;
        _configuration = configuration;
        _hubContext = hubContext;
    }
    //create
    [HttpPost("PosaljiPoruku")]
    public async Task<ActionResult<Poruka>> PosaljiPoruku([FromBody] Poruka poruka)
    {
        try
        {
            if (poruka.IdKorisnika.Length < 30)
                return BadRequest("Identifikator korisnika koji salje ili prima poruku mora biti veci od 30 karaktera");
            if (poruka.IdAgencije.Length< 30)
                return BadRequest("Identifikator agencije koja salje ili prima poruku mora biti veci od 30 karaktera");
            if (poruka.IdKorisnika == poruka.IdAgencije)
                return BadRequest("Identifikatori korisnika i agencije ne mogu biti ista vrednost");
            if (poruka.Sadrzaj.Length == 0)
                return BadRequest("Poruka koja se salje izmedju agencije i korisnika mora sadrzati makar 1 karakter");
            if (poruka.PoslataOdStraneKorisnika == false && poruka.PoslataOdStraneAgencije == false)
                return BadRequest("Poruku je poslao agencija ili korisnik");
            var korisnik = await _neo4jService.FindUserAsync(poruka.IdKorisnika);
            if (korisnik == null)
                return BadRequest("Korisnik sa prosledjenim IDem nije pronadjen u bazi");
            var a = await _neo4jService.PribaviAgencijuPoIDAsync(poruka.IdAgencije);
            if (a == null)
                return BadRequest("Agencija sa prosledjenim IDem nije pronadjena u bazi");

            var result=await _neo4jService.PosaljiPorukuAsync(poruka);
            //await _hubContext.Clients.All.SendAsync("MessageSetChanged");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
 
    

    [HttpGet("PribaviKorisnikeAgencije/{AgencijaID}")]
    public async Task<ActionResult<List<Korisnik>>> PribaviKorisnikeAgencije([FromRoute] string AgencijaID)
    {
        try
        {
            if (AgencijaID.Length<30)
                return BadRequest("Identifikator agencije za koju se pribavljaju korisnici sa kojima ta agencija komunicira mora biti veci od 30 karaktera");
            var agencija = await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (agencija == null)
                return BadRequest("Agencija sa zadatim IDem nije pronadjena u bazi");
            var korisnici = await _neo4jService.PribaviSveKorisnikeAsync(AgencijaID);
            List<Korisnik> listaBezDuplikata = korisnici
                .GroupBy(k => k.Id)
                .Select(g => g.First())
                .ToList();
            return Ok(listaBezDuplikata);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("PribaviCet/{KorisnikID}/{AgencijaID}")]
    public async Task<ActionResult<List<Poruka>>> PribaviCet([FromRoute] string KorisnikID ,[FromRoute] string AgencijaID)
    {
        try
        {
            if (KorisnikID.Length <30 )
                return BadRequest("Identifikator korisnika mora biti veci od 30 karaktera");
            if (AgencijaID.Length< 30)
                return BadRequest("Identifikator agencije mora biti veci od 30 karaktera");
            if (KorisnikID == AgencijaID)
                return BadRequest("Identifikatori agencije i korisnika morau biti razlicite vrednosti");
            var korisnik = await _neo4jService.FindUserAsync(KorisnikID);
            if (korisnik == null)
                return BadRequest("Korisnik nije pronadjen u bazi");
            var a = await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (a == null)
                return BadRequest("Agencija nije pronadjena u bazi");


            var cet = await _neo4jService.PribaviCetAsync(KorisnikID,AgencijaID);
            if (cet == null)
            {
                return BadRequest("Dati korisnik se nije dopisivao sa datom agencijom!");
            }
            cet.ToList().OrderBy(poruka=>DateTime.Parse(poruka.Datum));
            return Ok(cet);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("PribaviPoruku/{PorukaID}")]
    public async Task<ActionResult<Poruka>> PribaviPoruku([FromRoute]string PorukaID)
    {
        try
        {
            if (PorukaID.Length<30)
                return BadRequest("Identifikator poruke mora biti veci od 30 karaktera");
            var poruka = await _neo4jService.PribaviPorukuPoIDAsync(PorukaID);
            if (poruka == null)
                return BadRequest("Poruka nije pronadjena u bazi");
            return Ok(poruka);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //update
    [HttpPut("AzurirajPoruku/{PorukaID}")]
    public async Task<ActionResult<Poruka>> AzurirajPoruku([FromRoute] string PorukaID, [FromBody] Poruka poruka)
    {
        try
        {
            if (PorukaID.Length<30)
                return BadRequest("Idetifikator poruke koja se azurira mora biti veci od 30 karaktera");
            if (poruka.IdKorisnika.Length<30)
                return BadRequest("Identifikator korisnika koji salje ili prima poruku mora biti veci od 30 karaktera");
            if (poruka.IdAgencije.Length<30)
                return BadRequest("Identifikator agencije koja salje ili prima poruku mora biti veci od 30 karaktera");
            if (poruka.IdKorisnika == poruka.IdAgencije)
                return BadRequest("Identifikatori korisnika i agencije ne mogu biti ista vrednost");
            if (poruka.Sadrzaj.Length == 0)
                return BadRequest("Poruka koja se salje izmedju agencije i korisnika mora sadrzati makar 1 karakter");
            if (poruka.PoslataOdStraneKorisnika == false && poruka.PoslataOdStraneAgencije == false)
                return BadRequest("Poruku je poslao agencija ili korisnik");
            var p = await _neo4jService.PribaviPorukuPoIDAsync(PorukaID);
            if (p == null)
                return BadRequest("Poruka nije pronadjena u bazi");
            if (p.IdKorisnika != poruka.IdKorisnika || p.IdAgencije != poruka.IdAgencije)
                return BadRequest("Poruka mora ostati izmedju iste agencije i istog korisnika");
          
           
          var result=await _neo4jService.AzurirajPorukuAsync(PorukaID, poruka);
           // await _hubContext.Clients.All.SendAsync("MessageSetChanged");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //delete
    [HttpDelete("ObrisiPoruku/{PorukaID}")]
    public async Task<ActionResult> ObrisiPoruku([FromRoute] string PorukaID)
    {
        try
        {
            if (PorukaID.Length<30)
                return BadRequest("ID poruke mora biti veci od 30 karaktera");
            var poruka = await _neo4jService.PribaviPorukuPoIDAsync(PorukaID);
            if (poruka == null)
                return BadRequest("Poruka sa datim IDem nije pronadjena u bazi");
            await _neo4jService.ObrisiPorukuAsync(PorukaID);
          //  await _hubContext.Clients.All.SendAsync("MessageSetChanged");
            return Ok("Uspesno obrisana poruka.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }










}





