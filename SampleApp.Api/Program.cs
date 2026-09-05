using EF.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SampleApp.Api.Helper;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers()
       .AddXmlSerializerFormatters()  // Add support for XML format for content ContentNegotiationController
       .AddJsonOptions(options => {options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;});

//builder.Services.AddDbContext<DepartmentContext>(option =>option.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

string message = "";
/*builder.Services.AddAuthentication(sharedoptions =>
{
    sharedoptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.UseSecurityTokenValidators = true; //ADD THIS
    options.Authority = "https://login.microsoftonline.com/23babaf1-84a8-4072-887f-b7abccd693fe.onmicrosoft.com";
    // if you intend to validate only one audience for the access token, you can use options.Audience instead of
    // using options.TokenValidationParameters which allow for more customization.
    // options.Audience = "10e569bc5-4c43-419e-971b-7c37112adf691";

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidAudiences = new List<string> { "00000003-0000-0000-c000-000000000000","f6989dd5-cffa-4e30-8c43-dc1e7a3d78eb", "https://sts.windows.net/23babaf1-84a8-4072-887f-b7abccd693fe/" },
        ValidIssuers = new List<string> { "https://sts.windows.net/23babaf1-84a8-4072-887f-b7abccd693fe/", "https://login.microsoftonline.com/23babaf1-84a8-4072-887f-b7abccd693fe/oauth2/v2.0" },
        ValidateIssuerSigningKey = false,
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("z1rsYHHJ9-8mggt4HsZu8BKkBPw")),
        ValidateLifetime=false 
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            message += "From OnAuthenticationFailed:\n";
            //message += FlattenException(ctx.Exception);
            return Task.CompletedTask;
        },

        OnChallenge = ctx =>
        {
            message += "From OnChallenge:\n";
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ctx.Response.ContentType = "text/plain";
            return ctx.Response.WriteAsync(message);

        },

        OnMessageReceived = ctx =>
        {
            message = "From OnMessageReceived:\n";
            ctx.Request.Headers.TryGetValue("Authorization", out var BearerToken);
            if (BearerToken.Count == 0)
                BearerToken = "no Bearer token sent\n";
            message += "Authorization Header sent: " + BearerToken + "\n";
            return Task.CompletedTask;
        },

        OnTokenValidated = ctx =>
        {
            Console.WriteLine("token: " + ctx.SecurityToken.ToString());
            return Task.CompletedTask;
        }
    };
});*/


builder.Services.AddRateLimiter(options =>
{
    // 1. Fixed Window Limiter (20 requests per 2 minutes)
    // -----------------------------------------------
    // |----------------- 2 min -----------------|
    // |              Blocked if limit hit       |
    // -----------------------------------------------
    // Requests reset fully after 2 minutes, blocking all requests
    // until the next window starts.
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(2);
        opt.PermitLimit = 20;
    });

    // 2. Sliding Window Limiter (20 requests per 2 minutes, 4 segments)
    // ---------------------------------------------------------
    // |--30s--|--30s--|--30s--|--30s--| (4 segments in 2 min)
    // |       |       |       |       | <- Requests slide as segments roll over
    // ---------------------------------------------------------
    // As new 30-second segments open, the oldest one expires, allowing
    // smoother traffic over time.
    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(2);
        opt.PermitLimit = 20;
        opt.SegmentsPerWindow = 4;  // Divides into 4 rolling segments (every 30 seconds)
    });

    // 3. Token Bucket Limiter (15 tokens max, refill 1 token/sec)
    // ---------------------------------------------------------
    // Tokens:  15 (Max) - Burst Capacity
    // Refills: 1 token per second (smoother flow)
    // ---------------------------------------------------------
    // Allows bursts but gradually refills tokens over time.
    // Requests consume tokens, and when depleted, requests are throttled until refilled.
    options.AddTokenBucketLimiter("token", opt =>
    {
        opt.TokenLimit = 15;
        opt.TokensPerPeriod = 1;  // Refill rate
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(1);  // Refill interval
    });

    // 4. Concurrency Limiter (5 concurrent requests, 10 in queue)
    // ---------------------------------------------------------
    // Concurrent: | | | | | (5 slots)
    // Queue:      | | | | | | | | | | (10 slots)
    // ---------------------------------------------------------
    // Allows 5 active requests and queues 10 more. If the queue is full,
    // new requests are rejected with 503 or 429 (if overridden).
    options.AddConcurrencyLimiter("concurrency", opt =>
    {
        opt.PermitLimit = 5;   // Max concurrent requests
        opt.QueueLimit = 10;   // Queue limit before rejection
    });
});

var app = builder.Build();


/*
 app.UseRateLimiter();

// Map endpoints with rate limiting policies
app.MapGet("/fixed", () => "Fixed window response").RequireRateLimiting("fixed");
app.MapGet("/sliding", () => "Sliding window response").RequireRateLimiting("sliding");
app.MapGet("/token", () => "Token bucket response").RequireRateLimiting("token");
app.MapGet("/concurrency", () => "Concurrency response").RequireRateLimiting("concurrency");
*/
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
