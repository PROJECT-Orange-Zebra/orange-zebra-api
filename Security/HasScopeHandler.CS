using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
	
namespace Orange.Zebra.Api.Security
{
	public class HasScopeHandler : AuthorizationHandler<HasScopeRequirements>
	{
		protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			HasScopeRequirements requirement)
		{
		
			// if user does not have scope claim, get out of here.
			
			if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer ==requirement.Issuer))
				return Task.CompletedTask;
				
			// Split the scopes string into an array
			var scopes= context.User 
				.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer)
				.Value.Split(' ');
				
			// succeed if the scope array contains te required scope
			if (scopes.Any(s => s == requirement.Scope))
				context.Succeed(requirement);
				
			return Task.CompletedTask;
			
		}
	}
}