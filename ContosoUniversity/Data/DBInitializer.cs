using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Identity;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Person>>();

        // Apply pending migrations
        await context.Database.EnsureCreatedAsync();

        // ---------------------------
        // 1. Ensure Roles Exist
        // ---------------------------
        string[] roles = new[] { "Admin", "Instructor", "Student" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
        }

        // ---------------------------
        // 2. Create Admin User
        // ---------------------------
        string adminEmail = "kim.abercrombie@contoso.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new Instructor
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstMidName = "Kim",
                LastName = "Abercrombie",
                HireDate = DateTime.Parse("1995-03-11")
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            await userManager.AddToRoleAsync(adminUser, "Admin");

        // ---------------------------
        // 3. Seed Students
        // ---------------------------
        // 1. Seed roles first
        if (!await roleManager.RoleExistsAsync("Instructor"))
            await roleManager.CreateAsync(new IdentityRole<int>("Instructor"));
        if (!await roleManager.RoleExistsAsync("Student"))
            await roleManager.CreateAsync(new IdentityRole<int>("Student"));

        // 2. Seed students
        if (!context.Students.Any())
        {
            var students = new List<Student>
    {
        new Student { FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateOnly.Parse("2019-09-01") },
        new Student { FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateOnly.Parse("2017-09-01") },
    };

            foreach (var s in students)
            {
                s.UserName = $"{s.FirstMidName}.{s.LastName}".ToLower() + "@school.com";
                s.Email = $"{s.FirstMidName}.{s.LastName}".ToLower() + "@school.com";
                s.EmailConfirmed = true;

                var createResult = await userManager.CreateAsync(s, "Student123@");
                if (!createResult.Succeeded)
                    throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(s, "Student");
            }
        }


        // ---------------------------
        // 4. Seed Instructors
        // ---------------------------
        if (!context.Instructors.Any())
        {
            var instructors = new List<Instructor>
            {
                new Instructor { FirstMidName = "Kim", LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstMidName = "Fadi", LastName = "Fakhouri", HireDate = DateTime.Parse("2002-07-06") },
                // add more instructors here...
            };

            foreach (var i in instructors)
            {
                i.UserName = $"{i.FirstMidName}.{i.LastName}".ToLower();
                i.Email = $"{i.FirstMidName}.{i.LastName}".ToLower() + "@school.com";
                i.EmailConfirmed = true;

                var createResult = await userManager.CreateAsync(i, "Instructor123!");
                if (!createResult.Succeeded)
                    throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(i, "Instructor");
            }
        }

        // ---------------------------
        // 5. Seed Departments, Courses, Enrollments (Optional EF Only)
        // ---------------------------
        if (!context.Departments.Any())
        {
            var departments = new List<Department>
            {
                new Department { Name = "English", Budget = 350000, StartDate = DateTime.Parse("2007-09-01") },
                new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01") },
                // add more departments...
            };

            context.Departments.AddRange(departments);
            await context.SaveChangesAsync();
        }

        // You can continue adding Courses and Enrollments here...

        Console.WriteLine("Database seeding completed successfully!");
    }
}
