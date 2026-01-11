# 🚀 Quick Start Guide

## 5-Minute Setup

### 1. Prerequisites
```bash
# Verify .NET 9
dotnet --version

# Verify Git
git --version
```

### 2. Clone & Build
```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse

# Restore packages
dotnet restore

# Build
dotnet build -c Debug
```

### 3. Run
```bash
# Run from CLI
dotnet run -p src/SystemPulse.App/SystemPulse.App.csproj

# Or open in Visual Studio and press F5
```

---

## Project Structure Quick Reference

```
src/SystemPulse.App/
├── Views/              → XAML pages (*.xaml)
├── ViewModels/         → MVVM logic (*.cs)
├── Models/             → Data models
├── Services/           → Business logic
├── Helpers/            → Utilities
├── Resources/          → Colors, styles
├── MainWindow.xaml     → Main UI shell
└── App.xaml            → App config
```

---

## Common Tasks

### Create a New Page

1. **Create XAML file** in `Views/`:
   ```xml
   <Page x:Class="SystemPulse.App.Views.NewPage"
         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
       <Grid>
           <!-- Content -->
       </Grid>
   </Page>
   ```

2. **Create code-behind** (*.xaml.cs):
   ```csharp
   public sealed partial class NewPage : Page
   {
       public NewPage()
       {
           this.InitializeComponent();
       }
   }
   ```

3. **Create ViewModel** in `ViewModels/`:
   ```csharp
   public partial class NewViewModel : ObservableObject
   {
       // Properties and methods
   }
   ```

### Create a New Service

1. **Create interface** in `Services/`:
   ```csharp
   public interface IMyService
   {
       void DoSomething();
   }
   ```

2. **Create implementation**:
   ```csharp
   public class MyService : IMyService
   {
       public void DoSomething() { }
   }
   ```

### Add NuGet Package

```bash
# Search
dotnet package search package-name

# Add
dotnet add package package-name --version 1.0.0
```

---

## Debugging

### Visual Studio Debugging
1. Set breakpoint (Ctrl+B)
2. Press F5 to start debugging
3. Step through code (F10 = step over, F11 = step into)
4. Watch variables in Debug window

### Console Logging
```csharp
Debug.WriteLine($"CPU Usage: {cpu}%");
System.Diagnostics.Debugger.Break();  // Break into debugger
```

### Output Window
- View → Output (Ctrl+Alt+O)
- View → Debug (Ctrl+Alt+D)

---

## Performance Tips

1. **Use Release builds for testing**
   ```bash
   dotnet run -c Release
   ```

2. **Profile with diagnostics tools**
   - Debug → Performance Profiler (Alt+F2)

3. **Check memory with diagnostics**
   - Debug → Memory Diagnostics

---

## Common Issues

### "Windows App SDK not found"
```bash
dotnet add package Microsoft.WindowsAppSDK --version 1.8.0
```

### "XAML files not recognized"
- Close Visual Studio
- Delete `bin/` and `obj/` folders
- Reopen and rebuild

### Build fails
```bash
dotnet clean
dotnet restore
dotnet build
```

---

## Next Steps

1. Read [ARCHITECTURE.md](docs/ARCHITECTURE.md)
2. Check [SETUP.md](docs/SETUP.md) for detailed setup
3. Review [CONTRIBUTING.md](CONTRIBUTING.md)
4. Start with `good first issue` label on GitHub
5. Ask questions in GitHub Discussions

---

## Useful Commands

```bash
# Build only
dotnet build

# Build and run
dotnet run

# Run tests
dotnet test

# Clean artifacts
dotnet clean

# List packages
dotnet list package

# Check package updates
dotnet list package --outdated

# Restore packages
dotnet restore

# Publish release
dotnet publish -c Release
```

---

## Resources

- **[WinUI 3 Docs](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/)**
- **[.NET 9 Docs](https://learn.microsoft.com/en-us/dotnet/)**
- **[MVVM Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)**
- **[GitHub Issues](https://github.com/Gzeu/SystemPulse/issues)**

---

**Happy coding! 🚀**
