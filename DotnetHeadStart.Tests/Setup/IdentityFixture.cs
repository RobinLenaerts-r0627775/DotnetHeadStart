﻿namespace DotnetHeadStart.Tests;

public class IdentityFixture
{
    public UserManager<TestUser> UserManager { get; }

    public IdentityFixture()
    {
        // Set up a DbContext and a UserStore
        var options = new DbContextOptionsBuilder<TestIdentityContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var dbContext = new TestIdentityContext(options);
        var userStore = new UserStore<TestUser>(dbContext);

        // Set up the UserManager
        var passwordHasher = new PasswordHasher<TestUser>(); // Add this line to create a password hasher
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        UserManager = new UserManager<TestUser>(userStore, null, passwordHasher, null, null, null, null, null, null); // Pass the password hasher to the UserManager constructor
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
