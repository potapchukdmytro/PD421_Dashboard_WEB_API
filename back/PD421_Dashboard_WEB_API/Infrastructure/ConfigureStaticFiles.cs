using Microsoft.Extensions.FileProviders;

namespace PD421_Dashboard_WEB_API.Infrastructure
{
    public static class ConfigureStaticFiles
    {
        public static void AddStaticFiles(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            string rootPath = environment.ContentRootPath;
            string storagePath = Path.Combine(rootPath, "storage");
            string imagesPath = Path.Combine(storagePath, "images");

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/images",
                FileProvider = new PhysicalFileProvider(imagesPath)
            });
        }
    }
}
