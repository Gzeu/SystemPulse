# 🛠️ Development Setup Guide

This guide will help you set up your development environment to contribute to **SystemPulse**.

## Prerequisites

### System Requirements
- **OS**: Windows 10 (Build 19041+) or Windows 11/12
- **RAM**: Minimum 8GB (16GB recommended)
- **Disk Space**: 10GB free for development tools

### Required Software

1. **Visual Studio 2022** (v17.8 or later)
   - Download: [Visual Studio 2022 Community](https://visualstudio.microsoft.com/downloads/)
   - Install with these workloads:
     - ✅ Desktop & Mobile → Desktop development with C++
     - ✅ Desktop & Mobile → .NET desktop development
     - ✅ Windows → Universal Windows Platform development (for WinUI 3)
     - ✅ Desktop & Mobile → .NET Framework development tools

2. **.NET 9 SDK**
   - Download: [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
   - Verify installation:
     ```bash
     dotnet --version
     ```

3. **Windows App SDK 1.8.0+**
   - Install via NuGet (automatically handled by project)
   - Or manually: [Windows App SDK](https://github.com/microsoft/WindowsAppSDK)

4. **Git**
   - Download: [Git for Windows](https://git-scm.com/download/win)
   - Configure:
     ```bash
     git config --global user.name "Your Name"
     git config --global user.email "your.email@example.com"
     ```

### Optional Tools
- **GitHub Desktop** - For easier version control
- **Visual Studio Code** - For code editing and documentation
- **Windows SDK** - For advanced debugging

---

## Setup Instructions

### 1. Clone Repository

```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

Or in Visual Studio: **Build** → **Clean Solution** → **Rebuild Solution**

### 3. Build Project

**From Command Line:**
```bash
# Debug build
dotnet build -c Debug

# Release build
dotnet build -c Release
```

**From Visual Studio:**
1. Open `SystemPulse.sln`
2. Select configuration: **Debug** or **Release**
3. Build → Build Solution (Ctrl+Shift+B)

### 4. Run Application

**From Command Line:**
```bash
dotnet run -p src/SystemPulse.App/SystemPulse.App.csproj
```

**From Visual Studio:**
1. Set `SystemPulse.App` as startup project (right-click → Set as Startup Project)
2. Press F5 or click **Start Debugging**

---

## Project Structure

```
SystemPulse/
├── src/
│   ├── SystemPulse.App/
│   │   ├── Views/              # XAML pages
│   │   ├── ViewModels/         # MVVM ViewModels
│   │   ├── Models/             # Data models
│   │   ├── Services/           # Business logic
│   │   ├── Helpers/            # Utilities
│   │   ├── Resources/          # Styles, colors
│   │   ├── MainWindow.xaml     # Main UI
│   │   └── App.xaml            # Application settings
│   └── SystemPulse.Core/       # (Optional) Shared logic
├── docs/                       # Documentation
├── .github/workflows/          # CI/CD pipelines
└── README.md                   # Project overview
```

---

## Development Workflow

### Creating a Feature Branch

```bash
# Update main branch
git checkout main
git pull origin main

# Create feature branch
git checkout -b feature/your-feature-name
```

### Committing Changes

```bash
# Stage changes
git add .

# Commit with clear message
git commit -m "feat: Add feature description"

# Push to remote
git push origin feature/your-feature-name
```

### Creating Pull Request

1. Go to [GitHub Repository](https://github.com/Gzeu/SystemPulse)
2. Click **Compare & pull request**
3. Add title and description
4. Select reviewers
5. Click **Create pull request**

---

## Troubleshooting

### Issue: "Windows App SDK not found"
**Solution:**
```bash
dotnet package search WindowsAppSDK
dotnet add package Microsoft.WindowsAppSDK --version 1.8.0
```

### Issue: "XAML files not recognized"
**Solution:**
1. Close Visual Studio
2. Delete `bin` and `obj` folders
3. Reopen solution and rebuild

### Issue: Build fails with "Target Framework not supported"
**Solution:**
1. Ensure .NET 9 SDK is installed: `dotnet --version`
2. Update Visual Studio to latest version
3. Restart Visual Studio

### Issue: MSIX packaging errors
**Solution:**
```bash
# Clear cache and rebuild
dotnet clean
dotnet build -c Release --force
```

---

## Useful Commands

```bash
# List all projects
dotnet sln list

# Run tests
dotnet test

# Clean build artifacts
dotnet clean

# Publish release
dotnet publish -c Release -o ./publish

# Check dependencies
dotnet list package --outdated
```

---

## IDE Configuration

### Visual Studio Extensions (Recommended)
- **Fluent UI System Icons** - For WinUI icon integration
- **XAML Analyzer** - For XAML validation
- **GitHub Copilot** - For AI-assisted coding
- **Code Cleanup** - For code formatting

### Code Style
- **Language**: C# 12.0 (latest)
- **Null Safety**: Enabled (`<Nullable>enable</Nullable>`)
- **Format**: Follow [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

## Performance Tips

1. **Disable unnecessary extensions** in Visual Studio
2. **Use Fast Build** on startup
3. **Enable lightweight code analysis**
4. **Clear NuGet cache** if experiencing slow restores:
   ```bash
   nuget locals all -clear
   ```

---

## Next Steps

1. Review [ARCHITECTURE.md](ARCHITECTURE.md) for project design
2. Check [API_REFERENCE.md](API_REFERENCE.md) for available services
3. Read [Contributing Guidelines](../CONTRIBUTING.md)
4. Start with issues labeled `good first issue`

---

For questions, open an issue on [GitHub Issues](https://github.com/Gzeu/SystemPulse/issues)
