# Siteimprove

The Siteimprove CMS Plugin bridges the gap between Progress Sitefinity CMS and the Siteimprove Intelligence Platform. You are able to put your Siteimprove results to use where they are most valuable – during your content creation and editing process as well as during any operation in the CMS backend pages.

## Installation

### Getting Started

- Clone the repository using the command `git clone https://github.com/Siteimprove/CMS-plugin-Sitefinity.git` and checkout the `master` branch. 

### Build the Project
- Open `Siteimprove.Integration.Sitefinity.csproj` in Visual Studio
- Make sure to restore all the NuGet packages using the [Official Sitefinity NuGet Source](http://nuget.sitefinity.com/#/home)

You will also need to make sure that your targeted Sitefinity version matches the one referenced by the Siteimprove Plugin. If you need to change the Sitefinity packages version used by this project, start with the `Telerik.Sitefinity.Core`, `Telerik.Sitefinity.Feather.Core`, and the `Telerik.DataAccess.Fluent` packages - they should update all other NuGet package dependencies.

- If you are adding the Plugin as part of a Sitefinity solution, ensure that the main SitefinityWebApp project is the startup project for the solution

### Activation
The Plugin leverages the *PreApplicationStartMethodAttribute* to hook to the Sitefinity startup process and self-install itself. This is done using the approach described in the following [blog post](https://www.sitefinity.com/blogs/peter-marinovs-blog/2013/03/20/creating-self-installing-widgets-and-modules-in-sitefinity). Assuming, you have build the project, all you need to do to activate the Plugin is to:

- Ensure that the compiled assembly is in the bin folder of your Sitefinity solution
- Once the solution runs successfully, authenticate as administrator and go to the *[Administration » Modules & Services](https://docs.sitefinity.com/activate-and-deactivate-modules)*
- Simply find the "Siteimprove Module" and install it to switch it on

If you were successful, you should see the Siteimprove overlay onto the next and every backend Sitefinity page

## Configuration Options
The Siteimprove Plugin has the following configuration options, which are located under `Settings > Advanced Settings > Siteimprove`

### Log Activity in the Browser Console
Checking this setting to True, will generate logs in the browser console, so that you can monitor the activity of the plugin. Useful for debugging on just ensuring that it works as expected.

### Script Url
With this settings, you can configure the Url for the main script that will be used by Siteimprove to load its libraries. By default, the Url should be https://cdn.siteimprove.net/cms/overlay.js

### Api Tokens
This section aggregates token/domain pairs needed to speak to the Siteimprove's API. The Plugin will automatically generate the token/domain entries if a token for the current domain is missing:
* _Domain_ - Domain managed in Siteimprove and served by Sitefinity, i.e. https://www.mydomain.com
* _Token_ - The Api token that will be used by Siteimprove to manage the respective domain, i.e. 8669723c3a4e475cbe2b303b690a3db6. It is auto generated by Siteimprove

## Going Live
- You will need the plugin installed and activated only on the Sitefinity nodes where the backend will be accessed and content publishing operations will be executed. Since it is only visible to backend users as they browse the backend, it is simply doesn't needed on pure frontend nodes
- The Plugin follows the Siteimprove integration guidelines and generates API tokens for each domain included in the Siteimprove optimization services. By default those API tokens are going to be persisted in the `App_Data/Sitefinity/Configuration/SiteimproveConfig.config` file located in your Sitefinity project and it will look something like this:

``` xml
<?xml version="1.0" encoding="utf-8"?>
<siteimproveConfig xmlns:config="urn:telerik:sitefinity:configuration" xmlns:type="urn:telerik:sitefinity:configuration:type" config:version="10.2.6600.0" logActivityInTheConsole="True">
	<tokens>
		<add token="37bf8cd59acd4f07a53cfeae8e45a853" domain="https://your.public-domain.com" />
		<add token="4df27be12550451086r0950da941e4c2" domain="https://your.other-public-domain.com" />
	</tokens>
</siteimproveConfig>
```
Thus, it is recommended to include the API tokens in your configuration transformation and environmental variables and to also generate the tokens ahead of time prior to deployment. This applies especially for Load Balanced and Cloud setups where multiple servers can operate as Sitefinity backend nodes.

If the Plugin cannot resolve the API tokens for a given domain, it will use the Siteimprove external API to fetch them and upon successful fetching will store them in the `SiteimproveConfig.config`. 

- Ensure that the web server can reach the following two Urls, on which the Siteimprove's API is being used to request tokens and recheck operations:
⋅⋅* https://api-gateway.siteimprove.com/cms-recheck
⋅⋅* https://my2.siteimprove.com/auth/token

## Troubleshooting
In case of errors, the plugin will provide verbose error logging using the Sitefinity default error logger. In case you are monitoring its production performance or are trying to troubleshoot its behavior, your should check the Sitefinity error logs for details.

### Security
Since Sitefinity 11, there is the Web Security Module, which runs alongside with Sitefinity. The module enforces many security rules, which control the resources that the user agent is allowed to load. It specifies the server origins and script endpoints for page resources.

During inital installation or upgrade, the Siteimprove Plugin should programatically modify those values, so that the required scripts are allowed to be loaded onto the Sitefinity pages. If for some reason, this operation is not succesffull, you may see "Content-Security-Policy" error in the browser.

In case of Content-Security-Policy error, ensure that the correct values are added in the WebSecurity section in the advanced configuration.
Go to `Settings > Advanced Settings > WebSecurity > HttpSecurityHeaders > Response Headers > Content-Security-Policy`. Content-Security-Policy HTTP header controls the resources that the user agent is allowed to load. The `Http header value` contains semicolon delimited values for every resource type of the Content-Security-Policy header. Edit this value and ensure the resource types from below contain the following values:
- `script-src` contains `https://cdn.siteimprove.net`
- `connect-src` contains `https://*.siteimprove.com`
- `child-src` contains `https://*.siteimprove.com`

Save these settings.

For more information on the Web Security Module in Sitefinity, visit the [official Sitefinity documentation](https://docs.sitefinity.com/predefined-security-headers-in-http-response)

## License
The code of this repository is licensed using MIT license.

Using the Siteimprove plugin falls under the [Siteimprove CMS Plugin terms](https://siteimprove.com/en/legal/cms-plugin-terms/)