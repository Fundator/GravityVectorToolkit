# GravityVectorToKML

Kystkontur (land og konstruert) + skjær og grunner er hentet fra https://kartkatalog.geonorge.no/metadata/kartverket/dybdedata/2751aacf-5472-4850-a208-3532a51c529a

Kommando for å importere shapefiles til PostGIS: `for /r %v in (\*.shp) do ogr2ogr -f "PostgreSQL" PG:"dbname=gvtk user=gvtk password=gvtk" "%v" -nln "Kystkontur" -nlt geometry`

Husk på spesifisere -nln "NavnPåTabell" og -nlt geometry. Ogr2ogr vil potensielt opprette tabellen med en spesifikk geometritype (f.eks LineString) som vil fører til feilmeldinger når man kjører importen av data. Isåfall kan man generere et create-script for tabellen, droppe den, og endre geometritypen til å være kun `geometry`.

```
CREATE TABLE public.kystkontur
(
    ogc_fid integer NOT NULL DEFAULT nextval('kystkontur_ogc_fid_seq'::regclass),
    oppdaterin date,
    objtype character varying(32) COLLATE pg_catalog."default",
    id numeric(10,0),
    "fØrstedata" date,
    wkb_geometry geometry,
    kysttyp character varying(2) COLLATE pg_catalog."default",
    kystref character varying(5) COLLATE pg_catalog."default",
    dybde_min numeric(31,15),
    dybde_max numeric(31,15),
    CONSTRAINT kystkontur_pkey PRIMARY KEY (ogc_fid)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

CREATE INDEX kystkontur_wkb_geometry_geom_idx
    ON public.kystkontur USING gist
    (wkb_geometry)
    TABLESPACE pg_default;
```
