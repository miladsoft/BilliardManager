﻿using Microsoft.AspNetCore.Identity;

namespace Billiard.Services.Contracts.Identity;

public interface IIdentityDbInitializer
{
    /// <summary>
    /// Applies any pending migrations for the context to the database.
    /// Will create the database if it does not already exist.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Adds some default values to the IdentityDb
    /// </summary>
    void SeedData();

    Task<IdentityResult> SeedDatabaseWithAdminUserAsync();
}