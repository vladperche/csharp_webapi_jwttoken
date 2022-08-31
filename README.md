# csharp_webapi_jwttoken
Example in CSharp with JWT Token Access Authorization.
Follow the steps below:

# Create a JWT Security Key
In local/debug Environment, add a Security Key in the launchSettings.json, in the "environmentVariables" section.

## Example
```
	"environmentVariables": {
		"JWT_SECURITY_KEY": "4fc584e4-ff47-4a7a-9359-b57cb0abfec4",
		"ASPNETCORE_ENVIRONMENT": "Development"
	}
```

In Production Environment, the key must be defined as an System Environment Variable.

## Note
In both cases, key values must be different.
The value can be any random string (for example, a UUID or GUID, or any string you want).

# Setup project's Security
Inside Startup.cs, in the ConfigureServices, call the SecurityExtenion methods to add Authentication:

```
	//Setup Jwt Security
	services.AddSecurityProviders();
	services.AddSecurityPolicies(Configuration);
```

## Note
These methods above, are defined in the class SecurityExtenion

# AuthController
Create an AuthController with a Login method.
These method must be without Authentication to let the user uses this method as an entry point to the entire system.

## Example
```
	[HttpPost("Login")]
	[AllowAnonymous]
	public ActionResult Login([FromBody] LoginRequestModel model)
```

# All other Controllers
To add Authentication check, put the annotation just like below

## Example
```
	[HttpGet("Info")]
	[Authorize(Roles = RolesEnum.ADMINISTRATOR, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public ActionResult Info()
```
