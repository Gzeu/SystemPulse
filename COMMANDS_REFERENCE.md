# 👋 Command Reference

## Initial Setup

### Clone Repository
```bash
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
```

### Verify Prerequisites
```bash
# Check .NET version (need 9.x)
dotnet --version

# Check Git
git --version

# Verify Visual Studio (optional, for GUI)
where devenv
```

### Restore & Build
```bash
# Restore NuGet packages
dotnet restore

# Build Debug configuration
dotnet build -c Debug

# Build Release configuration
dotnet build -c Release
```

---

## Running the Application

### From Command Line
```bash
# Run Debug build
dotnet run -p src/SystemPulse.App/SystemPulse.App.csproj

# Run with Release configuration
dotnet run -c Release -p src/SystemPulse.App/SystemPulse.App.csproj
```

### From Visual Studio
1. Open `SystemPulse.sln`
2. Select `SystemPulse.App` as startup project
3. Press **F5** (Debug) or **Ctrl+F5** (Release)

---

## Development Workflow

### Create Feature Branch
```bash
# Update develop branch
git checkout develop
git pull origin develop

# Create new feature branch
git checkout -b feature/add-gpu-monitoring
```

### Commit Changes
```bash
# Stage all changes
git add .

# Commit with conventional commit message
git commit -m "feat: Add real-time GPU monitoring to dashboard"

# Or stage selectively
git add src/SystemPulse.App/Services/GPUService.cs
git commit -m "feat: Implement GPU monitoring service"
```

### Push & Create Pull Request
```bash
# Push feature branch
git push origin feature/add-gpu-monitoring

# Create PR on GitHub (web interface)
# https://github.com/Gzeu/SystemPulse/compare
```

### Update from Main
```bash
# Fetch latest changes
git fetch origin

# Rebase feature branch on develop
git rebase origin/develop

# Or merge (if conflicts are complex)
git merge origin/develop

# Resolve conflicts if needed
git add .
git rebase --continue

# Push updated branch
git push origin feature/add-gpu-monitoring --force-with-lease
```

---

## Project Management

### Clean Build
```bash
# Remove build artifacts
dotnet clean

# Remove NuGet cache (if packages corrupted)
nuget locals all -clear

# Restore and rebuild
dotnet restore
dotnet build -c Release
```

### Run Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test src/SystemPulse.Tests/SystemPulse.Tests.csproj

# Run with verbosity
dotnet test --verbosity detailed

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

### Publish Release
```bash
# Publish to output directory
dotnet publish -c Release -o ./publish

# Publish as self-contained executable
dotnet publish -c Release -r win-x64 --self-contained -o ./publish
```

---

## NuGet Package Management

### Search Packages
```bash
# Search for package
dotnet package search linq-to-objects

# List installed packages
dotnet list package

# Check for updates
dotnet list package --outdated
```

### Add/Update Packages
```bash
# Add specific version
dotnet add package LiveChartsCore.SkiaSharpView.WinUI --version 2.1.0

# Add latest version
dotnet add package Serilog

# Update all packages
dotnet outdated

# Update specific package
dotnet package update Serilog
```

### Remove Packages
```bash
dotnet remove package UnusedPackage
```

---

## Git Workflow

### Check Status
```bash
# Status of working directory
git status

# See changes
git diff

# See staged changes
git diff --staged

# See commit history
git log --oneline

# See branch info
git branch -a
```

### Undo Changes
```bash
# Discard changes in working directory
git checkout -- src/file.cs

# Unstage file
git reset HEAD src/file.cs

# Undo last commit (keep changes)
git reset --soft HEAD~1

# Undo last commit (discard changes)
git reset --hard HEAD~1

# Revert specific commit
git revert abc123def
```

### Sync with Remote
```bash
# Fetch all branches
git fetch --all

# Pull latest changes
git pull origin main

# Pull with rebase (avoid merge commits)
git pull --rebase origin develop
```

---

## Visual Studio Debugging

### Breakpoints
```
F9              - Toggle breakpoint
Ctrl+Alt+B      - Breakpoints window
Ctrl+Shift+F9   - Delete all breakpoints
```

### Execution
```
F5              - Start debugging
Ctrl+F5         - Start without debugging
F10             - Step over
F11             - Step into
Shift+F11       - Step out
Ctrl+Alt+Break  - Break all
Shift+F5        - Stop debugging
```

### Windows
```
Ctrl+Alt+V, L   - Locals window
Ctrl+Alt+W, 1   - Watch 1 window
Ctrl+Alt+Q      - Quick watch
Ctrl+Alt+H      - Call stack
Ctrl+Alt+T      - Threads window
Ctrl+Alt+M      - Memory window
Ctrl+Alt+P      - Processes window
```

---

## Code Analysis & Formatting

### Static Analysis
```bash
# Run code analyzer
dotnet analyzers

# Run StyleCop
dotnet stylecop

# Run SonarQube (if configured)
dotnet sonarscanner begin /k:SystemPulse
dotnet build
dotnet sonarscanner end
```

### Format Code
```bash
# Format document (Visual Studio)
Ctrl+K, Ctrl+D

# Format selection
Ctrl+K, Ctrl+F

# Cleanup code
Ctrl+K, Ctrl+E

# Auto-format entire solution
dotnet format
```

---

## Performance Profiling

### Visual Studio Profiler
```
Debug > Performance Profiler (Alt+F2)
Debug > Attach to Process (Ctrl+Alt+P)
Debug > Memory Diagnostics
```

### Command Line Profiling
```bash
# Profile with dotnet-trace
dotnet-trace collect -- dotnet run

# View trace
dotnet-trace convert trace.nettrace

# Profile memory
dotnet-counters monitor
```

---

## Documentation & Help

### Project Documentation
```bash
# View README
type README.md

# View setup guide
type QUICKSTART.md

# View architecture
type docs/ARCHITECTURE.md

# View API reference
type docs/API_REFERENCE.md
```

### .NET Help
```bash
# .NET CLI help
dotnet --help

# Build command help
dotnet build --help

# Run command help
dotnet run --help
```

---

## Continuous Integration

### GitHub Actions
```bash
# View workflow status
# https://github.com/Gzeu/SystemPulse/actions

# View specific workflow
# https://github.com/Gzeu/SystemPulse/actions/workflows/build.yml

# View job logs
# Click on failed job in Actions tab
```

### Local CI Simulation
```bash
# Act (run GitHub Actions locally)
act -j build

# Act with specific event
act push
```

---

## Useful Shortcuts

### PowerShell/CMD
```bash
# Quick project setup
cd SystemPulse && dotnet restore && dotnet build

# Run with immediate feedback
watch -n 2 dotnet test

# Build and run
dotnet build -c Release && dotnet run -p src/SystemPulse.App/SystemPulse.App.csproj
```

### VS Code
```
Ctrl+Shift+`    - Open terminal
Ctrl+P          - Command palette
Ctrl+K Ctrl+W   - Close editor
Ctrl+K Ctrl+X   - Close all editors
Ctrl+B          - Toggle sidebar
Ctrl+J          - Toggle panel
```

---

## Troubleshooting

### Common Issues

**Build fails with missing SDK:**
```bash
dotnet --version  # Check installed version
dotnet nuget restore  # Try explicit restore
```

**NuGet package not found:**
```bash
nutget sources  # List sources
nuget locals all -clear  # Clear cache
dotnet restore  # Restore packages
```

**XAML designer not working:**
```bash
dotnet clean
rm -r bin obj
dotnet build
```

**Port already in use:**
```bash
netstat -ano | findstr :5000  # Find process
taskkill /PID 12345 /F  # Kill process
```

---

## Resources

- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [WinUI 3 Documentation](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/)
- [Git Documentation](https://git-scm.com/doc)
- [Visual Studio Documentation](https://learn.microsoft.com/en-us/visualstudio/)

---

## Quick Links

- **Repository**: https://github.com/Gzeu/SystemPulse
- **Clone**: `https://github.com/Gzeu/SystemPulse.git`
- **Issues**: https://github.com/Gzeu/SystemPulse/issues
- **Discussions**: https://github.com/Gzeu/SystemPulse/discussions

---

**Last Updated**: January 11, 2026
