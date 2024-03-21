using Jzor.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Jzor.Host
{
    public class JzorLoader(JzorHost _jzorHost) : PageModel
    {
        public IActionResult OnGet()
        {
            if (!_jzorHost.ResourceLimitsReached) return Page();

            return new ContentResult
            {
                StatusCode = 503,
                Content = $"503 (Jzor) - Apologies, the server has hit its maximum of {_jzorHost.AppCount} allowed applications. Please try again later.",
                ContentType = "text/plain"
            };
        }
    }
}