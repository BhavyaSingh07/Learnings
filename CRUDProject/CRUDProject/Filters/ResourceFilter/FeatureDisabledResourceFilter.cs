using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CRUDProject.Filters.ResourceFilter
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly bool _isDisabled;
        public FeatureDisabledResourceFilter(bool isDisabled = true)
        {
            _isDisabled = isDisabled;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (_isDisabled)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
            }
        }
    }
}
