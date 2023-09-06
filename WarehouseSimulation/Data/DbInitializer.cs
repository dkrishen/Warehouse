namespace WarehouseSimulation.Data;

public class DbInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        context.Database.EnsureCreated();
    }
}