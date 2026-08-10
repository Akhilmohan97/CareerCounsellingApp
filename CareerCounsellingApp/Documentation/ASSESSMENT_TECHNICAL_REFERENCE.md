# Assessment Redesign - Technical Reference

## Code Changes Summary

### File 1: ViewModels/AssessmentViewModel.cs

#### New Fields
```csharp
private int _currentQuestionIndex = 0;  // Tracks current question (0-based)
```

#### New Properties
```csharp
// The currently displayed question
public AssessmentQuestion? CurrentQuestion { get; set; }

// Human-readable question number (1-based)
public int CurrentQuestionNumber => _currentQuestionIndex + 1;

// Can move to next question?
public bool CanGoNext => _currentQuestionIndex < TotalQuestions - 1;

// Can move to previous question?
public bool CanGoPrevious => _currentQuestionIndex > 0;

// Can submit assessment?
public bool CanSubmit => AnsweredCount == TotalQuestions 
                      && _currentQuestionIndex == TotalQuestions - 1;
```

#### New Commands
```csharp
// Navigate to next question
public ICommand NextQuestionCommand { get; }

// Navigate to previous question
public ICommand PreviousQuestionCommand { get; }
```

#### Modified Constructor
```csharp
public AssessmentViewModel(Student student, Action? onAssessmentSubmitted = null)
{
    _student = student;
    _onAssessmentSubmitted = onAssessmentSubmitted;

    // OLD: new RelayCommand(SubmitAssessment, CanSubmitAssessment);
    // NEW: Uses CanSubmit property instead
    SubmitAssessmentCommand = 
        new RelayCommand(SubmitAssessment, () => CanSubmit);
    
    // NEW: Navigation commands
    NextQuestionCommand = 
        new RelayCommand(GoToNextQuestion, () => CanGoNext);
    PreviousQuestionCommand = 
        new RelayCommand(GoToPreviousQuestion, () => CanGoPrevious);

    LoadQuestions();
}
```

#### New Methods
```csharp
// Move to next question
private void GoToNextQuestion()
{
    if (CanGoNext)
    {
        _currentQuestionIndex++;
        CurrentQuestion = Questions[_currentQuestionIndex];
        UpdateNavigationCommands();
    }
}

// Move to previous question
private void GoToPreviousQuestion()
{
    if (CanGoPrevious)
    {
        _currentQuestionIndex--;
        CurrentQuestion = Questions[_currentQuestionIndex];
        UpdateNavigationCommands();
    }
}

// Update button states after navigation or answer change
private void UpdateNavigationCommands()
{
    OnPropertyChanged(nameof(CanGoNext));
    OnPropertyChanged(nameof(CanGoPrevious));
    OnPropertyChanged(nameof(CanSubmit));
    OnPropertyChanged(nameof(CurrentQuestionNumber));
    
    ((RelayCommand)NextQuestionCommand).RaiseCanExecuteChanged();
    ((RelayCommand)PreviousQuestionCommand).RaiseCanExecuteChanged();
    ((RelayCommand)SubmitAssessmentCommand).RaiseCanExecuteChanged();
}
```

#### Modified LoadQuestions()
```csharp
private void LoadQuestions()
{
    using var db = new AppDbContext();
    Questions.Clear();

    var questions = db.Questions
        .Include(q => q.Image)
        .Include(q => q.Options)
        .AsNoTracking()
        .ToList();

    foreach (var question in questions)
    {
        var assessmentQuestion = new AssessmentQuestion 
        { 
            Question = question 
        };

        assessmentQuestion.Number = Questions.Count + 1;

        // When user selects/changes an answer
        assessmentQuestion.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(AssessmentQuestion.SelectedOption))
            {
                OnPropertyChanged(nameof(AnsweredCount));
                OnPropertyChanged(nameof(ProgressText));
                // NEW: Update all navigation commands
                UpdateNavigationCommands();
            }
        };

        foreach (var option in question.Options)
        {
            var assessmentOption = new AssessmentOption(option)
            {
                UseMalayalam = _useMalayalam
            };
            assessmentQuestion.Options.Add(assessmentOption);
        }

        Questions.Add(assessmentQuestion);
    }

    // NEW: Set first question as current
    if (Questions.Count > 0)
    {
        _currentQuestionIndex = 0;
        CurrentQuestion = Questions[0];
        UpdateNavigationCommands();
    }
}
```

---

### File 2: Views/AssessmentWindow.axaml

#### Header Section Changes

**Before:**
```xml
<StackPanel Grid.Column="0">
    <TextBlock Text="Career Assessment" FontSize="28" .../>
    <TextBlock Text="Answer all questions..." FontSize="13" .../>
</StackPanel>

<!-- Progress in right column -->
<Border Grid.Column="2" Background="#2C3E50" ...>
    <StackPanel>
        <TextBlock Text="Progress" .../>
        <TextBlock Text="{Binding ProgressText}" .../>
        <ProgressBar Value="{Binding AnsweredCount}" .../>
    </StackPanel>
</Border>
```

**After:**
```xml
<!-- Previous Button (Column 0) -->
<Button Grid.Column="0"
        Command="{Binding PreviousQuestionCommand}"
        Width="45" Height="45" ...>
    <TextBlock Text="?" FontSize="18" FontWeight="Bold"/>
</Button>

<!-- Question Counter (Column 1) -->
<StackPanel Grid.Column="1" Margin="20,0,20,0">
    <TextBlock Text="Career Assessment" FontSize="28" .../>
    <TextBlock Text="{Binding CurrentQuestionNumber, 
                     StringFormat='Question {0} of '}"
               FontSize="13" .../>
</StackPanel>

<!-- Language Toggle (Column 2) - Same as before -->
<Border Grid.Column="2" Width="150" Height="36" .../>

<!-- Next Button (Column 3) -->
<Button Grid.Column="3"
        Command="{Binding NextQuestionCommand}"
        Width="45" Height="45" ...>
    <TextBlock Text="?" FontSize="18" FontWeight="Bold"/>
</Button>
```

**Layout Definition:**
```xml
<Grid ColumnDefinitions="Auto,*,Auto,Auto">
    <!-- 45px | Flexible | 150px | 45px -->
</Grid>
```

#### Question Display Changes

**Before:**
```xml
<Border Padding="30">
    <ScrollViewer>
        <ItemsControl ItemsSource="{Binding Questions}">
            <!-- Displays ALL questions in a StackPanel -->
        </ItemsControl>
    </ScrollViewer>
</Border>
```

**After:**
```xml
<Border Padding="30">
    <ScrollViewer>
        <ContentPresenter Content="{Binding CurrentQuestion}">
            <ContentPresenter.ContentTemplate>
                <DataTemplate x:DataType="model:AssessmentQuestion">
                    <!-- Displays ONLY CurrentQuestion -->
                </DataTemplate>
            </ContentPresenter.ContentTemplate>
        </ContentPresenter>
    </ScrollViewer>
</Border>
```

**Why ContentPresenter?**
- Displays a single object with a DataTemplate
- When `CurrentQuestion` binding changes, template automatically re-renders
- Much lighter than ItemsControl for single item
- No unnecessary DOM nodes

#### Footer Section Changes

**Before:**
```xml
<Border DockPanel.Dock="Bottom" Padding="30,20">
    <Button Height="45" Command="{Binding SubmitAssessmentCommand}" ...>
        <TextBlock Text="Submit Assessment"/>
    </Button>
</Border>
```

**After:**
```xml
<Border DockPanel.Dock="Bottom" Padding="30,20">
    <Grid ColumnDefinitions="*,*" ColumnSpacing="15">
        <!-- Progress on left -->
        <StackPanel Grid.Column="0" Spacing="8">
            <TextBlock Text="Overall Progress" FontSize="12" .../>
            <TextBlock Text="{Binding ProgressText}" FontSize="14" .../>
            <ProgressBar Value="{Binding AnsweredCount}" 
                         Maximum="{Binding TotalQuestions}" 
                         Height="8" .../>
        </StackPanel>

        <!-- Submit on right -->
        <Button Grid.Column="1" Height="45" 
                Command="{Binding SubmitAssessmentCommand}" ...>
            <TextBlock Text="Submit Assessment"/>
        </Button>
    </Grid>
</Border>
```

---

## Data Flow: Navigation

### Click Next Button
```
User clicks [Next ?] Button (in header)
    ?
    ?? Button.Command = {Binding NextQuestionCommand}
    ?
    ?? Avalonia checks Command.CanExecute()
    ?   ?? Returns NextQuestionCommand.CanExecute()
    ?       ?? Which calls () => CanGoNext
    ?           ?? CanGoNext = (_currentQuestionIndex < TotalQuestions - 1)
    ?
    ?? If CanExecute() = true:
    ?   ?
    ?   ?? Command.Execute() is called
    ?   ?   ?? Calls GoToNextQuestion()
    ?   ?       ?
    ?   ?       ?? _currentQuestionIndex++
    ?   ?       ?? CurrentQuestion = Questions[_currentQuestionIndex]
    ?   ?       ?? UpdateNavigationCommands()
    ?   ?
    ?   ?? UpdateNavigationCommands()
    ?       ?
    ?       ?? Raises PropertyChanged for:
    ?       ?   ?? CanGoNext
    ?       ?   ?? CanGoPrevious
    ?       ?   ?? CanSubmit
    ?       ?   ?? CurrentQuestionNumber
    ?       ?
    ?       ?? Calls RaiseCanExecuteChanged() on all commands
    ?
    ?? Avalonia data binding system reacts to changes
    ?   ?
    ?   ?? Previous button state updates
    ?   ?? Next button state updates
    ?   ?? Submit button state updates
    ?   ?? Question number text updates
    ?   ?? ContentPresenter updates to show CurrentQuestion
    ?
    ?? UI displays Question N+1
```

### Answer Selection
```
User clicks radio button for Option 2
    ?
    ?? RadioButton_Click event fires (code-behind)
    ?
    ?? Find parent ItemsControl.DataContext = AssessmentQuestion
    ?
    ?? assessmentQuestion.SelectedOption = selectedOption
    ?
    ?? SelectedOption setter raises PropertyChanged
    ?   ?
    ?   ?? Event handlers listen for PropertyChanged
    ?       ?
    ?       ?? Handler in LoadQuestions() detects:
    ?       ?   if e.PropertyName == nameof(AssessmentQuestion.SelectedOption)
    ?       ?
    ?       ?? Calls UpdateNavigationCommands()
    ?       ?
    ?       ?? OnPropertyChanged(nameof(AnsweredCount))
    ?       ?   ?? AnsweredCount getter re-counts answered questions
    ?       ?       ?? Questions.Count(q => q.SelectedOption != null)
    ?       ?
    ?       ?? OnPropertyChanged(nameof(ProgressText))
    ?       ?   ?? ProgressText = $"{AnsweredCount} of {TotalQuestions} answered"
    ?       ?
    ?       ?? All Commands re-check CanExecute()
    ?
    ?? UI updates:
        ?? Progress bar fills
        ?? Progress text updates
        ?? Submit button might become enabled (if all answered)
        ?? Answer option shows as selected visually
```

---

## Property Change Propagation

```
AssessmentViewModel
?? Questions: ObservableCollection<AssessmentQuestion>
?  ?
?  ?? [0] AssessmentQuestion
?     ?
?     ?? Question (Question model)
?     ?? Number (int)
?     ?? SelectedOption (QuestionOption)
?     ?  ?? PropertyChanged event
?     ?     ?? Triggers UpdateNavigationCommands()
?     ?
?     ?? Options: ObservableCollection<AssessmentOption>
?
?? CurrentQuestion: AssessmentQuestion? ?? Binding target
?  ?? When changed, ContentPresenter updates
?
?? AnsweredCount: int (computed)
?  ?? OnPropertyChanged("AnsweredCount")
?     ?? Binding updates Progress bar/text
?
?? CanGoNext: bool (computed)
?  ?? OnPropertyChanged("CanGoNext")
?     ?? Next button.IsEnabled updates
?
?? CanGoPrevious: bool (computed)
?  ?? OnPropertyChanged("CanGoPrevious")
?     ?? Previous button.IsEnabled updates
?
?? CanSubmit: bool (computed)
   ?? OnPropertyChanged("CanSubmit")
      ?? Submit button.IsEnabled updates
```

---

## XAML Binding Patterns

### Command Binding with CanExecute
```xml
<Button Command="{Binding PreviousQuestionCommand}">
```
Avalonia automatically:
1. Calls `Command.CanExecute(parameter)` 
2. If false ? Button.IsEnabled = false
3. On PropertyChanged ? Re-checks CanExecute()

### ContentPresenter with Template
```xml
<ContentPresenter Content="{Binding CurrentQuestion}">
    <ContentPresenter.ContentTemplate>
        <DataTemplate x:DataType="model:AssessmentQuestion">
            <!-- Template content -->
        </DataTemplate>
    </ContentPresenter.ContentTemplate>
</ContentPresenter>
```
When `CurrentQuestion` binding changes:
1. ContentPresenter compares old vs new value
2. If changed, re-renders template with new data context
3. Old template removed, new template instantiated
4. Smooth transition between questions

### Progress Bar Binding
```xml
<ProgressBar Value="{Binding AnsweredCount}"
             Maximum="{Binding TotalQuestions}"/>
```
Automatic recalculation:
- When AnsweredCount changes ? Value binding updates
- Bar fills: (AnsweredCount / TotalQuestions) × 100%

---

## Performance Considerations

### Memory Usage
- All questions kept in memory (Questions collection)
- Only one question rendered at a time (CurrentQuestion)
- No significant memory difference vs scrollable list
- ? Acceptable for 10-50 questions

### Rendering Performance
- Only CurrentQuestion DataTemplate is rendered
- No rendering of off-screen questions
- ? Better performance than scrollable list with many questions
- ScrollViewer still present (can scroll within question if needed)

### Navigation Speed
- Next/Previous just changes `_currentQuestionIndex`
- No database calls
- Instantaneous UI update
- ? Fast, responsive navigation

---

## Testing Edge Cases

### Test 1: First Question
```csharp
_currentQuestionIndex = 0
CanGoPrevious = (_currentQuestionIndex > 0) = false
CanGoNext = (_currentQuestionIndex < TotalQuestions - 1) = true
// Expected: Previous disabled, Next enabled ?
```

### Test 2: Last Question with All Answered
```csharp
_currentQuestionIndex = 14  // Question 15 of 15
AnsweredCount = 15
TotalQuestions = 15
CanSubmit = (15 == 15 && 14 == 14) = true
// Expected: Submit enabled ?
```

### Test 3: Last Question but Only 14 Answered
```csharp
_currentQuestionIndex = 14
AnsweredCount = 14
TotalQuestions = 15
CanSubmit = (14 == 15 && 14 == 14) = false
// Expected: Submit disabled ?
```

### Test 4: All Questions Answered but on Question 5
```csharp
_currentQuestionIndex = 4  // Question 5 of 15
AnsweredCount = 15  // All answered
TotalQuestions = 15
CanSubmit = (15 == 15 && 4 == 14) = false
// Expected: Submit disabled (must be on last question) ?
```

---

## Backward Compatibility

? **Fully backward compatible**

What didn't change:
- Database schema (no new tables/columns)
- Assessment submission logic
- Score calculation
- Report generation
- AI interpretation
- Historical assessment data

What changed:
- UI presentation only (View layer)
- Navigation state (ViewModel layer)
- No impact on Model or Data layers

---

## Future Enhancement Ideas

### 1. Keyboard Shortcuts
```csharp
// Add to AssessmentWindow.axaml.cs
private void Window_KeyDown(object sender, KeyEventArgs e)
{
    switch(e.Key)
    {
        case Key.Left:
            ((RelayCommand)viewModel.PreviousQuestionCommand).Execute(null);
            break;
        case Key.Right:
            ((RelayCommand)viewModel.NextQuestionCommand).Execute(null);
            break;
        case Key.Enter:
            ((RelayCommand)viewModel.SubmitAssessmentCommand).Execute(null);
            break;
    }
}
```

### 2. Question Review Summary
```csharp
// Before submit, show:
// Q1: ? Answered
// Q2: ? Answered
// ...
// Q14: ? NOT answered
// Q15: ? Answered
```

### 3. Go to Question Dropdown
```csharp
// Button showing "Jump to Question"
// Dropdown with all questions + checkmarks
// Click to jump directly
```

### 4. Auto-Save Answers
```csharp
// Every 30 seconds:
// Save CurrentQuestion answer to database
// Show "Saved" indicator
// Allows resume if app crashes
```

---

**Technical Documentation Complete**  
**Status:** ? Implemented  
**Build Status:** ? Successful
