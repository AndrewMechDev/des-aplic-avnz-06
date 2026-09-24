using Microsoft.AspNetCore.Mvc;

namespace Lab05_Andrew_Alca.Extensions;

public static class ActionResultExtensions
{
    public static IActionResult ToGetResult<T>(this T? item) where T : class
        => item is null ? new NotFoundResult() : new OkObjectResult(item);

    public static IActionResult ToUpdateResult(this bool wasUpdated)
        => wasUpdated ? new NoContentResult() : new NotFoundResult();

    public static IActionResult ToDeleteResult(this bool wasDeleted)
        => wasDeleted ? new NoContentResult() : new NotFoundResult();
}
