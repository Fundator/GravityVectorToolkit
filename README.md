# GravityVectorToKML

Kystkontur (land og konstruert) + skjær og grunner er hentet fra https://kartkatalog.geonorge.no/metadata/kartverket/dybdedata/2751aacf-5472-4850-a208-3532a51c529a

Kommando for å importere shapefiles til PostGIS: for /r %v in (\*.shp) do ogr2ogr -f "PostgreSQL" PG:"dbname=gvtk user=gvtk password=gvtk" "%v" -nln "Kystkontur" -nlt geometry

Husk på spesifisere -nln "NavnPåTabell" og -nlt geometry. Ogr2ogr vil potensielt opprette tabellen med en spesifikk geometritype (f.eks LineString) som vil fører til feilmeldinger når man kjører importen av data. Isåfall kan man generere et create-script for tabellen, droppe den, og endre geometritypen til å være kun _geometry_.
