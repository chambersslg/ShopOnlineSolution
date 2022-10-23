https://github.com/GavinLonDigital/ShopOnlineSolution/blob/main/ShopOnline.Api/Data/ShopOnlineDbContext.cs

dotnet add package Microsoft.EntityFrameworkCore.Sqlite 
dotnet add package Microsoft.EntityFrameworkCore.Tools

Setup optionsBuilder in Context
Setup OnModelCreating to seed database

Install dotnet ef tool 
dotnet tool update --global dotnet-ef

Install the designer 
dotnet add package Microsoft.EntityFrameworkCore.Design

Add database migrations
dotnet ef migrations add initial

Update database 
dotnet ef database update


To run both Api and Web configure using SetStatusProject rightclick menu on Solution

Cors blocks access to API project by default.
AddCors to API Project

Add FontAwesome using CDN found: https://cdnjs.com/libraries/font-awesome
Then paste into index.html file
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css" integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g==" crossorigin="anonymous" referrerpolicy="no-referrer" />


Paypal integration
https://www.paypal.com/merchantapps/appcenter/acceptpayments/checkout

Create App in Paypal
https://developer.paypal.com/developer/applications/create

Copy client id and paste into index.html

Mock Credit Card Details:
Card Type: Visa

Card Number: 4032035822862470

Expiration Date: 01/2024

CVV: 869

Local Storage Add Nuget Blazored.LocalStorage