using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace OnlineStore.Extensions
{
    public static class CustomJWTAuthExtension
    {

        public static void AddCustomJWTAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(o =>
            {   // this three lines tell the .net core that you are using jwtbearer way to authentication
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => { // to handler the token by this method
                o.RequireHttpsMetadata = false; // using https or http
                o.SaveToken = true; // to save tokens and invoking it later throw context.user
                o.TokenValidationParameters = new TokenValidationParameters() // this is the core object to validate token components
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // this is maybe should be <=5 to combination between systems delay
                };
            });
        }


        public static void AddSwaggerGenJWTAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o => // o is a SwaggerGeneration object (composition object)
            {// open api responsible for providing json which is contained metadata(description) about our api
                o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "Open API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Api",
                        Email = "Api@gmail.com",
                        Url = new Uri("https://mydomain.com")
                    }
                });
                // this define an authorize way in openapi to swagger that names bearer and how to pass the token
                o.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {// definition
                    Name = "Authorization", // name of the header
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http, // type of sending , here http providing you to add bearer + ... token , so you can past the token only
                    Scheme = "Bearer",
                    BearerFormat = "JwT", // header.payload.signature
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,// send token through header
                    Description = "Enter the JWT key as: Bearer {Token}" 
                });
                // this tells " system (bearer) is required when we call an authorized endpoint like (category in our case)
                o.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference() // this relate requirement with definition (we named Bearer)
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>() // a empty value because we don't use scopes like Auth2
                    }
                });

            });
        }

    }
}
/*
 
     1️⃣ ما هو الـ OpenApiSecurityRequirement

    OpenApiSecurityRequirement في Swashbuckle (الذي يولّد Swagger) هو في الحقيقة:

    Dictionary<OpenApiSecurityScheme, IList<string>>


    يعني هو قاموس:

    الـ Key: كائن من نوع OpenApiSecurityScheme يصف نوع الحماية (Security Scheme).

    الـ Value: قائمة تحتوي على Scopes المطلوبة (عادةً تُستخدم مع OAuth2 أو OpenID Connect).




     🔄 كيف يتم الربط؟

    Swagger يولّد JSON/YAML خاص اسمه OpenAPI Specification.
    بعد إضافة التعريف + الربط، النتيجة تصبح شيء مثل:

    components:
      securitySchemes:
        Bearer:
          type: apiKey
          in: header
          name: Authorization
    security:
      - Bearer: []


    وهذه الـ security في الـ OpenAPI Specification تعني:

    "كل الـ endpoints (أو التي تحدد لها security يدويًا) لازم تستخدم نظام Bearer."

    عند تشغيل Swagger UI، هذا يظهر زر 🔒 في الأعلى، وتستطيع إدخال التوكن مرة واحدة، وسيتم إرساله تلقائيًا مع كل طلب.

    🧠 لماذا هي لامبدا ترجع void؟

    الـ AddSwaggerGen يقبل delegate (دالة) من نوع Action<SwaggerGenOptions>، ويمرر لك الـ Options كـ parameter (o).
    أنت تعدل في هذا الـ Options (تضيف تعريفات، متطلبات، إلخ).

    لاحظ:

    أنت لا ترجع شيء.

    مجرد تعدّل على الكائن الممرر (Mutate object).

    بعد أن تنتهي، مكتبة Swagger تجمع كل الـ Options وتستعملها لتوليد الـ Swagger JSON.

    يعني:

    o.AddSecurityDefinition(...) → تضيف تعريف.

    o.AddSecurityRequirement(...) → تضيف الربط.

    كل هذا يخزن داخليًا في SwaggerGenOptions.

    وفي النهاية، Swagger يقرأ هذه الخيارات ويولّد منها المواصفات تلقائيًا.
 */

