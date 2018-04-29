#SingleSignOn

Requirment:
- Visual studio 2017
- Sql server 2008 or later
- .Net Core SDK 2.0 or later

Steps:
Here are some instructions for running the Single Sign On Application on your local machine.

To run the Single Sign On Application in Visual Studio you first have to create a database (Step 1 to 4).

1. In Microsoft SQL Server Management right click the Databases folder and right click select restore.
2. Select Device and click the browse button on right side.
3. In the pop up windows, click the add button and point to the back up file directory. Click OK button. 
4. Click OK 

At this point the database exists. The Single Sign On Application can now be started (Step 5 and 10).

5. Open the SingleSignOn.sln Solution project 
6. Build the solution once.
7. Go to Service folder > SingleSignOn.Service project > appsetting.json
8. Under the DefaultDatabase field, Modify the Server value on the right side. Change the "Atasa-PC" to your database server name
9. Right click the SingleSignOn.Service project > Debug > Start new instance.
10. Right click the SingleSignOn.Web project > Debug > Start new instance.

The Single Sign On Application is now running in a browser.

Login Access:

Admin Role
Username: admin123
Password: admin123

Business User Role
Username: business123
Password: business123

Guest Role
Username: guest123
Password: guest123
