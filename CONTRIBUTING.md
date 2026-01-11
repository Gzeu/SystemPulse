# Contributing to SystemPulse

🌟 Thank you for your interest in contributing to **SystemPulse**! We welcome all types of contributions.

## Code of Conduct

This project adheres to the [Contributor Covenant](https://www.contributor-covenant.org/). By participating, you are expected to uphold this code.

---

## How to Contribute

### Reporting Bugs

1. **Check existing issues** to avoid duplicates
2. **Create a detailed bug report** including:
   - Windows version and build number
   - Application version
   - Steps to reproduce
   - Expected vs actual behavior
   - Screenshots/logs if applicable

### Requesting Features

1. **Describe the feature** clearly
2. **Explain the use case** and why it's valuable
3. **Provide examples** or mockups if helpful
4. **Check if similar features** are already requested

### Improving Documentation

Documentation improvements are always welcome!

- Fix typos or clarify explanations
- Add examples or tutorials
- Translate documentation
- Improve diagrams or screenshots

---

## Development Process

### 1. Fork and Clone

```bash
git clone https://github.com/YOUR-USERNAME/SystemPulse.git
cd SystemPulse
git remote add upstream https://github.com/Gzeu/SystemPulse.git
```

### 2. Create Feature Branch

```bash
git checkout -b feature/your-feature-name
```

**Branch naming conventions:**
- `feature/add-gpu-monitoring` - New features
- `fix/process-crash-bug` - Bug fixes
- `docs/update-api-docs` - Documentation
- `refactor/improve-performance` - Code improvements

### 3. Make Changes

- Follow code style guidelines (see below)
- Write clear, descriptive commit messages
- Keep commits atomic and logical

### 4. Commit with Clear Messages

```bash
git commit -am "feat: Add real-time GPU monitoring to dashboard"
```

Use conventional commits:
- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation
- `style:` - Code style (formatting)
- `refactor:` - Code refactoring
- `perf:` - Performance improvement
- `test:` - Tests
- `chore:` - Build, dependencies

### 5. Push and Create Pull Request

```bash
git push origin feature/your-feature-name
```

Then create a pull request with:
- **Clear title** describing the change
- **Description** explaining what and why
- **Related issues** (close #123)
- **Screenshots** for UI changes
- **Checklist** completion

---

## Code Style Guide

### C# Coding Standards

```csharp
// Use meaningful names
public class ProcessMonitorService { }

// Use naming conventions
private string _processName;  // private fields
public string ProcessName { get; set; }  // properties
private void OnProcessStarted() { }  // methods

// Use var for obvious types
var name = "SystemPulse";  // OK
var items = GetProcessList();  // OK
var result = dbContext.Processes.First();  // OK

// Use explicit types for clarity
List<ProcessInfo> processes = new();  // Better than var
Dictionary<int, ProcessInfo> cache = new();  // Better than var

// Use null-coalescing
string title = process?.Name ?? "Unknown";

// Use pattern matching
if (process is ProcessInfo { IsActive: true })
{
    // ...
}

// Use async/await
public async Task<List<ProcessInfo>> GetProcessesAsync()
{
    return await Task.FromResult(GetProcesses());
}
```

### XAML Standards

```xml
<!-- Use proper spacing and indentation -->
<Button 
    Content="Click Me"
    Click="OnButtonClick"
    Style="{StaticResource PrimaryButtonStyle}"/>

<!-- Use binding instead of code-behind when possible -->
<TextBlock Text="{Binding ProcessName}"/>

<!-- Use consistent naming for resources -->
x:Name="MainView"  <!-- PascalCase -->
Tag="main-container"  <!-- kebab-case -->
```

### Naming Conventions

| Type | Convention | Example |
|------|-----------|----------|
| Classes | PascalCase | `ProcessMonitorService` |
| Methods | PascalCase | `GetProcessInfo()` |
| Properties | PascalCase | `ProcessName` |
| Private fields | _camelCase | `_processList` |
| Constants | UPPER_CASE | `MAX_PROCESS_COUNT` |
| XAML files | PascalCase.xaml | `ProcessesPage.xaml` |
| View Models | PascalCase + ViewModel | `ProcessesViewModel` |

---

## Testing Requirements

### Writing Tests

```csharp
[TestClass]
public class ProcessServiceTests
{
    [TestMethod]
    public void GetProcesses_ReturnsValidList()
    {
        // Arrange
        var service = new ProcessMonitorService();

        // Act
        var result = service.GetProcesses();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Count > 0);
    }
}
```

### Before Submitting

- [ ] Run `dotnet test` locally
- [ ] No breaking tests
- [ ] New features include tests
- [ ] Code builds without warnings

---

## Pull Request Checklist

- [ ] Fork and branch created
- [ ] Changes are well-commented
- [ ] Code follows style guide
- [ ] Tests pass locally
- [ ] No new compiler warnings
- [ ] Commit messages are clear
- [ ] PR description is detailed
- [ ] Related issues are referenced
- [ ] Documentation updated (if needed)
- [ ] Screenshots added (for UI changes)

---

## Review Process

1. **Automated checks** run (build, tests, code analysis)
2. **Maintainers review** code quality and design
3. **Feedback** provided for improvements
4. **Changes requested** or approved
5. **Merge** once approved

### Tips for Review

- Be responsive to feedback
- Keep PR focused (one feature per PR)
- Smaller PRs are easier to review
- Respond to comments professionally

---

## Setting Up Your Environment

See [SETUP.md](docs/SETUP.md) for detailed instructions.

**Quick Start:**
```bash
# Clone and setup
git clone https://github.com/Gzeu/SystemPulse.git
cd SystemPulse
dotnet restore
dotnet build
```

---

## Community

- **GitHub Issues** - Report bugs and request features
- **GitHub Discussions** - Ask questions and discuss ideas
- **Email** - georgedev@example.com (maintainer)

---

## Recognition

Contributors will be:
- Listed in CONTRIBUTORS.md
- Credited in release notes
- Acknowledged in project README

---

Thank you for contributing! ❤️
