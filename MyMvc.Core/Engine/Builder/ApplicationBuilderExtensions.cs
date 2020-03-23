using MyMvc.Interfaces;

namespace MyMvc.Core
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseStaticFiles(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<StaticFiles>();
        public static void UseMvc(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<Mvc>();
    }
}
