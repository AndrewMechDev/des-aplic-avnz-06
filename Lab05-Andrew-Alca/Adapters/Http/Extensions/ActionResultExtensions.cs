using Microsoft.AspNetCore.Mvc;

namespace Lab05_Andrew_Alca.Adapters.Http.Extensions;

public static class ActionResultExtensions
{
    public static IActionResult ToGetResult<T>(this T? item) where T : class
        => item is null ? new NotFoundResult() : new OkObjectResult(item);

    public static IActionResult ToUpdateResult(this bool wasUpdated)
        => wasUpdated ? new NoContentResult() : new NotFoundResult();

    public static IActionResult ToDeleteResult(this bool wasDeleted)
        => wasDeleted ? new NoContentResult() : new NotFoundResult();

    public static IActionResult ToCreateResult<T>(
        this (bool Success, string? Error, T? Item) result,
        ControllerBase controller,
        string actionName,
        Func<T, object> routeValues) where T : class
    {
        if (!result.Success)
        {
            return controller.BadRequest(new { message = result.Error });
        }

        return controller.CreatedAtAction(actionName, routeValues(result.Item!), result.Item);
    }
}
