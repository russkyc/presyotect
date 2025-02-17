using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Mvc;
using Presyotect.Core.Contracts;
using Presyotect.Utilities;

namespace Presyotect.Features.Configuration;

public class ConfigurationEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("configuration")
            .WithTags("Configuration");

        group.MapGet("/categories", OnGetCategories);
        group.MapPost("/categories", OnPostCategory);
        group.MapDelete("/categories", OnDeleteCategory);
    }

    private static async Task<IResult> OnDeleteCategory(HttpContext context, IDataStore dataStore, string category)
    {
        var response = new ResponseData<string[]>();
        if (!dataStore.GetKeys().ContainsKey("categories"))
        {
            response.Success = true;
            response.Message = "No categories available.";
            response.Content = [];
            return Results.Ok(response);
        }

        var categories = dataStore.GetItem<string[]>("categories");
        if (categories is null || categories.Length == 0)
        {
            response.Success = true;
            response.Message = "No categories available.";
            response.Content = [];
            return Results.Ok(response);
        }

        if (!categories.Contains(category))
        {
            response.Success = true;
            response.Message = $"{category} is not in category list.";
            response.Content = categories;
        
            return Results.Ok(response);
        }

        categories = categories.Where(c => !c.Equals(category))
            .ToArray();

        await dataStore.ReplaceItemAsync("categories", categories);

        response.Success = true;
        response.Message = $"Successfully removed {category} from categories.";
        response.Content = categories;
        
        return Results.Ok(response);
    }

    private static async Task<IResult> OnPostCategory(HttpContext context, IDataStore dataStore, [FromBody] string category)
    {
        string[] categories;
        var response = new ResponseData<string[]>();

        if (!dataStore.GetKeys().ContainsKey("categories"))
        {
            categories = [category];
            response.Success = true;
            response.Message = $"{category} added to categories.";
            response.Content = categories;
            await dataStore.InsertItemAsync("categories", categories);
            return Results.Ok(response);
        }
        
        categories = dataStore.GetItem<string[]>("categories");
        categories = [..categories, category];
        response.Success = true;
        response.Message = $"{category} added to categories.";
        response.Content = categories;
        await dataStore.ReplaceItemAsync("categories", categories, true);
        return Results.Ok(response);
    }

    private static async Task<IResult> OnGetCategories(HttpContext context, IDataStore dataStore)
    {
        var response = new ResponseData<string[]>();

        dataStore.Reload();
        
        if (!dataStore.GetKeys().ContainsKey("categories"))
        {
            response.Message = "No categories available.";
            response.Content = [];
            await dataStore.ReplaceItemAsync("categories", response.Content, true);
            return Results.Ok(response);
        }

        var categories = dataStore.GetItem<string[]>("categories");

        if (categories is null || categories.Length == 0)
        {
            response.Message = "No categories available";
            response.Content = [];
            return Results.Ok(response);
        }

        response.Success = true;
        response.Message = "Categories successfully listed";
        response.Content = categories;
        return Results.Ok(response);
    }
}