using Library.Data;
using Library.Models;
using Library.Repositories;
using Library.Repositories.Interfaces;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<IBookshelfRepository, BookshelfRepository>();
builder.Services.AddScoped<IBookshelfService, BookshelfService>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IPublisherService, PublisherService>();

builder.Services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
builder.Services.AddScoped<IBookAuthorService, BookAuthorService>();

builder.Services.AddScoped<IReservedBookRepository, ReservedBookRepository>();
builder.Services.AddScoped<IReservedBookService, ReservedBookService>();

builder.Services.AddScoped<IReturnRequestRepository, ReturnRequestRepository>();
builder.Services.AddScoped<IReturnRequestService, ReturnRequestService>();

builder.Services.AddScoped<IContactFormRepository, ContactFormRepository>();
builder.Services.AddScoped<IContactFormService, ContactFormService>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<UserManager<User>>();


builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<LibraryContext>();
builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDB")));


builder.Services.Configure<IdentityOptions>(options =>
{
   options.Password.RequireDigit = true;
   options.Password.RequiredLength = 8;
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequireUppercase = true;
   options.Password.RequireLowercase = false;
   options.Password.RequiredUniqueChars = 6;

   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
   options.Lockout.MaxFailedAccessAttempts = 10;
   options.Lockout.AllowedForNewUsers = true;

   options.User.RequireUniqueEmail = true;
   options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

   options.SignIn.RequireConfirmedPhoneNumber = false;
   options.Stores.MaxLengthForKeys = 128;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	 name: "default",
	 pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

await SeedRoles(app);

app.Run();



async Task SeedRoles(IHost app)
{
   using (var scope = app.Services.CreateScope())
   {
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      await ContextSeed.SeedRolesAsync(roleManager);
   }
}
