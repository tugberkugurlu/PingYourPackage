#PingYourPackage Project
PingYourPackage: Small city delivery company's HTTP API application (the sample application of the [Pro ASP.NET Web API book](http://www.apress.com/9781430247258)).

##Run the Application
To run the application successfully on your local environment, you need to perform a few steps first:

 1. Make sure you at least have **Visual Studio 2012** or **Visual Web Developer Express 2012** installed on your machine.
 2. Open up an elevated PowerShell command prompt.
 3. Make sure that your ExecutionPolicy set to Unrestricted. You can run the `Get-ExecutionPolicy` PowerShell command to see your execution policy. If it's not set to Unrestricted, run the following command: `Set-ExecutionPolicy Unrestricted`
 4. After setting your execution policy properly, navigate to PingYourPackage solution's root folder and run the following command: `.\AddIISExpressCertToTrustedStore.ps1` This will make the IIS Express' development SSL certificated into the Trusted Root Certificates store.

After these steps, open up the solution on **Visual Studio** or **Visual Web Developer Express**. Click on the solution and press `ALT + ENTER` to bring up the *Solution Property Pages* dialog window. From there, navigate the **Startup Project** tab and set the following project as the startup project (put them in the same order):

 - PingYourPackage.API.WebHost
 - PingYourPackage.API.Client.Web

You're all done! Hit F5 (or `CTRL + F5`) to get the application up and running.

##Development Workflow
`master` branch only holds the latest stable version of the product. Navigate to `dev` branch in order to see latest work.

##Pull Requests &amp; Branching
Every feature must be developed under a so-called feature branch and that branch must be brached off from `dev` branch.

*Pull Requests* should be targeted to `dev` branch, not `master`! Before sending the PR, make sure you have the latest `dev` branch merged into you feature branch.

#License and Copyright
This project licensed under MIT license.