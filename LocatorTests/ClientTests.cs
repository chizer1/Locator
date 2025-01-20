using System.Linq;
using Locator;
using Locator.Domain;
using LocatorTests.Fixtures;

namespace LocatorTests;

[Collection("Locator")]
public class ClientTests
{
    private readonly Locator.Locator _locator;

    public ClientTests(LocatorFixture locatorFixture)
    {
        _locator = locatorFixture.Locator;
    }

    [Fact]
    public async Task AddMultipleClientsAndSearchByKeyWord()
    {
        var clientName = StringUtilities.RandomString(10);
        var clientCode = StringUtilities.RandomString(3);
        var clientId = await _locator.AddClient(clientName, clientCode, Status.Active);

        var clientName2 = StringUtilities.RandomString(10);
        var clientCode2 = StringUtilities.RandomString(3);
        var clientId2 = await _locator.AddClient(clientName2, clientCode2, Status.Active);

        var clients = (await _locator.GetClients(clientName.Substring(0, 5), 1, 25)).Items.ToList();
        Assert.True(clients.Count() == 1);
        Assert.Equal(clientName, clients[0].Name);
        Assert.Equal(clientCode, clients[0].Code);
    }

    [Fact]
    public async Task AddAndDeleteClient()
    {
        var clientName = StringUtilities.RandomString(10);
        var clientCode = StringUtilities.RandomString(3);
        var clientId = await _locator.AddClient(clientName, clientCode, Status.Active);

        await _locator.DeleteClient(clientId);
        var client = await _locator.GetClients(clientName.Substring(0, 8), 1, 25);
        Assert.True(client.Items.Count() == 0);
    }

    [Fact]
    public async Task AddAndUpdateClient()
    {
        var clientName = StringUtilities.RandomString(10);
        var clientCode = StringUtilities.RandomString(3);
        var clientId = await _locator.AddClient(clientName, clientCode, Status.Active);

        var clientName2 = StringUtilities.RandomString(10);
        var clientCode2 = StringUtilities.RandomString(3);
        await _locator.UpdateClient(clientId, clientName2, clientCode2, Status.Inactive);

        var oldClients = (
            await _locator.GetClients(clientName.Substring(0, 8), 1, 25)
        ).Items.ToList();
        Assert.True(oldClients.Count() == 0);

        var newClients = (
            await _locator.GetClients(clientName2.Substring(0, 8), 1, 25)
        ).Items.ToList();
        Assert.Equal(clientName2, newClients[0].Name);
        Assert.Equal(clientCode2, newClients[0].Code);
        Assert.Equal(Status.Inactive, newClients[0].Status);
    }
}
