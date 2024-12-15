# Blazor POS System

A modern Point of Sale (POS) system built with Blazor WebAssembly, featuring real-time inventory management, sales tracking, and responsive design.

## Features

- 💻 Intuitive POS interface
- 🔍 Real-time product search
- 🛒 Shopping cart management
- 💰 Multi-payment method support
- 📊 Sales reporting and analytics
- 📦 Inventory management
- 👥 User authentication and role-based access
- 🖨️ Receipt printing

## Technology Stack

- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core 8
- **Database**: SQL Server
- **Authentication**: ASP.NET Core Identity
- **UI Framework**: Bootstrap 5
- **Icons**: Font Awesome

## Project Structure

```
BlazorPOS/
├── src/
│   ├── BlazorPOS.Client/           # Blazor WebAssembly Client
│   ├── BlazorPOS.Server/           # ASP.NET Core Backend
│   ├── BlazorPOS.Shared/           # Shared Models and Interfaces
│   └── BlazorPOS.Infrastructure/   # Data Access and Services
└── tests/
    ├── BlazorPOS.Client.Tests/
    ├── BlazorPOS.Server.Tests/
    └── BlazorPOS.Infrastructure.Tests/
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/ZubeidHendricks/blazor-pos-system.git
   ```

2. Navigate to the project directory
   ```bash
   cd blazor-pos-system
   ```

3. Create and update the database
   ```bash
   dotnet ef database update
   ```

4. Run the application
   ```bash
   dotnet run --project src/BlazorPOS.Server
   ```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
