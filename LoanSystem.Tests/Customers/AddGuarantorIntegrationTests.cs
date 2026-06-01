using LoanSystem.Application.Customers.Guarantors;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Infrastructure.Database;
using LoanSystem.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoanSystem.Tests.Customers;

public class AddGuarantorIntegrationTests
{
    [Fact]
    public async Task Handle_Should_AddGuarantor_WhenCustomerHasNoGuarantors()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "AddGuarantor_NoGuarantors")
            .Options;

        using var context = new AppDbContext(options);
        var customer = new Customer(
            "Dennis Tai",
            "12345678",
            "0712345678",
            "http://photo.url",
            "Nairobi",
            "1,2",
            "Nairobi",
            "Nairobi",
            "P.O Box 1",
            Guid.NewGuid(),
            Guid.NewGuid());

        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var repository = new CustomerRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var handler = new AddGuarantorCommandHandler(repository, unitOfWork);

        var command = new AddGuarantorCommand(
            customer.Id,
            "John Doe",
            "87654321",
            "0722222222",
            1000m,
            "Uncle");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);

        var savedCustomer = await context.Customers
            .Include(c => c.Guarantors)
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        Assert.NotNull(savedCustomer);
        Assert.Single(savedCustomer!.Guarantors);
        var savedGuarantor = savedCustomer.Guarantors.First();
        Assert.Equal("John Doe", savedGuarantor.Name);
        Assert.Equal("87654321", savedGuarantor.IdNumber);
        Assert.Equal("0722222222", savedGuarantor.Phone);
        Assert.Equal(1000m, savedGuarantor.AmountGuaranteed);
        Assert.Equal("Uncle", savedGuarantor.Relationship);
    }

    [Fact]
    public async Task Handle_Should_UpdateExistingGuarantor_WhenCustomerAlreadyHasGuarantor()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "AddGuarantor_UpdateExisting")
            .Options;

        using var context = new AppDbContext(options);
        var customer = new Customer(
            "Dennis Tai",
            "12345678",
            "0712345678",
            "http://photo.url",
            "Nairobi",
            "1,2",
            "Nairobi",
            "Nairobi",
            "P.O Box 1",
            Guid.NewGuid(),
            Guid.NewGuid());

        var existingGuarantor = new Guarantor(
            customer.Id,
            "Jane Doe",
            "12345678",
            "0721111111",
            500m,
            "Sister");

        customer.Guarantors.Add(existingGuarantor);
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var repository = new CustomerRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var handler = new AddGuarantorCommandHandler(repository, unitOfWork);

        var command = new AddGuarantorCommand(
            customer.Id,
            "John Doe",
            "87654321",
            "0722222222",
            1000m,
            "Uncle");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(existingGuarantor.Id, result.Value);

        var savedCustomer = await context.Customers
            .Include(c => c.Guarantors)
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        Assert.NotNull(savedCustomer);
        Assert.Single(savedCustomer!.Guarantors);
        var savedGuarantor = savedCustomer.Guarantors.First();
        Assert.Equal("John Doe", savedGuarantor.Name);
        Assert.Equal("87654321", savedGuarantor.IdNumber);
        Assert.Equal("0722222222", savedGuarantor.Phone);
        Assert.Equal(1000m, savedGuarantor.AmountGuaranteed);
        Assert.Equal("Uncle", savedGuarantor.Relationship);
    }
}
