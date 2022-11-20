

** Tretton37 **

The application uses a clean code architecture/DDD approach. 
The inner layer Domain. Although tiny, it has a core object 'TreeNode'.
The Application logic resides in Code/Application. Buisiness logic services, in this case 'TreeTraversalService'.
(Database repos whould have been stored in Infrastructure.Persistence.)
Other external access resourses belong in Infrastructure.Shared, such as web clients or file system accessors. In this project, HTTPClientService and IOService resides in this area.
Presentation layer, since this is a console applicaiton, the console is the gui.

Tests.
Dependecy injection makes this application testable, although not much is really needed or appropriate to test.
It is not meaningful to test if a file system is working.
It is not meaningful to test an external accessor such as a web service, since the result of the responses would be mocked.
The only method I found that was any purpose to test was 'RetreiveHyperLinksFromHtml'

** To Use **

In appsettings it is possible to set:
BaseUrl: From which site that is the base.
StartDirForFolderCreation: The directory base from which the html pages are stored.









  








