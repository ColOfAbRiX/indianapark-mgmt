@echo off

rem Genera un certificato server a nome di Fabrizio Colonna senza scadenza e lo installa tra le Autorità di
rem certificazione radice attendibili del computer locale
makecert -pe -r -cy authority -h 1 -sky signature -n "CN=Fabrizio Colonna" -sr localmachine -ss root server.cer

rem Genera un certificato server a nome di Client con scadenza a fine anno firmato dal certificato precedente
rem e lo installa in IndianaPark nel computer locale
makecert -pe -e 12/31/2012 -sky signature  -n "CN=Client" -sr localmachine -ss "IndianaPark" -ir localmachine -is root -in "Fabrizio Colonna" client.cer