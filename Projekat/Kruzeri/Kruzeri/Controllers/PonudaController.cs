using System.Security.Claims;
using System.Text.RegularExpressions;
using Kruzeri.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kruzeri.Controllers;

[ApiController]
[Route("[controller]")]
public class PonudaController : ControllerBase
{

    private readonly ILogger<KorisnikController> _logger;
    private readonly INeo4jService _neo4jService;
    private readonly IConfiguration _configuration;
    public PonudaController(ILogger<KorisnikController> logger, INeo4jService neo4jService, IConfiguration configuration)
    {
        _logger = logger;
        _neo4jService = neo4jService;
        _configuration = configuration;
    }
    //create
    [HttpPost("DodajPonudu/{AgencijaID}")]
    public async Task<ActionResult<Ponuda>> DodajPonudu([FromRoute]string AgencijaID,[FromBody] Ponuda ponuda)
    {
        try
        {
            if (AgencijaID.Length<30)
                return BadRequest("Identifikator agencije kojoj se dodaje ponuda mora biti veci od 30 karaktera");
            var provera = await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (provera == null)
                return BadRequest("Agencija sa zadatim identifikatorom nije pronadjena u bazi");
            if (!ponuda.NazivAerodroma.StartsWith("Aerodrom"))
                return BadRequest("Naziv aerodroma sa koga se polazi na putovanje mora posedovati rec Aerodrom");
            if (ponuda.DatumPolaska == ponuda.DatumDolaska)
                return BadRequest("Datum polaska na putovanje mora biti razlicit od datuma dolaska sa putovanja");
            if (ponuda.CenaSmestajaSaHranom <= ponuda.CenaSmestajaBezHrane)
                return BadRequest("Cena smestaja sa uracunatom hranom mora biti veca od cene smestaja bez uracunate hrane");
            if (ponuda.CenaSmestajaBezHrane < 100)
                return BadRequest("Minimalna dozvoljena cena putovanja bez uracunate hrane jeste 100 evra");
            if (ponuda.CenaSmestajaSaHranom < 200)
                return BadRequest("Minimalna dozvoljena cena putovanja sa uracunatom hranom jeste 200 evra");
            if (ponuda.ListaGradova.Count() == 0)
                return BadRequest("Lista gradova koji se obilaze mora sadrzati makar jedan grad");
            if (ponuda.OpisPutovanja.Length < 10)
                return BadRequest("Opis putovanja koje nudi agencija mora se opisati upotrebom najmanje 10 karaktera");
            var result=await _neo4jService.DodajPonuduAsync(AgencijaID,ponuda);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //read
    [HttpGet("PribaviSvePonudeAgencije/{AgencijaID}")]
    public async Task<ActionResult<List<Ponuda>>> PribaviSvePonudeAgencije([FromRoute]string AgencijaID)
    {
        try
        {
            if (AgencijaID.Length<30)
                return BadRequest("Identifikator agencije cije se ponude pribavljaju mora biti veci od 30 karaktera");
            var provera = await _neo4jService.PribaviAgencijuPoIDAsync(AgencijaID);
            if (provera == null)
                return BadRequest("Ne postoji agencija sa zadatim IDem u bazi");
            var ponude = await _neo4jService.PribaviSvePonudeAgencijeAsync(AgencijaID);
          
            return Ok(ponude);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("PribaviPonuduPoId/{PonudaID}")]
    public async Task<ActionResult<Ponuda>> PribaviPonudePrekoID([FromRoute] string PonudaID)
    {
        if (PonudaID.Length < 30)
            return BadRequest("Identifikator ponude koja se pribavlja mora biti veci od 30 karaktera");
        var result = await _neo4jService.PribaviPonuduAsync(PonudaID);
        if (result == null)
            return BadRequest("Ponuda sa identifikatorom koji ste prosledili nije pronadjena");
        return Ok(result);
    }
    
    [HttpGet("PribaviSvePonude")]
    public async Task<ActionResult<List<Ponuda>>> PribaviSvePonude()
    {
        try
        {

            var ponude = await _neo4jService.PribaviSvePonudeAsync();
           
            return Ok(ponude);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //update
    [HttpPut("AzurirajPonudu/{PonudaID}")]
    public async Task<ActionResult<Ponuda>> AzurirajPonudu([FromRoute] string PonudaID, [FromBody] Ponuda ponuda)
    {
        try
        {
            if (PonudaID.Length<30)
                return BadRequest("Identifikator ponude mora biti veci od 30 karaktera");
            var provera = await _neo4jService.PribaviPonuduAsync(PonudaID);
            if (provera == null)
                return BadRequest("Ponuda sa zadatim identifikatorom nije pronadjena u bazi");
            if (!ponuda.NazivAerodroma.StartsWith("Aerodrom"))
                return BadRequest("Naziv aerodroma sa koga se polazi na putovanje mora posedovati rec Aerodrom");
            if (ponuda.DatumPolaska == ponuda.DatumDolaska)
                return BadRequest("Datum polaska na putovanje mora biti razlicit od datuma dolaska sa putovanja");
            if (ponuda.CenaSmestajaSaHranom <= ponuda.CenaSmestajaBezHrane)
                return BadRequest("Cena smestaja sa uracunatom hranom mora biti veca od cene smestaja bez uracunate hrane");
            if (ponuda.CenaSmestajaBezHrane < 100)
                return BadRequest("Minimalna dozvoljena cena putovanja bez uracunate hrane jeste 100 evra");
            if (ponuda.CenaSmestajaSaHranom < 200)
                return BadRequest("Minimalna dozvoljena cena putovanja sa uracunatom hranom jeste 200 evra");
            if (ponuda.ListaGradova.Count() == 0)
                return BadRequest("Lista gradova koji se obilaze mora sadrzati makar jedan grad");
            if (ponuda.OpisPutovanja.Length < 10)
                return BadRequest("Opis putovanja koje nudi agencija mora se opisati upotrebom najmanje 10 karaktera");
            var result=await _neo4jService.AzurirajPonuduAsync(PonudaID, ponuda);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //delete
    [HttpDelete("ObrisiPonudu/{PonudaID}")]
    public async Task<ActionResult> ObrisiPonudu([FromRoute] string PonudaID)
    {
        try
        {
            if (PonudaID.Length < 30)
                return BadRequest("Identifikator ponude koja se brise mora biti veci od 30 karaktera");
            var provera = await _neo4jService.PribaviPonuduAsync(PonudaID);
            if (provera == null)
                return BadRequest("Ponuda sa datim IDem ne postoji u bazi");

            await _neo4jService.ObrisiPonuduAsync(PonudaID);
            return Ok("Uspesno obrisana ponuda.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }










}





