# SincronizaListaSharepoint
Proyecto que sincroniza una lista de sharepoint desde un sistema de tercero mediante una aplicación de consola realizada en .Net.

El proyecto descarga la lista de empleados desde un sistema de tercero conectando a una base de datos SQL Server a través de ado.net.

Luego que se ha descargado la lista de empleados a un tabla en memoria, se procede a limpiar nuestra lista en sharepoint utilizando 
**Microsoft.SharePoint.Client** para interactuar con sharepoint. Luego que eliminamos los registro de nuestra lista, procedemos a agregar 
los registros que tenemos en nuestra tabla en memoria. Todo eso lo podemos realizar en la clase **SharepointInteraction**.

## Clases

- SharepointInteraction: permite acceder a las listas de sharepoint.
- SqlData: Busca información desde el sistema de recursos humanos.
- ADModel: Extrae información desde el active directoy de la empresa para complementar algunas informaciones de los empleados.


## Pre-requisitos

- Microsoft.SharePoint.Client.
- System.DirectoryServices.
- System.DirectoryServices.AccountManagement.
- System.Security.
