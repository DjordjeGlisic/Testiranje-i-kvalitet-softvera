# Testiranje-i-kvalitet-softvera
naredba za kreiranje docker kontejnara docker run --name tiks-neo -p 7474:7474 -p 7687:7687 -e NEO4J_AUTH=neo4j/neo4jneo4j -d neo4j:5.15.0
Skripta za popunu baze test podacima:
CREATE (:Korisnik {
    id: "284df44e-14a7-409d-9044-22682f6ef901",
    ime: "Cypher1",
    prezime: "Cypher1",
    email: "korisnikcypher@gmail.com",
    sifra: "korisnik123",
    telefon: 1234567890,
    datumRodjenja: "11.3.2004",
    grad: "Nis",
    adresa:"Adresa 2"
});
CREATE (:Korisnik {
    id: "284df44e-14a7-409d-9044-22682f6ef903",
    ime: "Cypher1Admin",
    prezime: "Cypher1Admin",
    email: "admincypher@gmail.com",
    sifra: "administrator123",
    telefon: 1234567890,
    datumRodjenja: "11.3.2004",
    grad: "Nis",
    adresa:"Adresa 2"
});
CREATE (:Agencija {
    id: "f150e0a3-267c-47a7-bf83-a0d90bb7d62c",
    naziv:"Agencija Cypher",
    adresa: "Adresa Agencije CYpher",
    telefon: 1234567890,
    email: "agencijacypher@gmail.com",
    sifra: "agencija123",
    prosecnaOcena: 0,
    brojKorisnikaKojiSuOcenili: 0,
    listaOcenaKorisnika:[]
});
CREATE (:Ponuda {
    id: 	"9665a35e-7bb6-4e43-b4bd-13c31a8a9153",
    nazivPonude: "Cypher Ponuda",
    gradPolaskaBroda: "Marsej",
    nazivAerodroma: "Aerodrom Marsej",
    datumPolaska:"11.3.2020",
    datumDolaska:"15.3.20202",
    cenaSmestajaBezHrane: 300,
    cenaSmestajaSaHranom: 400,
    listaGradova:["Monaco","Nica","Geona"],
    opisPutovanja: "Cypher putovanje je to"
});
CREATE (:Poruka {
    id: "90754614-e821-4683-8333-dee7378c4c06",
    idKorisnika:"284df44e-14a7-409d-9044-22682f6ef901" ,
    idAgencije:"f150e0a3-267c-47a7-bf83-a0d90bb7d62c",
    datum: date(),
    sadrzaj:"Cypher poruka",
    poslataOdStraneAgencije: false,
    poslataOdStraneKorisnika: true
});
match (k:Korisnik{id:"284df44e-14a7-409d-9044-22682f6ef901"}),(a:Agencija{id:"f150e0a3-267c-47a7-bf83-a0d90bb7d62c"}),(po:Poruka{id:"90754614-e821-4683-8333-dee7378c4c06"}) merge (k)-[:SADRZI_PORUKU]->(po)<-[:SADRZI_PORUKU]-(a);
match (a:Agencija{id:"f150e0a3-267c-47a7-bf83-a0d90bb7d62c"}), (p:Ponuda{id:"9665a35e-7bb6-4e43-b4bd-13c31a8a9153"}) merge (a)-[:IMA_PONUDU]->(p);
