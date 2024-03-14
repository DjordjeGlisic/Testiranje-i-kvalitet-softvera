using Kruzeri.Models;
using Microsoft.AspNetCore.Identity.Data;
using Neo4j.Driver;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using static Kruzeri.Controllers.KorisnikController;
using static Kruzeri.Controllers.AdministratorController;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.PortableExecutable;
public interface INeo4jService
{

    Task<Korisnik> RegisterAsync(Korisnik korisnik);
    Task<Korisnik> LoginAsync(KorisnikData data);
    Task<Korisnik> UpdateUserAsync(string id,Korisnik korisnik);
    Task<Korisnik> FindUserAsync(string id);
    Task DeleteUserAsync(string id);
    Task RezervacijaAsync(string KorisnikID, string PonudaID);

    Task OtkazRezervacijaAsync(string KorisnikID,string  PonudaID);
    Task<TuristickaAgencija> DodajAgencijuAsync(TuristickaAgencija agencija);
    Task<IEnumerable<TuristickaAgencija>> PribaviAgencijeAsync();
    Task<TuristickaAgencijaAzur> AzurirajAgencijuAsync(string AgencijaID, TuristickaAgencijaAzur agencija);
    Task<TuristickaAgencija> LoginAgencijaAsync(AgencijaData data);
    Task<TuristickaAgencija> PribaviAgencijuPoIDAsync(string AgencijaID);
    Task ObrisiAgencijuAsync(string AgencijaID);
    Task<OcenaNovo> DodajOcenuAsync(string AgencijaID, int Ocena);

    Task<Ponuda> DodajPonuduAsync(string AgencijaID,Ponuda ponuda);

    Task<Ponuda> PribaviPonuduAsync(string PonudaID);
    Task<IEnumerable<Ponuda>> PribaviSvePonudeAgencijeAsync(string AgencijaID);
    Task<IEnumerable<Ponuda>> PribaviSvePonudeAsync();
    Task<Ponuda> AzurirajPonuduAsync(string PonudaID,Ponuda  ponuda);
    Task ObrisiPonuduAsync(string PonudaID);
    Task<Poruka> PosaljiPorukuAsync( Poruka poruka);
   
  
    Task<IEnumerable<Korisnik>> PribaviSveKorisnikeAsync(string AgencijaID);
    Task<IEnumerable<Poruka>> PribaviCetAsync(string KorisnikID,string  AgencijaID);
    Task<Poruka> AzurirajPorukuAsync(string PorukaID,Poruka poruka);
    Task ObrisiPorukuAsync(string PorukaID);
    
    Task<Poruka> PribaviPorukuPoIDAsync(string PorukaID);
    // ...

}


public class Neo4jService : INeo4jService
{

    private readonly IDriver _driver;

    public Neo4jService(string uri, string user, string password)
    {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }
    public Neo4jService(IDriver driver)
    {
        _driver = driver;
    }

    public static Neo4jService CreateInMemoryService()
    {
        var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "neo4jneo4j"));

        return new Neo4jService(driver);
    }
    ///////////////////////////////////////////////////////////////////////
    public async Task<Korisnik> RegisterAsync(Korisnik korisnik)
    {
        var session = _driver.AsyncSession();
        Guid newGuid = Guid.NewGuid();
        string uuid = newGuid.ToString();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("CREATE (k:Korisnik {id: $ID, ime: $Ime, prezime:$Prezime, telefon: $Telefon, email: $Email, sifra: $Sifra,  datumRodjenja: $DatumRodjenja,grad: $Grad,adresa: $Adresa})",
                   new
                   {

                       ID = uuid,
                       Ime = korisnik.Ime,
                       Prezime = korisnik.Prezime,
                       Telefon = korisnik.Telefon,
                       Email = korisnik.Email,
                       Sifra = korisnik.Sifra,
                       DatumRodjenja = korisnik.DatumRodjenja,
                       Grad=korisnik.Grad,
                       Adresa=korisnik.Adresa

                   });

              
            });
            korisnik.Id = uuid;
            
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            await session.CloseAsync();
        }
        return korisnik;
    }
    public async Task<Korisnik> LoginAsync(KorisnikData data)
    {
        Korisnik korisnik = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (k:Korisnik) WHERE k.email=$Email and k.sifra=$Sifra RETURN k;", new { Email = data.Email, Sifra = data.Sifra });
                while (await reader.FetchAsync())
                {
                    var guestNode = reader.Current["k"].As<INode>();


                    korisnik = new Korisnik
                    {
                        Id = guestNode.Properties["id"].As<string>(),
                        Ime = guestNode.Properties["ime"].As<string>(),
                        Prezime = guestNode.Properties["prezime"].As<string>(),
                        Telefon = guestNode.Properties["telefon"].As<long>(),
                        Email = guestNode.Properties["email"].As<string>(),
                        Sifra = guestNode.Properties["sifra"].As<string>(),
                        DatumRodjenja = guestNode.Properties["datumRodjenja"].As<string>(),
                        Grad = guestNode.Properties["grad"].As<string>(),
                        Adresa = guestNode.Properties["adresa"].As<string>(),

                    };
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return korisnik;

    }
   public async Task<Korisnik> FindUserAsync(string id)
    {
        Korisnik korisnik = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (k:Korisnik) WHERE k.id=$ID RETURN k;", new { ID=id });
                while (await reader.FetchAsync())
                {
                    var guestNode = reader.Current["k"].As<INode>();


                    korisnik = new Korisnik
                    {
                        Id = guestNode.Properties["id"].As<string>(),
                        Ime = guestNode.Properties["ime"].As<string>(),
                        Prezime = guestNode.Properties["prezime"].As<string>(),
                        Telefon = guestNode.Properties["telefon"].As<long>(),
                        Email = guestNode.Properties["email"].As<string>(),
                        Sifra = guestNode.Properties["sifra"].As<string>(),
                        DatumRodjenja = guestNode.Properties["datumRodjenja"].As<string>(),
                        Grad = guestNode.Properties["grad"].As<string>(),
                        Adresa = guestNode.Properties["adresa"].As<string>(),

                    };
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return korisnik;
    }
    public async Task<Korisnik> UpdateUserAsync(string id, Korisnik korisnik)
    {
        Korisnik user = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("MATCH (k:Korisnik {id: $Id}) SET k.ime = $Ime, k.prezime=$Prezime,k.email = $Email, k.sifra = $Sifra, k.telefon = $Telefon, k.datumRodjenja = $DatumRodjenja, k.grad = $Grad,k.adresa = $Adresa",
                    new { Id = id, Ime = korisnik.Ime, Prezime = korisnik.Prezime, Email = korisnik.Email,Sifra = korisnik.Sifra,Telefon = korisnik.Telefon, DatumRodjenja = korisnik.DatumRodjenja,Grad=korisnik.Grad,Adresa=korisnik.Adresa });


            });
            korisnik.Id = id;
            user = korisnik;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            await session.CloseAsync();
        }
        return user;
    }


    public async Task DeleteUserAsync(string id)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                
                var deletePonudaQuery = "MATCH (k:Korisnik {id: $id})-[rel:IMA_REZERVACIJU]->(p:Pounda) DETACH DELETE rel";
                await tx.RunAsync(deletePonudaQuery, new { id = id });
                var deleteAgencijaQuery = "MATCH (k:Korisnik {id: $id})-[rel1:SADRZI_PORUKU]->(po:Poruka)<-[rel2:SADRZI_PORUKU]-(a:Agencija) DETACH DELETE rel1,rel2";
                await tx.RunAsync(deleteAgencijaQuery, new { id = id });
               
                
                var deleteKorisnikQuery = "MATCH (k:Korisnik {id: $id}) DETACH DELETE k";
                await tx.RunAsync(deleteKorisnikQuery, new { id = id });
            });
        }
        finally
        {
            await session.CloseAsync();
        }
    }
    public async Task RezervacijaAsync(string KorisnikID, string PonudaID)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {



                await tx.RunAsync("MATCH (k:Korisnik {id: $KorisnikID}), (p:Ponuda {id: $PonudaID}) MERGE (k)-[:IMA_REZERVACIJU ]->(p)", new { KorisnikID = KorisnikID, PonudaID = PonudaID });


            });
        }
        finally
        {
            await session.CloseAsync();
        }
    }
    public async Task OtkazRezervacijaAsync(string KorisnikID,string PonudaID)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {



                await tx.RunAsync("MATCH (k:Korisnik{id: $KorisnikID})-[rel:IMA_REZERVACIJU]->(p:Ponuda{id: $PonudaID}) delete rel;", new { KorisnikID = KorisnikID, PonudaID = PonudaID });


            });
        }
        finally
        {
            await session.CloseAsync();
        }
    }
    ////////////////////////////////////////////////////////////////////////



    public async Task<TuristickaAgencija> DodajAgencijuAsync(TuristickaAgencija agencija)
    {
        var session = _driver.AsyncSession();
        Guid newGuid = Guid.NewGuid();
        string uuid = newGuid.ToString();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("CREATE (a:Agencija {id: $ID, prosecnaOcena: $ProsecnaOcena, naziv:$Naziv, adresa: $Adresa, telefon: $Telefon, email: $Email,  sifra: $Sifra,brojKorisnikaKojiSuOcenili: $Korisnici,listaOcenaKorisnika:[]})",
                   new
                   {

                       ID = uuid,
                       Naziv = agencija.Naziv,
                       Adresa = agencija.Adresa,
                       Telefon = agencija.Telefon,
                       Email = agencija.Email,
                       Sifra = agencija.Sifra,
                       ProsecnaOcena = 0,
                       Korisnici=0
                      

                       
                   });


            });
            agencija.Id = uuid;
            agencija.ProsecnaOcena = 0;
            agencija.BrojKorisnikaKojiSuOcenili = 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            await session.CloseAsync();
        }
        return agencija;
    }



    public async Task<IEnumerable<TuristickaAgencija>> PribaviAgencijeAsync()
    {
        var agencije =  new List<TuristickaAgencija>();
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (a:Agencija) RETURN a");
                while (await reader.FetchAsync())
                {
                    var agencijaNode = reader.Current["a"].As<INode>();

                    agencije.Add(new TuristickaAgencija
                    {


                        Id = agencijaNode.Properties.ContainsKey("id") ? agencijaNode.Properties["id"].As<string>() :"",
                        Naziv = agencijaNode.Properties.ContainsKey("naziv") ? agencijaNode.Properties["naziv"].As<string>():"",
                        Adresa = agencijaNode.Properties.ContainsKey("adresa") ? agencijaNode.Properties["adresa"].As<string>():"",
                        Telefon = agencijaNode.Properties.ContainsKey("telefon") ? agencijaNode.Properties["telefon"].As<long>():default,
                        Email = agencijaNode.Properties.ContainsKey("email") ? agencijaNode.Properties["email"].As<string>() : "",
                        Sifra = agencijaNode.Properties.ContainsKey("sifra") ? agencijaNode.Properties["sifra"].As<string>() : "",
                        ProsecnaOcena = agencijaNode.Properties.ContainsKey("prosecnaOcena") ? agencijaNode.Properties["prosecnaOcena"].As<int>() : default,
                        BrojKorisnikaKojiSuOcenili = agencijaNode.Properties["brojKorisnikaKojiSuOcenili"].As<int>(),
                        ListaOcenaKorisnika = agencijaNode.Properties.TryGetValue("listaOcenaKorisnika", out var value) && value != null ? value.As<List<int>>() : null,


                    });
                }


            });
        }
        
        finally
        {
            await session.CloseAsync();
        }

        return agencije;


    }
     public async Task<TuristickaAgencijaAzur> AzurirajAgencijuAsync(string AgencijaID,TuristickaAgencijaAzur agencija)
    {
         
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("MATCH (a:Agencija {id: $AgencijaId}) SET a.naziv = $Naziv, a.adresa=$Adresa,a.telefon = $Telefon, a.email = $Email, a.sifra = $Sifra",
                    new { AgencijaId = AgencijaID, Naziv = agencija.Naziv, Adresa = agencija.Adresa, Telefon = agencija.Telefon, Email = agencija.Email, Sifra = agencija.Sifra});


            });
            agencija.Id = AgencijaID;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            
            await session.CloseAsync();
            
        }
        return agencija;

    }
    public async Task ObrisiAgencijuAsync(string AgencijaID)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                var deleteAllQuery = "MATCH (a:Agencija {id: $AgencijaID})-[rel:IMA_PONUDU]->(p:Pounda)<-[relacija:IMA_REZERVACIJU]-(k:Korisnik) DETACH DELETE relacija,rel,p";
                await tx.RunAsync(deleteAllQuery, new { AgencijaID = AgencijaID });
                var deleteKorisnikQuery = "MATCH (k:Korisnik)-[rel1:SALJE_PORUKU]->(po:Poruka)<-[rel2:SALJE_PORUKU]-(a:Agencija{id:$AgencijaID}) DETACH DELETE rel1,rel2";
                await tx.RunAsync(deleteKorisnikQuery, new { AgencijaID = AgencijaID });

                
                var deleteAgencijaQuery = "MATCH (a:Agencija {id: $AgencijaID}) DETACH DELETE a";
                await tx.RunAsync(deleteAgencijaQuery, new { AgencijaID = AgencijaID });
            });
        }
        finally
        {
            await session.CloseAsync();
        }
    }
    public async Task<TuristickaAgencija> LoginAgencijaAsync(AgencijaData data)
    {
        TuristickaAgencija agencija = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (a:Agencija) WHERE a.email=$Email and a.sifra=$Sifra RETURN a;", new { Email = data.Email, Sifra = data.Sifra });
                while (await reader.FetchAsync())
                {
                    var guestNode = reader.Current["a"].As<INode>();


                    agencija = new TuristickaAgencija
                    {
                        Id = guestNode.Properties["id"].As<string>(),
                        Naziv = guestNode.Properties["naziv"].As<string>(),
                        Adresa = guestNode.Properties["adresa"].As<string>(),
                        Telefon = guestNode.Properties["telefon"].As<long>(),
                        Email = guestNode.Properties["email"].As<string>(),
                        Sifra = guestNode.Properties["sifra"].As<string>(),
                        ProsecnaOcena = guestNode.Properties["prosecnaOcena"].As<int>(),
                        BrojKorisnikaKojiSuOcenili = guestNode.Properties["brojKorisnikaKojiSuOcenili"].As<int>(),
                        ListaOcenaKorisnika = guestNode.Properties.TryGetValue("listaOcenaKorisnika", out var value) && value != null ? value.As<List<int>>() : null,

                    };
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return agencija;
    }
    public async Task<TuristickaAgencija> PribaviAgencijuPoIDAsync(string AgencijaID)
    {
        TuristickaAgencija agencija = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                try
                {
                    var reader = await tx.RunAsync("MATCH (a:Agencija) WHERE a.id=$AgencijaID RETURN a;", new { AgencijaID = AgencijaID });
                    while (await reader.FetchAsync())
                    {
                        var guestNode = reader.Current["a"].As<INode>();


                        agencija = new TuristickaAgencija
                        {
                            Id = guestNode.Properties["id"].As<string>(),
                            Naziv = guestNode.Properties["naziv"].As<string>(),
                            Adresa = guestNode.Properties["adresa"].As<string>(),
                            Telefon = guestNode.Properties["telefon"].As<long>(),
                            Email = guestNode.Properties["email"].As<string>(),
                            Sifra = guestNode.Properties["sifra"].As<string>(),
                            ProsecnaOcena = guestNode.Properties["prosecnaOcena"].As<int>(),
                            BrojKorisnikaKojiSuOcenili = guestNode.Properties["brojKorisnikaKojiSuOcenili"].As<int>(),
                            ListaOcenaKorisnika = guestNode.Properties.TryGetValue("listaOcenaKorisnika", out var value) && value != null ? value.As<List<int>>() : null,

                        };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            });
        }
        catch
        {
            Console.WriteLine("Cacth");
        }
        finally
        {
            await session.CloseAsync();
        }
        return agencija;

    }
    public async Task<OcenaNovo> DodajOcenuAsync( string AgencijaID, int Ocena)
    {
        TuristickaAgencija agencija = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (a:Agencija) WHERE a.id=$AgencijaID RETURN a;", new { AgencijaID = AgencijaID });
                while (await reader.FetchAsync())
                {
                    var guestNode = reader.Current["a"].As<INode>();


                    agencija = new TuristickaAgencija
                    {
                        Id = guestNode.Properties["id"].As<string>(),
                        Naziv = guestNode.Properties["naziv"].As<string>(),
                        Adresa = guestNode.Properties["adresa"].As<string>(),
                        Telefon = guestNode.Properties["telefon"].As<long>(),
                        Email = guestNode.Properties["email"].As<string>(),
                        Sifra = guestNode.Properties["sifra"].As<string>(),
                        ProsecnaOcena = guestNode.Properties["prosecnaOcena"].As<int>(),
                        BrojKorisnikaKojiSuOcenili = guestNode.Properties["brojKorisnikaKojiSuOcenili"].As<int>(),
                        ListaOcenaKorisnika = guestNode.Properties.TryGetValue("listaOcenaKorisnika", out var value) && value != null ? value.As<List<int>>() : null,

                    };
                }
                agencija.ListaOcenaKorisnika.Add(Ocena);
                agencija.BrojKorisnikaKojiSuOcenili += 1;
                int sum = 0;
                foreach(int ocena in agencija.ListaOcenaKorisnika)
                {
                    sum += ocena;
                }
                agencija.ProsecnaOcena = (int)sum / agencija.BrojKorisnikaKojiSuOcenili;
               
               

            });
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("MATCH (a:Agencija {id: $AgencijaId}) SET a.prosecnaOcena = $ProsecnaOcena, a.brojKorisnikaKojiSuOcenili=$BrojKorisnikaKojiSuOcenili,a.listaOcenaKorisnika = $ListaOcenaKorisnika",
                    new { AgencijaId = AgencijaID, ProsecnaOcena = agencija.ProsecnaOcena, BrojKorisnikaKojiSuOcenili = agencija.BrojKorisnikaKojiSuOcenili, ListaOcenaKorisnika = agencija.ListaOcenaKorisnika });


            });
        }
        finally
        {
            await session.CloseAsync();
        }
        var rating = new OcenaNovo
        {
         AgencijaID=AgencijaID,
        ProsecnaOcena=agencija.ProsecnaOcena,
        Korisnici=agencija.BrojKorisnikaKojiSuOcenili,
        Ocene=agencija.ListaOcenaKorisnika
        };
        return rating;
    }

    /// ////////////////////////////////////////////////////////////////////////////////////
    public async Task<Ponuda> DodajPonuduAsync(string AgencijaID,Ponuda ponuda)
    {
        var session = _driver.AsyncSession();

        Guid newGuid = Guid.NewGuid();
        string uuid = newGuid.ToString();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("CREATE (p:Ponuda {id: $ID, nazivPonude: $NazivPonude, gradPolaskaBroda:$GradPolaskaBroda, nazivAerodroma: $NazivAerodroma, datumPolaska: $DatumPolaska, datumDolaska: $DatumDolaska,  cenaSmestajaBezHrane: $CenaSmestajaBezHrane,  cenaSmestajaSaHranom: $CenaSmestajaSaHranom,  listaGradova: $ListaGradova,opisPutovanja: $OpisPutovanja})",
                   new
                   {

                       ID = uuid,
                       NazivPonude = ponuda.NazivPonude,
                       GradPolaskaBroda = ponuda.GradPolaskaBroda,
                       NazivAerodroma = ponuda.NazivAerodroma,
                       DatumPolaska = ponuda.DatumPolaska,
                       DatumDolaska = ponuda.DatumDolaska,
                       CenaSmestajaBezHrane = ponuda.CenaSmestajaBezHrane,
                       CenaSmestajaSaHranom=ponuda.CenaSmestajaSaHranom,
                       ListaGradova=ponuda.ListaGradova,
                       OpisPutovanja=ponuda.OpisPutovanja,
                       
                     

                   });
                await tx.RunAsync("MATCH (a:Agencija {id: $AgencijaID}), (p:Ponuda {id: $PonudaID}) MERGE (a)-[:IMA_PONUDU ]->(p)", new { AgencijaID = AgencijaID, PonudaID = uuid});


            });
            ponuda.Id = uuid;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            await session.CloseAsync();
        }
        
        return ponuda;
    }
    public async Task<IEnumerable<Ponuda>> PribaviSvePonudeAgencijeAsync(string AgencijaID)
    {
        var ponude = new List<Ponuda>();
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (a:Agencija{id: $AgencijaID})-[rel:IMA_PONUDU]-(p:Ponuda) RETURN p", new { AgencijaID = AgencijaID });
                while (await reader.FetchAsync())
                {
                    var ponudeNode = reader.Current["p"].As<INode>();

                    ponude.Add(new Ponuda
                    {


                        Id = ponudeNode.Properties.ContainsKey("id") ? ponudeNode.Properties["id"].As<string>() : "",
                        NazivPonude = ponudeNode.Properties.ContainsKey("nazivPonude") ? ponudeNode.Properties["nazivPonude"].As<string>() : "",
                        GradPolaskaBroda = ponudeNode.Properties.ContainsKey("gradPolaskaBroda") ? ponudeNode.Properties["gradPolaskaBroda"].As<string>() : "",
                        NazivAerodroma = ponudeNode.Properties.ContainsKey("nazivAerodroma") ? ponudeNode.Properties["nazivAerodroma"].As<string>() : "",
                        DatumPolaska = ponudeNode.Properties.ContainsKey("datumPolaska") ? ponudeNode.Properties["datumPolaska"].As<string>() : "",
                        DatumDolaska = ponudeNode.Properties.ContainsKey("datumDolaska") ? ponudeNode.Properties["datumDolaska"].As<string>() : "",
                        CenaSmestajaBezHrane = ponudeNode.Properties.ContainsKey("cenaSmestajaBezHrane") ? ponudeNode.Properties["cenaSmestajaBezHrane"].As<int>() : default,
                        CenaSmestajaSaHranom = ponudeNode.Properties.ContainsKey("cenaSmestajaSaHranom") ? ponudeNode.Properties["cenaSmestajaSaHranom"].As<int>() : default,
                        ListaGradova = ponudeNode.Properties.TryGetValue("listaGradova", out var value) && value != null ? value.As<List<string>>() : null,
                        OpisPutovanja = ponudeNode.Properties.ContainsKey("opisPutovanja") ? ponudeNode.Properties["opisPutovanja"].As<string>() : "",
                        
                    







                    });
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return ponude;
    }
    public async Task<IEnumerable<Ponuda>> PribaviSvePonudeAsync()
    {
        var ponude = new List<Ponuda>();
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (p:Ponuda) RETURN p;");
                while (await reader.FetchAsync())
                {
                    var ponudeNode = reader.Current["p"].As<INode>();

                    ponude.Add(new Ponuda
                    {


                        Id = ponudeNode.Properties.ContainsKey("id") ? ponudeNode.Properties["id"].As<string>() : "",
                        NazivPonude = ponudeNode.Properties.ContainsKey("nazivPonude") ? ponudeNode.Properties["nazivPonude"].As<string>() : "",
                        GradPolaskaBroda = ponudeNode.Properties.ContainsKey("gradPolaskaBroda") ? ponudeNode.Properties["gradPolaskaBroda"].As<string>() : "",
                        NazivAerodroma = ponudeNode.Properties.ContainsKey("nazivAerodroma") ? ponudeNode.Properties["nazivAerodroma"].As<string>() : "",
                        DatumPolaska = ponudeNode.Properties.ContainsKey("datumPolaska") ? ponudeNode.Properties["datumPolaska"].As<string>() : "",
                        DatumDolaska = ponudeNode.Properties.ContainsKey("datumDolaska") ? ponudeNode.Properties["datumDolaska"].As<string>() : "",
                        CenaSmestajaBezHrane = ponudeNode.Properties.ContainsKey("cenaSmestajaBezHrane") ? ponudeNode.Properties["cenaSmestajaBezHrane"].As<int>() : default,
                        CenaSmestajaSaHranom = ponudeNode.Properties.ContainsKey("cenaSmestajaSaHranom") ? ponudeNode.Properties["cenaSmestajaSaHranom"].As<int>() : default,
                        ListaGradova = ponudeNode.Properties.TryGetValue("listaGradova", out var value) && value != null ? value.As<List<string>>() : null,
                        OpisPutovanja = ponudeNode.Properties.ContainsKey("opisPutovanja") ? ponudeNode.Properties["opisPutovanja"].As<string>() : "",
                        
                       








                    });
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return ponude;

    }
    public async Task<Ponuda> PribaviPonuduAsync(string PonudaID)
    {
        Ponuda ponuda = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (p:Ponuda) WHERE p.id=$PonudaID RETURN p;", new { PonudaID = PonudaID });
                while (await reader.FetchAsync())
                {
                    var ponudeNode = reader.Current["p"].As<INode>();


                    ponuda = new Ponuda
                    {
                        Id = ponudeNode.Properties.ContainsKey("id") ? ponudeNode.Properties["id"].As<string>() : "",
                        NazivPonude = ponudeNode.Properties.ContainsKey("nazivPonude") ? ponudeNode.Properties["nazivPonude"].As<string>() : "",
                        GradPolaskaBroda = ponudeNode.Properties.ContainsKey("gradPolaskaBroda") ? ponudeNode.Properties["gradPolaskaBroda"].As<string>() : "",
                        NazivAerodroma = ponudeNode.Properties.ContainsKey("nazivAerodroma") ? ponudeNode.Properties["nazivAerodroma"].As<string>() : "",
                        DatumPolaska = ponudeNode.Properties.ContainsKey("datumPolaska") ? ponudeNode.Properties["datumPolaska"].As<string>() : "",
                        DatumDolaska = ponudeNode.Properties.ContainsKey("datumDolaska") ? ponudeNode.Properties["datumDolaska"].As<string>() : "",
                        CenaSmestajaBezHrane = ponudeNode.Properties.ContainsKey("cenaSmestajaBezHrane") ? ponudeNode.Properties["cenaSmestajaBezHrane"].As<int>() : default,
                        CenaSmestajaSaHranom = ponudeNode.Properties.ContainsKey("cenaSmestajaSaHranom") ? ponudeNode.Properties["cenaSmestajaSaHranom"].As<int>() : default,
                        ListaGradova = ponudeNode.Properties.TryGetValue("listaGradova", out var value) && value != null ? value.As<List<string>>() : null,
                        OpisPutovanja = ponudeNode.Properties.ContainsKey("opisPutovanja") ? ponudeNode.Properties["opisPutovanja"].As<string>() : "",
                        
                     

                    };
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return ponuda;
    }
  
    public async Task<Ponuda> AzurirajPonuduAsync(string PonudaID, Ponuda ponuda)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("MATCH (p:Ponuda {id: $PonudaId}) SET p.nazivPonude = $NazivPonude, p.gradPolaskaBroda = $GradPolaskaBroda,p.nazivAerodroma = $NazivAerodroma, p.datumPolaska = $DatumPolaska, p.datumDolaska = $DatumDolaska,p.cenaSmestajaBezHrane = $CenaSmestajaBezHrane,p.cenaSmestajaSaHranom = $CenaSmestajaSaHranom,p.listaGradova = $ListaGradova,p.opisPutovanja = $OpisPutovanja",
                    new {
                        PonudaId = PonudaID,
                        NazivPonude=ponuda.NazivPonude,
                        GradPolaskaBroda = ponuda.GradPolaskaBroda,
                        NazivAerodroma=ponuda.NazivAerodroma,
                        DatumPolaska=ponuda.DatumPolaska,
                        DatumDolaska=ponuda.DatumDolaska,
                        CenaSmestajaBezHrane=ponuda.CenaSmestajaBezHrane,
                        CenaSmestajaSaHranom=ponuda.CenaSmestajaSaHranom,
                        ListaGradova=ponuda.ListaGradova,
                        OpisPutovanja=ponuda.OpisPutovanja




                    });


            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            await session.CloseAsync();
        }
        return ponuda;
    }
    public async Task  ObrisiPonuduAsync(string PonudaID)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
               
                var deleteAgencijaQuery = "MATCH (a:Agencija)-[rel:IMA_PONUDU]->(p:Pounda{id: $PonudaID}) DETACH DELETE rel";
                await tx.RunAsync(deleteAgencijaQuery, new { PonudaID = PonudaID });
                var deleteKorisnikQuery = "MATCH (k:Korisnik)-[rel:IMA_REZERVACIJU]->(p:Ponuda{id:$PonudaID}) DETACH DELETE rel";
                await tx.RunAsync(deleteKorisnikQuery, new { PonudaID = PonudaID });

                 
                var deletePonudaQuery = "MATCH (p:Ponuda {id: $PonudaID}) DETACH DELETE p";
                await tx.RunAsync(deletePonudaQuery, new { PonudaID = PonudaID });
            });
        }
        finally
        {
            await session.CloseAsync();
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////
  
    public async Task<Poruka> PosaljiPorukuAsync(Poruka poruka)
    {
        var session = _driver.AsyncSession();
        Guid newGuid = Guid.NewGuid();
        string uuid = newGuid.ToString();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("CREATE (po:Poruka {id: $ID, idKorisnika: $KorisnikID, idAgencije:$IdAgencije, poslataOdStraneAgencije: $PoslataOdStraneAgencije,poslataOdStraneKorisnika: $PoslataOdStraneKorisnika ,datum: $Datum, sadrzaj: $Sadrzaj})",
                   new
                   {

                       ID = uuid,
                       KorisnikID = poruka.IdKorisnika,
                       IdAgencije = poruka.IdAgencije,
                       PoslataOdStraneAgencije = poruka.PoslataOdStraneAgencije,
                       PoslataOdStraneKorisnika=poruka.PoslataOdStraneKorisnika,
                       Datum = poruka.Datum,
                       Sadrzaj = poruka.Sadrzaj,

                   });
                await tx.RunAsync("MATCH (k:Korisnik {id: $KorisnikID}), (a:Agencija {id: $AgencijaID}),(po:Poruka {id: $PorukaID}) MERGE (k)-[:SADRZI_PORUKU ]->(po)<-[:SADRZI_PORUKU]-(a)", new { AgencijaID = poruka.IdAgencije, KorisnikID = poruka.IdKorisnika, PorukaID = uuid });
                poruka.Id = uuid;
                //poruka.Datum = DateTime.Now.ToString();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            await session.CloseAsync();
        }
        return poruka;
    }
  
    public async Task<IEnumerable<Korisnik>> PribaviSveKorisnikeAsync(string AgencijaID)
    {
        var korisnici = new List<Korisnik>();
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (a:Agencija{id: $AgencijaID})-[:SADRZI_PORUKU]->(po:Poruka)<-[:SADRZI_PORUKU]-(k:Korisnik) RETURN k", new { AgencijaID = AgencijaID });
                while (await reader.FetchAsync())
                {
                    var guestNode = reader.Current["k"].As<INode>();

                    korisnici.Add(new Korisnik
                    {

                        
                        Id = guestNode.Properties.ContainsKey("id") ? guestNode.Properties["id"].As<string>():"",
                        Ime = guestNode.Properties.ContainsKey("ime") ? guestNode.Properties["ime"].As<string>():"",
                        Prezime = guestNode.Properties.ContainsKey("prezime") ? guestNode.Properties["prezime"].As<string>():"",
                        Telefon = guestNode.Properties.ContainsKey("telefon") ? guestNode.Properties["telefon"].As<long>():default,
                        Email = guestNode.Properties.ContainsKey("email") ? guestNode.Properties["email"].As<string>():"",
                        Sifra = guestNode.Properties.ContainsKey("sifra") ? guestNode.Properties["sifra"].As<string>():"",
                        DatumRodjenja = guestNode.Properties.ContainsKey("datumRodjenja") ? guestNode.Properties["datumRodjenja"].As<string>():"",
                        Grad = guestNode.Properties.ContainsKey("grad") ? guestNode.Properties["grad"].As<string>():"",
                        Adresa = guestNode.Properties.ContainsKey("adresa") ? guestNode.Properties["adresa"].As<string>():"",


                    });
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return korisnici;

    }
    public async Task<IEnumerable<Poruka>> PribaviCetAsync(string KorisnikID, string AgencijaID)
    {
        var poruke = new List<Poruka>();
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (po:Poruka) WHERE po.idKorisnika= $KorisnikID AND po.idAgencije= $AgencijaID return po", new {KorisnikID=KorisnikID, AgencijaID = AgencijaID });
                while (await reader.FetchAsync())
                {
                    var porukaNode = reader.Current["po"].As<INode>();

                    poruke.Add(new Poruka
                    {



                        Id = porukaNode.Properties.ContainsKey("id") ? porukaNode.Properties["id"].As<string>() : "",
                        IdKorisnika = porukaNode.Properties.ContainsKey("idKorisnika") ? porukaNode.Properties["idKorisnika"].As<string>() : "",
                        IdAgencije = porukaNode.Properties.ContainsKey("idAgencije") ? porukaNode.Properties["idAgencije"].As<string>() : "",
                        Datum = porukaNode.Properties.ContainsKey("datum") ? porukaNode.Properties["id"].As<string>() : "",
                       PoslataOdStraneKorisnika = porukaNode.Properties.ContainsKey("poslataOdStraneKorisnika") ? porukaNode.Properties["poslataOdStraneKorisnika"].As<bool>() : default,
                        PoslataOdStraneAgencije = porukaNode.Properties.ContainsKey("poslataOdStraneAgencije") ? porukaNode.Properties["poslataOdStraneAgencije"].As<bool>() : default,
                        Sadrzaj = porukaNode.Properties.ContainsKey("sadrzaj") ? porukaNode.Properties["sadrzaj"].As<string>() : "",








                    });
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return poruke;
    }
    public async Task<Poruka> PribaviPorukuPoIDAsync(string PorukaID)
    {
        Poruka poruka = null;
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteReadAsync(async tx =>
            {
                var reader = await tx.RunAsync("MATCH (po:Poruka) WHERE po.id=$PorukaID RETURN po;", new { PorukaID = PorukaID });
                while (await reader.FetchAsync())
                {
                    var porukaNode = reader.Current["po"].As<INode>();


                    poruka = new Poruka
                    {
                        Id = porukaNode.Properties.ContainsKey("id") ? porukaNode.Properties["id"].As<string>() : "",
                        IdKorisnika = porukaNode.Properties.ContainsKey("idKorisnika") ? porukaNode.Properties["idKorisnika"].As<string>() : "",
                        IdAgencije = porukaNode.Properties.ContainsKey("idAgencije") ? porukaNode.Properties["idAgencije"].As<string>() : "",
                        Datum = porukaNode.Properties.ContainsKey("datum") ? porukaNode.Properties["id"].As<string>() : "",
                        PoslataOdStraneKorisnika = porukaNode.Properties.ContainsKey("poslataOdStraneKorisnika") ? porukaNode.Properties["poslataOdStraneKorisnika"].As<bool>() : default,
                        PoslataOdStraneAgencije = porukaNode.Properties.ContainsKey("poslataOdStraneAgencij") ? porukaNode.Properties["poslataOdStraneAgencije"].As<bool>() : default,
                        Sadrzaj = porukaNode.Properties.ContainsKey("sadrzaj") ? porukaNode.Properties["sadrzaj"].As<string>() : "",

                    };
                }


            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return poruka;
    }
    public async Task<Poruka> AzurirajPorukuAsync(string PorukaID, Poruka poruka)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                await tx.RunAsync("MATCH (po:Poruka {id: $PorukaID}) SET po.datum = $Datum, po.sadrzaj = $Sadrzaj",
                    new
                    {
                        PorukaID=PorukaID,
                        Datum=poruka.Datum,
                        Sadrzaj=poruka.Sadrzaj,
                      



                    });


            });
            //poruka.Datum=DateTime.Now.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            await session.CloseAsync();
        }
        return poruka;
    }
    public async Task ObrisiPorukuAsync(string PorukaID)
    {
        var session = _driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {

                var deleteChatQuery = "MATCH (a:Agencija)-[rel1:SADRZI_PORUKU]->(po:Poruka{id: $PorukaID})<-[rel2:SADRZI_PORUKU]-(k:Korisnik) DETACH DELETE rel1,rel2";
                await tx.RunAsync(deleteChatQuery, new { PorukaID = PorukaID });
                

                var deletePorukaQuery = "MATCH (pp:Poruka {id: $PorukaID}) DETACH DELETE pp";
                await tx.RunAsync(deletePorukaQuery, new { PorukaID = PorukaID });
            });
        }
        finally
        {
            await session.CloseAsync();
        }

    }
    /// /////////////////////////////////////////////////////////////////////////////////////































    public void Dispose()
    {
        _driver?.Dispose();
    }
}