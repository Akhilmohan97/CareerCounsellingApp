using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.PhotoCapture.Services.Server
{
    public static class PhotoCaptureEndpoints
    {
        public static void MapPhotoCaptureEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/health", async context =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Running");
            });

            endpoints.MapGet("/capture", context =>
            {
                context.Response.Redirect("/index.html");
                return Task.CompletedTask;
            });
        }
    }
}
