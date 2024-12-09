# Song BookStore

A modern, full-featured e-commerce bookstore built with ASP.NET Core MVC and Entity Framework Core.

## 🌟 Features

- **User Authentication & Authorization**
  - Identity Framework integration
  - Role-based access control
  - External authentication (Microsoft)
  - User management system

- **Product Management**
  - Book catalog with detailed information
  - Multiple product images support
  - Category management
  - Dynamic pricing system

- **Shopping Experience**
  - Shopping cart functionality
  - Order processing system
  - Secure checkout process
  - Order history and tracking

- **Admin Dashboard**
  - Sales analytics
  - Order management
  - User role management
  - Product inventory control

- **Company Management**
  - B2B functionality
  - Company profiles
  - Business customer management

## 🚀 Technology Stack

- **Backend**
  - ASP.NET Core MVC (.NET 7.0)
  - Entity Framework Core
  - PostgreSQL Database
  - Identity Framework

- **Frontend**
  - Bootstrap 5
  - jQuery
  - JavaScript
  - Razor Views

- **Payment Processing**
  - Stripe Integration

- **Email Services**
  - SendGrid Integration

## 📋 Prerequisites

- .NET 7.0 SDK or later
- PostgreSQL 13.0 or later
- Node.js (for frontend package management)
- Visual Studio 2022 or VS Code

## ⚙️ Configuration

1. **Database Configuration**
   - Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your_PostgreSQL_Connection_String"
     }
   }
   ```

2. **External Services**
   - Configure Stripe in `appsettings.json`:
   ```json
   {
     "Stripe": {
       "PublishableKey": "Your_Stripe_Publishable_Key",
       "SecretKey": "Your_Stripe_Secret_Key"
     }
   }
   ```
   - Configure SendGrid for email services
   - Set up Microsoft Authentication credentials

## 🛠️ Installation

1. Clone the repository
   ```bash
   git clone https://github.com/songyang8964/Song_BookStore.git
   ```

2. Navigate to the project directory
   ```bash
   cd Song_BookStore
   ```

3. Restore dependencies
   ```bash
   dotnet restore
   ```

4. Update database
   ```bash
   dotnet ef database update
   ```

5. Run the application
   ```bash
   dotnet run
   ```

## 🏗️ Project Structure

```
Song_BookStore/
├── SongWeb/                 # Main web application
├── Song.DataAccess/        # Data access layer
├── Song.Models/            # Domain models
└── Song.Utility/           # Shared utilities
```

## 🔒 Security

- Sensitive information is stored in user secrets or environment variables
- HTTPS enforcement
- Cross-site scripting (XSS) protection
- SQL injection prevention
- CSRF protection

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a pull request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Song Yang** - *Initial work* - [songyang8964](https://github.com/songyang8964)

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Bootstrap team for the frontend framework
- All contributors who have helped with the project
