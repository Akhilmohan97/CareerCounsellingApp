# ?? PDF Export Feature - Complete Implementation Instructions

## ?? IMPORTANT: Fix Build Error First

You have empty XAML files causing build failures. Follow these steps:

---

## ? Step 1: Delete Empty XAML Files (CRITICAL)

**In Visual Studio or File Explorer, DELETE these files:**

```
CareerCounsellingApp\Views\ReportsWindow.axaml (empty)
CareerCounsellingApp\Views\ReportsWindowTemp.axaml (empty)
CareerCounsellingApp\Views\Reports Window.axaml (empty - note the space)
```

**Method 1: Visual Studio**
1. Solution Explorer ? Views folder
2. Right-click each file above
3. Delete ? Delete

**Method 2: Command Line**
```powershell
cd F:\CareerCounsellingApp\CareerCounsellingApp\Views
rm "ReportsWindow.axaml" 2>$null
rm "ReportsWindowTemp.axaml" 2>$null
rm "Reports Window.axaml" 2>$null
```

---

## ? Step 2: Copy New XAML File Content

The complete XAML content is here:
`Documentation\PDF_EXPORT_IMPLEMENTATION.md` (Copy the XAML section)

Or use this location:
`Views\ReportsWindow.axaml.new`

**Create file:** `CareerCounsellingApp\Views\ReportsWindow.axaml`

**Paste this XAML content:**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Window xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:vm="using:CareerCounsellingApp.ViewModels"
    mc:Ignorable="d" d:DesignWidth="1200" d:DesignHeight="900"
    x:Class="CareerCounsellingApp.ReportsWindow"
    x:DataType="vm:ReportsViewModel"
    WindowState="Maximized"
    Title="Reports Dashboard"
    Background="{StaticResource BackgroundBrush}">

    <Grid RowDefinitions="Auto,*">
        
        <!-- Header -->
        <Border Grid.Row="0" Background="#2563EB" Padding="35,25">
            <Grid ColumnDefinitions="*,Auto" ColumnSpacing="20">
                <StackPanel>
                    <TextBlock Text="Reports Dashboard" FontSize="32" FontWeight="Bold" Foreground="White"/>
                    <TextBlock Text="Comprehensive assessment analytics and insights" FontSize="15" Foreground="#DCEBFF" Margin="0,6,0,0"/>
                </StackPanel>
                
                <!-- Export Button -->
                <Button Grid.Column="1" 
                        Command="{Binding ExportPdfCommand}"
                        IsEnabled="{Binding !IsExporting}"
                        Height="45"
                        Padding="20,0"
                        CornerRadius="8"
                        Background="White"
                        Foreground="#2563EB"
                        VerticalAlignment="Center">
                    <StackPanel Orientation="Horizontal" Spacing="8">
                        <TextBlock Text="??"/>
                        <TextBlock Text="Export PDF" FontWeight="SemiBold"/>
                    </StackPanel>
                </Button>
            </Grid>
        </Border>

        <!-- Main Content -->
        <ScrollViewer Grid.Row="1">
            <StackPanel Padding="35,30" Spacing="30">
                
                <!-- Export Status Message -->
                <StackPanel IsVisible="{Binding ExportMessage, Converter={x:Static StringConverters.IsNotNullOrEmpty}}">
                    <Border Background="#EFF6FF" 
                            BorderBrush="#2563EB" 
                            BorderThickness="1" 
                            CornerRadius="8" 
                            Padding="15">
                        <TextBlock Text="{Binding ExportMessage}" 
                                   FontSize="13" 
                                   Foreground="#1E40AF"
                                   TextWrapping="Wrap"/>
                    </Border>
                </StackPanel>

                <!-- Summary Cards -->
                <Grid ColumnDefinitions="*,*,*,*" ColumnSpacing="20">
                    
                    <!-- Total Assessments -->
                    <Border Background="White" CornerRadius="12" Padding="20" BoxShadow="0 2 8 #00000010">
                        <StackPanel Spacing="12">
                            <TextBlock Text="??" FontSize="24"/>
                            <TextBlock Text="Total Assessments" FontSize="13" Foreground="Gray" FontWeight="SemiBold"/>
                            <TextBlock Text="{Binding TotalAssessments}" FontSize="32" FontWeight="Bold" Foreground="#2563EB"/>
                        </StackPanel>
                    </Border>

                    <!-- Average Score -->
                    <Border Grid.Column="1" Background="White" CornerRadius="12" Padding="20" BoxShadow="0 2 8 #00000010">
                        <StackPanel Spacing="12">
                            <TextBlock Text="??" FontSize="24"/>
                            <TextBlock Text="Average Score" FontSize="13" Foreground="Gray" FontWeight="SemiBold"/>
                            <TextBlock Text="{Binding AverageScore, StringFormat='{}{0}%'}" FontSize="32" FontWeight="Bold" Foreground="#F59E0B"/>
                        </StackPanel>
                    </Border>

                    <!-- High Performers -->
                    <Border Grid.Column="2" Background="White" CornerRadius="12" Padding="20" BoxShadow="0 2 8 #00000010">
                        <StackPanel Spacing="12">
                            <TextBlock Text="?" FontSize="24"/>
                            <TextBlock Text="High Performers" FontSize="13" Foreground="Gray" FontWeight="SemiBold"/>
                            <TextBlock Text="{Binding HighPerformers}" FontSize="32" FontWeight="Bold" Foreground="#10B981"/>
                        </StackPanel>
                    </Border>

                    <!-- Need Support -->
                    <Border Grid.Column="3" Background="White" CornerRadius="12" Padding="20" BoxShadow="0 2 8 #00000010">
                        <StackPanel Spacing="12">
                            <TextBlock Text="??" FontSize="24"/>
                            <TextBlock Text="Need Support" FontSize="13" Foreground="Gray" FontWeight="SemiBold"/>
                            <TextBlock Text="{Binding StudentsNeedingSupport}" FontSize="32" FontWeight="Bold" Foreground="#EF4444"/>
                        </StackPanel>
                    </Border>
                </Grid>

                <!-- Band Distribution -->
                <Border Background="White" CornerRadius="12" Padding="25" BoxShadow="0 2 8 #00000010">
                    <StackPanel Spacing="15">
                        <TextBlock Text="Performance Distribution" FontSize="20" FontWeight="Bold"/>
                        <ScrollViewer MaxHeight="300" VerticalScrollBarVisibility="Auto">
                            <ItemsControl ItemsSource="{Binding BandDistribution}">
                                <ItemsControl.ItemTemplate>
                                    <DataTemplate>
                                        <Grid ColumnDefinitions="120,*,80,80" ColumnSpacing="20" Margin="0,8">
                                            <TextBlock Text="{Binding Band}" FontWeight="SemiBold" VerticalAlignment="Center"/>
                                            <ProgressBar Grid.Column="1" Value="{Binding Percentage}" Maximum="100" Height="24" CornerRadius="12" Foreground="#2563EB" Background="#E5E7EB" VerticalAlignment="Center"/>
                                            <TextBlock Grid.Column="2" Text="{Binding Count}" FontWeight="Bold" Foreground="#2563EB" VerticalAlignment="Center" HorizontalAlignment="Center"/>
                                            <TextBlock Grid.Column="3" Text="{Binding Percentage, StringFormat='{}{0}%'}" FontWeight="Bold" Foreground="Gray" VerticalAlignment="Center" HorizontalAlignment="Right"/>
                                        </Grid>
                                    </DataTemplate>
                                </ItemsControl.ItemTemplate>
                            </ItemsControl>
                        </ScrollViewer>
                    </StackPanel>
                </Border>

                <!-- Category Performance -->
                <Border Background="White" CornerRadius="12" Padding="25" BoxShadow="0 2 8 #00000010">
                    <StackPanel Spacing="15">
                        <TextBlock Text="Category Performance" FontSize="20" FontWeight="Bold"/>
                        <ScrollViewer MaxHeight="400" VerticalScrollBarVisibility="Auto">
                            <ItemsControl ItemsSource="{Binding CategoryPerformance}">
                                <ItemsControl.ItemTemplate>
                                    <DataTemplate>
                                        <Border BorderBrush="#E5E7EB" BorderThickness="0,0,0,1" Padding="0,15">
                                            <Grid ColumnDefinitions="*,120,120,120,120" ColumnSpacing="15">
                                                <StackPanel>
                                                    <TextBlock Text="{Binding CategoryName}" FontSize="14" FontWeight="SemiBold"/>
                                                    <TextBlock Text="{Binding TotalAttempts, StringFormat='{}{0} attempts'}" FontSize="12" Foreground="Gray" Margin="0,4,0,0"/>
                                                </StackPanel>
                                                <StackPanel Grid.Column="1" VerticalAlignment="Center">
                                                    <TextBlock Text="Average" FontSize="11" Foreground="Gray" HorizontalAlignment="Center"/>
                                                    <TextBlock Text="{Binding AverageScore, StringFormat='{}{0}%'}" FontSize="16" FontWeight="Bold" Foreground="#2563EB" HorizontalAlignment="Center"/>
                                                </StackPanel>
                                                <StackPanel Grid.Column="2" VerticalAlignment="Center">
                                                    <TextBlock Text="Highest" FontSize="11" Foreground="Gray" HorizontalAlignment="Center"/>
                                                    <TextBlock Text="{Binding HighestScore, StringFormat='{}{0}%'}" FontSize="16" FontWeight="Bold" Foreground="#10B981" HorizontalAlignment="Center"/>
                                                </StackPanel>
                                                <StackPanel Grid.Column="3" VerticalAlignment="Center">
                                                    <TextBlock Text="Lowest" FontSize="11" Foreground="Gray" HorizontalAlignment="Center"/>
                                                    <TextBlock Text="{Binding LowestScore, StringFormat='{}{0}%'}" FontSize="16" FontWeight="Bold" Foreground="#EF4444" HorizontalAlignment="Center"/>
                                                </StackPanel>
                                                <StackPanel Grid.Column="4" VerticalAlignment="Center" HorizontalAlignment="Right">
                                                    <TextBlock Text="?" FontSize="14" FontWeight="Bold" Foreground="#10B981"/>
                                                </StackPanel>
                                            </Grid>
                                        </Border>
                                    </DataTemplate>
                                </ItemsControl.ItemTemplate>
                            </ItemsControl>
                        </ScrollViewer>
                    </StackPanel>
                </Border>

                <!-- Student Performance -->
                <Border Background="White" CornerRadius="12" Padding="25" BoxShadow="0 2 8 #00000010">
                    <StackPanel Spacing="15">
                        <Grid ColumnDefinitions="*,Auto">
                            <TextBlock Text="Student Performance" FontSize="20" FontWeight="Bold"/>
                            <ComboBox Grid.Column="1" ItemsSource="{Binding FilterOptions}" SelectedItem="{Binding SelectedFilter}" Width="180" Height="36" VerticalAlignment="Center"/>
                        </Grid>
                        <ScrollViewer MaxHeight="500" VerticalScrollBarVisibility="Auto">
                            <ItemsControl ItemsSource="{Binding StudentPerformance}">
                                <ItemsControl.ItemTemplate>
                                    <DataTemplate>
                                        <Border BorderBrush="#E5E7EB" BorderThickness="0,0,0,1" Padding="0,12">
                                            <Grid ColumnDefinitions="*,100,100,80,60" ColumnSpacing="15">
                                                <StackPanel>
                                                    <TextBlock Text="{Binding StudentName}" FontSize="14" FontWeight="SemiBold"/>
                                                    <Grid ColumnDefinitions="Auto,*" ColumnSpacing="8" Margin="0,4,0,0">
                                                        <TextBlock Text="Adm:" FontSize="11" Foreground="Gray"/>
                                                        <TextBlock Grid.Column="1" Text="{Binding AdmissionNo}" FontSize="11"/>
                                                    </Grid>
                                                </StackPanel>
                                                <StackPanel Grid.Column="1" VerticalAlignment="Center">
                                                    <TextBlock Text="Score" FontSize="11" Foreground="Gray" HorizontalAlignment="Center"/>
                                                    <TextBlock Text="{Binding Score, StringFormat='{}{0}%'}" FontSize="18" FontWeight="Bold" Foreground="#2563EB" HorizontalAlignment="Center"/>
                                                </StackPanel>
                                                <Border Grid.Column="2" Background="#EFF6FF" CornerRadius="8" Padding="12,8" VerticalAlignment="Center">
                                                    <TextBlock Text="{Binding Band}" FontWeight="SemiBold" Foreground="#2563EB" FontSize="12" HorizontalAlignment="Center"/>
                                                </Border>
                                                <TextBlock Grid.Column="3" Text="{Binding AssessmentDate, StringFormat='{}{0:dd-MMM}'}" FontSize="12" Foreground="Gray" VerticalAlignment="Center" HorizontalAlignment="Center"/>
                                                <Button Grid.Column="4" Width="50" Height="32" CornerRadius="6" HorizontalAlignment="Right" VerticalAlignment="Center">
                                                    <TextBlock Text="View" FontSize="11"/>
                                                </Button>
                                            </Grid>
                                        </Border>
                                    </DataTemplate>
                                </ItemsControl.ItemTemplate>
                            </ItemsControl>
                        </ScrollViewer>
                    </StackPanel>
                </Border>

            </StackPanel>
        </ScrollViewer>

    </Grid>
</Window>
```

---

## ? Step 3: Create Code-Behind File

**Create file:** `CareerCounsellingApp\Views\ReportsWindow.axaml.cs`

**Content:**
```csharp
using Avalonia.Controls;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            DataContext = new ReportsViewModel();
        }
    }
}
```

---

## ? Step 4: Restore Packages

```bash
cd F:\CareerCounsellingApp\CareerCounsellingApp
dotnet restore
```

This downloads itext7 package.

---

## ? Step 5: Build Project

```bash
dotnet build
```

Should complete successfully.

---

## ? Step 6: Test

```bash
dotnet run
```

1. Login as admin
2. Click "Reports" in sidebar
3. Reports window opens with PDF export button
4. Click "?? Export PDF" button
5. See "Generating PDF report..." message
6. PDF generates and opens automatically
7. Check Documents folder for file
8. File named: `Assessment_Report_YYYY-MM-DD_HHmmss.pdf`

---

## ?? PDF Export Features

? **Export Button** - In Reports header  
? **Professional PDF** - Formatted tables and colors  
? **Auto Open** - Opens PDF after generation  
? **Status Messages** - Shows progress/success  
? **File Saved** - To Documents folder  
? **Complete Data** - All reports included  

---

## ?? PDF Sections

1. **Title** - "Assessment Reports Dashboard"
2. **Summary Statistics** - 4 key metrics
3. **Performance Distribution** - Band breakdown with bars
4. **Category Performance** - Category analysis
5. **Student Performance** - First 50 students
6. **Footer** - Generated timestamp

---

## ?? Time Required

- Delete files: 1 min
- Create XAML: 2 min
- Create code-behind: 1 min
- Restore packages: 2 min
- Build: 2 min
- Test: 2 min

**Total: ~10 minutes** ?

---

## ?? Documentation

See these files for more details:
- `PDF_EXPORT_GUIDE.md` - Full feature guide
- `PDF_EXPORT_SUMMARY.md` - Complete summary
- `PDF_EXPORT_QUICK_START.md` - 5-minute guide

---

## ? What You Get

After completion:

? Professional PDF export feature  
? Beautiful Reports dashboard  
? 4 summary metric cards  
? Performance distribution chart  
? Category performance analysis  
? Complete student performance list  
? User-friendly status messages  
? Automatic file handling  

**Ready to deploy!**

