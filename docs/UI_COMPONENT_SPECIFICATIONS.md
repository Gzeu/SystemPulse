# UI Component Specifications

## Overview

Detailed specifications for all UI components in SystemPulse Phase 3.

---

## 1. Dashboard Gauges (OverviewPage)

### CPU Usage Gauge
```xaml
<Grid Background="#f0f0f0" CornerRadius="16" Padding="20">
    <TextBlock Text="CPU" FontSize="14" FontWeight="SemiBold" Foreground="#666"/>
    <Ring Percentage="{Binding CPUUsage}" 
          Color="#00D4FF" 
          Size="120"/>
    <TextBlock Text="{Binding CPUUsage, Converter={StaticResource PercentageConverter}}" 
               FontSize="32" 
               FontWeight="Bold" 
               Foreground="#00D4FF"/>
</Grid>
```

**Properties:**
- Value: 0-100%
- Color: #00D4FF (Cyan)
- Size: 120x120
- Update Interval: 1 second
- Animation: Smooth transition

### RAM Usage Gauge
- Color: #10B981 (Green)
- Shows: Used / Total GB
- Bar style with percentage

### GPU Usage Gauge
- Color: #7C3AED (Purple)
- Shows: Usage %
- Ring chart

### Network Activity
- Down/Up arrows with speed
- Color: #F59E0B (Orange)
- Display: Mbps

---

## 2. Process DataGrid (ProcessesPage)

### Column Definitions

| Column | Type | Width | Sortable | Filterable |
|--------|------|-------|----------|------------|
| Name | Text | 250px | Yes | Yes |
| PID | Number | 80px | Yes | No |
| CPU % | Percentage | 80px | Yes | No |
| Memory | Bytes | 100px | Yes | No |
| Threads | Number | 80px | Yes | No |
| User | Text | 150px | Yes | Yes |
| Status | Status | 100px | Yes | No |

### Features
- [ ] Virtual scrolling (1000+ rows)
- [ ] Multi-select with Ctrl+Click
- [ ] Right-click context menu
- [ ] Column resizing
- [ ] Sort ascending/descending
- [ ] Filter with multiple criteria
- [ ] Highlight system processes (gray)
- [ ] Row color by status

### Context Menu
```
Kill Process          (Ctrl+K)
Kill Process Tree     (Ctrl+Shift+K)
---
Suspend              (Ctrl+P)
Resume               (Ctrl+R)
---
Set Priority >
    Realtime
    High
    Normal
    Below Normal
    Low
---
Properties           (Alt+Enter)
Open File Location
```

---

## 3. Performance Charts (PerformancePage)

### Chart Layout
```
┌─────────────────────────────────────────┐
│ CPU Usage (Last 5 Minutes)              │
│                                         │
│ ╱╲  ╱╲                                  │
│╱  ╲╱  ╲  Max: 85% Avg: 45%             │
│                                         │
└─────────────────────────────────────────┘
```

### Chart Specifications

- **Type**: Line chart
- **Data Points**: 300 (5 minutes @ 1 second)
- **X-Axis**: Time (minutes:seconds)
- **Y-Axis**: Percentage (0-100%)
- **Colors**: 
  - CPU: #00D4FF
  - RAM: #10B981
  - GPU: #7C3AED
  - Disk: #F59E0B
  - Network: #FF6B6B
- **Grid**: Light gray lines
- **Animation**: Smooth scrolling

### Statistics Panel
```
CPU Usage Statistics
┌─────────────┬────────────┐
│ Current     │ 45%        │
│ Minimum     │ 2%         │
│ Maximum     │ 89%        │
│ Average     │ 32%        │
└─────────────┴────────────┘
```

### Time Range Selector
- 1 minute (60 points)
- 5 minutes (300 points)
- 15 minutes (900 points)
- 30 minutes (1800 points)
- 1 hour (3600 points)

---

## 4. Settings Panel (SettingsPage)

### Layout
```
Settings

🎨 Appearance
  Theme              [Light ▼]      (Light / Dark / System)
  Window Opacity     [=====●====]   (50% - 100%)
  Always on Top      [Toggle]

⚙️ Behavior
  Refresh Interval   [=====●=====]  (1s - 60s)
  Auto-start         [Toggle]
  Minimize to Tray   [Toggle]
  Show Notifications [Toggle]

📊 Advanced
  Log Level          [Debug ▼]      (Debug / Info / Warning / Error)
  Enable Logging     [Toggle]
  Export Logs        [Button]

                    [Save] [Reset to Defaults]
```

### Controls

**Theme Selector**
```xaml
<ComboBox ItemsSource="{Binding ThemeOptions}" 
          SelectedItem="{Binding SelectedTheme, Mode=TwoWay}" />
```

**Refresh Interval Slider**
```xaml
<Slider Minimum="1" Maximum="60" 
        Value="{Binding RefreshInterval, Mode=TwoWay}" 
        StepFrequency="1" />
<TextBlock Text="{Binding RefreshInterval}s" />
```

**Toggle Switches**
```xaml
<ToggleSwitch IsOn="{Binding AlwaysOnTop, Mode=TwoWay}" 
              Header="Always on Top" />
```

---

## 5. Services DataGrid (ServicesPage)

### Column Definitions

| Column | Width | Sortable |
|--------|-------|----------|
| Name | 200px | Yes |
| Display Name | 250px | Yes |
| Status | 100px | Yes |
| Startup Type | 100px | Yes |
| Description | 300px | No |

### Status Colors
- Running: #10B981 (Green)
- Stopped: #6B7280 (Gray)
- Pending: #F59E0B (Orange)
- Error: #EF4444 (Red)

### Action Buttons
- Start (enabled when Stopped)
- Stop (enabled when Running)
- Restart (enabled when Running)
- Startup Type dropdown

---

## 6. Startup Apps (StartupPage)

### Layout
```
Startup Applications (Filter: [All ▼])

[Search box]

┌─────────────────────────────────────────────┐
│ [☑] Application Name          Delay: 5s    │
│ [☑] Another App              Delay: 10s    │
│ [☑] Third App                Delay: 0s     │
└─────────────────────────────────────────────┘

[Remove from Startup]  [Refresh]  [Enable All]  [Disable All]
```

### Features
- [ ] Toggle enable/disable per app
- [ ] Show delay time
- [ ] Filter by source (Windows/User/Manufacturer)
- [ ] Manufacturer logo icons
- [ ] Remove from startup option
- [ ] Reorder via drag-and-drop

---

## 7. Active Users (UsersPage)

### Layout
```
Active User Sessions

┌──────────────────────────────────────────────────┐
│ Username    │ Session │ Logon Time │ Idle Time  │
├──────────────────────────────────────────────────┤
│ admin       │ Console │ 08:30 AM   │ 5 min      │
│ user1       │ RDP     │ 14:15 PM   │ 2 min      │
│ user2       │ RDP     │ 19:00 PM   │ Inactive   │
└──────────────────────────────────────────────────┘

[Send Message] [Logoff User] [Disconnect] [Refresh]
```

---

## 8. Process Details (DetailsPage)

### Layout
```
Process Details: explorer.exe (PID: 1234)

📋 General Information
  Name:          explorer.exe
  PID:           1234
  Full Path:     C:\Windows\explorer.exe
  Status:        Running
  Priority:      Normal
  Threads:       45

📊 Memory
  Working Set:   125 MB
  Committed:     89 MB
  Peak:          156 MB

⚡ Performance
  [CPU Usage Graph - 5 min]
  Current: 5% | Avg: 3% | Peak: 12%
  
  [Memory Graph - 5 min]
  Current: 125 MB | Avg: 110 MB | Peak: 156 MB

📂 Open Files
  [List of open files and handles]

🔧 Environment
  [Key-value pairs of environment variables]

[Close] [Kill] [Properties]
```

---

## 9. About Page

### Layout
```
┌─────────────────────────────────────┐
│                                     │
│          SystemPulse Logo           │
│                                     │
│        SystemPulse v0.2.0           │
│                                     │
│  Advanced System Monitor for        │
│  Windows 10 / 11 / 12               │
│                                     │
├─────────────────────────────────────┤
│                                     │
│  Build: 2026.01.20                  │
│  Framework: .NET 8.0                │
│  License: MIT                       │
│                                     │
│  [GitHub] [Check for Updates] [OK]  │
│                                     │
└─────────────────────────────────────┘
```

---

## Color Palette

### Primary Colors
```
#00D4FF - Cyan (CPU)
#10B981 - Green (Memory/Success)
#7C3AED - Purple (GPU)
#F59E0B - Orange (Warning/Disk)
#EF4444 - Red (Error)
#6366F1 - Indigo (Primary Action)
```

### Neutral Colors
```
#FFFFFF - White (Background)
#F9FAFB - Light Gray (Secondary Background)
#E5E7EB - Light Border
#6B7280 - Medium Gray (Disabled)
#374151 - Dark Gray (Text Secondary)
#111827 - Near Black (Text Primary)
```

---

## Typography

### Font Family
- Primary: Segoe UI (System font)
- Monospace: Consolas (Code/Numbers)

### Font Sizes
```
Title:      32px (SemiBold)
Heading:    24px (SemiBold)
Subheading: 18px (Medium)
Body:       14px (Regular)
Small:      12px (Regular)
Caption:    11px (Regular)
```

---

## Spacing & Layout

### Padding
- Container: 20px
- Section: 16px
- Element: 8px
- Compact: 4px

### Corner Radius
- Large: 16px
- Medium: 8px
- Small: 4px

---

## Animations

### Transitions
- Default: 250ms cubic-bezier(0.16, 1, 0.3, 1)
- Fast: 150ms
- Slow: 500ms

### Effects
- Gauge sweep: Smooth
- Chart updates: Easing
- Transitions: Fade
- Hover: Subtle scale (1.02)

---

**Last Updated**: January 11, 2026  
**Version**: 1.0
