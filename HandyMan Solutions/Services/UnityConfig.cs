using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using HandyMan_Solutions.Services; // Adjust namespace as needed

public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();

        // Register types
        container.RegisterType<IEmailService, EmailService>();

        // Set the MVC dependency resolver to use Unity
        DependencyResolver.SetResolver(new UnityDependencyResolver(container));
    }
}
