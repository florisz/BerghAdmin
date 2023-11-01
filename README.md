# BerghAdmin README
## Why
Bergh in het Zadel is a non-profit organization collecting money to fight cancer or as we put it "Samen voor meer tijd" (Together for more time).
For more information visit https://www.berghinhetzadel.nl/

With the size of the organization and its volunteers support is needed for all backoffice processes. This repo contains the source code vor the system that delivers this support.

## How - quick technologies inside
* Microsoft  
**Github** for hosting this repository  
**Azure devops** for backlog management and build/release pipelines  
**Azure** for running test and production version of all system components  
_Special thanks to Microsoft for their free non-profit license_
* **.Net Core** (currently vs 7)
* **MySql** database (currently vs 8.0.34)
* **Syncfusion** for UI support (currently vs 23.1.42)   
_Special thanks to Syncfusion for their free non-profit license_ 

## Help in starting as developer
Get the code at https://github.com/florisz/BerghAdmin/settings/access

* SQL Server
    - install a SQL server on your local (or antoher accessible) machine
    - create a database with name 'BIHZ2021' with a user/password with read/write permissions 
(can actually be any name but this one is most convenient)
    - install latest entity framework client tools; see https://docs.microsoft.com/en-us/ef/core/cli/dotnet +
just calling 'dotnet tool install --global dotnet-ef' will do the job

* Secrets
    - get secrets file from another developer (is not in the repo)
    - store secrets file in solution directory (keep calm, git will not synchronize it to other repos)
    - load secrets into your private secret store with ./SetSecrets.ps1
    - change the "DatabaseConfiguration:ConnectionString" with your own specific value (use the values you've chosen above and one of the commands below)
    - run 'dotnet ef database update' from the command line (this should create an empty database)

* Source code
    - start Visual studio and load the solution file
    - Open the 'Test Explorer' window and run all unit tests (this should give you a safe feeling)
    - debug/run the default project (should be BerghAdmin)

* Help with setting secrets in local store:
    - dotnet user-secrets clear --project ./BerghAdmin
    - dotnet user-secrets list --project ./BerghAdmin
    - dotnet user-secrets remove "sectie:setting" --project ./BerghAdmin
    - dotnet user-secrets add "sectie:setting" --project ./BerghAdmin
