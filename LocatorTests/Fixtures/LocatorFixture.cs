namespace LocatorTests.Fixtures;

public class LocatorFixture : IDisposable
{
    public Locator.Locator Locator { get; private set; }
    private const string connString =
        "Server=localhost;Database=Locator;User Id=sa;Password=1StrongPwd!!;Encrypt=True;TrustServerCertificate=True;";

    public LocatorFixture()
    {
        Locator = new Locator.Locator(connString);
    }

    public void Dispose() { }
}

[CollectionDefinition("Locator")]
public class LocatorCollection : ICollectionFixture<LocatorFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
