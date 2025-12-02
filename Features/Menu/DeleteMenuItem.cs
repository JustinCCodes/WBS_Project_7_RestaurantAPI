namespace Restaurant.Api.Features.Menu;

public static class DeleteMenuItem
{
    // Handler for deleting menu item
    public static async Task<IResult> HandleAsync(int id, AppDbContext db)
    {
        // Finds menu item by id
        var item = await db.MenuItems.FindAsync(id);
        // Checks if item exists
        if (item is null)
        {
            // Returns NotFound response if item does not exist
            return TypedResults.NotFound();
        }

        // Removes item from database
        db.MenuItems.Remove(item);

        // Saves changes to database
        await db.SaveChangesAsync();

        // Returns NoContent response
        return TypedResults.NoContent();
    }
}
