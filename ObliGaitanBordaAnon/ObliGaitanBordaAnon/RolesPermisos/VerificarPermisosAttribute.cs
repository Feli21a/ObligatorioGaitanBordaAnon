using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public class VerificarPermisosAttribute : ActionFilterAttribute
{
    private readonly string _permiso;

    public VerificarPermisosAttribute(string permiso)
    {
        _permiso = permiso;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var userPermissions = httpContext.Session.GetString("UserPermissions")?.Split(',');

        if (userPermissions == null || !userPermissions.Contains(_permiso) && !userPermissions.Contains("VerTodo"))
        {
            context.Result = new RedirectToActionResult("Login", "LoginVM", null);
        }
        base.OnActionExecuting(context);
    }
}