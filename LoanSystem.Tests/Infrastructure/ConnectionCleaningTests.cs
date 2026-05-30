using LoanSystem.Infrastructure;

namespace LoanSystem.Tests.Infrastructure;

public class ConnectionCleaningTests
{
    [Fact]
    public void CleanConnectionString_Should_ReturnNull_WhenInputIsNull()
    {
        // Act
        var result = DependencyInjection.CleanConnectionString(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CleanConnectionString_Should_ReturnEmpty_WhenInputIsEmpty()
    {
        // Act
        var result = DependencyInjection.CleanConnectionString(string.Empty);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CleanConnectionString_Should_KeepValidProperties_AndAddDefault()
    {
        // Arrange
        var connStr = "Server=localhost;Database=LoanSystemDb;Uid=root;Pwd=root";

        // Act
        var result = DependencyInjection.CleanConnectionString(connStr);

        // Assert
        Assert.Contains("Server=localhost", result);
        Assert.Contains("Database=LoanSystemDb", result);
        Assert.Contains("Uid=root", result);
        Assert.Contains("Pwd=root", result);
        Assert.Contains("AllowPublicKeyRetrieval=True", result);
    }

    [Fact]
    public void CleanConnectionString_Should_StripSqlServerSpecificParameters()
    {
        // Arrange
        var connStr = "Server=localhost;Database=LoanSystemDb;Uid=root;Pwd=root;TrustServerCertificate=True;MultipleActiveResultSets=True;Encrypt=True;Connect Timeout=30";

        // Act
        var result = DependencyInjection.CleanConnectionString(connStr);

        // Assert
        Assert.Contains("Server=localhost", result);
        Assert.Contains("Database=LoanSystemDb", result);
        Assert.Contains("Uid=root", result);
        Assert.Contains("Pwd=root", result);
        Assert.Contains("AllowPublicKeyRetrieval=True", result);
        Assert.DoesNotContain("TrustServerCertificate", result);
        Assert.DoesNotContain("MultipleActiveResultSets", result);
        Assert.DoesNotContain("Encrypt", result);
        Assert.DoesNotContain("Connect Timeout", result);
    }

    [Fact]
    public void CleanConnectionString_Should_ParseMySQLUriFormat()
    {
        // Arrange
        var connStr = "mysql://db_user:db_password%40123@mysql-host.railway.internal:3306/railway_db";

        // Act
        var result = DependencyInjection.CleanConnectionString(connStr);

        // Assert
        Assert.Contains("Server=mysql-host.railway.internal", result);
        Assert.Contains("Port=3306", result);
        Assert.Contains("Database=railway_db", result);
        Assert.Contains("Uid=db_user", result);
        Assert.Contains("Pwd=db_password@123", result); // Unescaped
        Assert.Contains("AllowPublicKeyRetrieval=True", result);
    }
}
