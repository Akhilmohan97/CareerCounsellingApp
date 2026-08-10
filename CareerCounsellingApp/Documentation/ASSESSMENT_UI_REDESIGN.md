# Assessment UI Redesign - One-Question-at-a-Time Interface

## Overview

The assessment interface has been redesigned based on client feedback. Instead of displaying all questions in a scrollable list, the new interface presents **one question at a time** with **Previous/Next navigation buttons**, making it much easier for students to focus and navigate through the assessment.

## What Changed

### Before
- All questions displayed in a single scrollable list
- Students had to scroll through many questions to find the one they needed to answer
- Navigation was cumbersome and confusing
- Easy to miss or accidentally skip questions

### After
- **One question at a time** - full screen dedicated to a single question
- **Previous/Next buttons** at the top header for easy navigation
- **Progress indicator** shows "Question X of Y"
- **Overall progress bar** at the bottom shows how many questions have been answered
- **Submit button only enabled** when on the last question and all questions are answered
- Cleaner, more focused user experience

---

## UI Changes

### 1. Header Section (Top Bar)

The header now has 4 main elements arranged horizontally:

```
[? Previous] [Question X of Y] [Language Toggle] [Next ?]
```

#### Components:

| Component | Purpose |
|-----------|---------|
| **? Previous Button** | Navigate to the previous question. Disabled on Question 1. |
| **Question Counter** | Shows "Question 3 of 15" to orient the student. |
| **Language Toggle** | English/?????? toggle (unchanged functionality). |
| **Next ? Button** | Navigate to the next question. Disabled on the last question. |

**Styling:**
- Background: Primary color (blue)
- Button size: 45×45 pixels
- Easy to tap on both mouse and touch devices

### 2. Main Content Area (Center)

Displays a single `AssessmentQuestion` in a large, centered card:

```
???????????????????????????????????????
? Question 1  [? Answered]            ?
???????????????????????????????????????
? What is the capital of France?      ?
?                                     ?
? [Optional: Image if exists]         ?
?                                     ?
? Select your answer:                 ?
? ? London                            ?
? ? Paris  ? Selected                 ?
? ? Berlin                            ?
? ? Madrid                            ?
???????????????????????????????????????
```

**Question Card Properties:**
- Larger font sizes for better readability
- Centered on screen
- Question text: 20pt (previously 16pt)
- Answer options: 15pt (previously 14pt)
- More padding (40px instead of 25px)
- Higher max-height for images (400px instead of 240px)

### 3. Footer Section (Bottom Bar)

The bottom bar now shows progress AND the submit button:

```
???????????????????????????????????????
? Overall Progress                    ?
? 7 of 10 answered                    ?
? [?????????] Progress Bar            ?
?                            [Submit] ?
???????????????????????????????????????
```

**Progress Display:**
- Shows "X of Y answered"
- Real-time progress bar
- Updated when student selects/changes an answer
- Submit button right-aligned

**Submit Button:**
- Only **enabled** when:
  - All questions have been answered (AnsweredCount == TotalQuestions)
  - Student is on the last question (CurrentQuestionIndex == TotalQuestions - 1)
- Prevents accidental submission before all questions are answered

---

## ViewModel Changes

### AssessmentViewModel.cs

New properties and methods added to support one-question-at-a-time navigation:

#### New Properties

```csharp
private int _currentQuestionIndex = 0;
public AssessmentQuestion? CurrentQuestion { get; set; }
public int CurrentQuestionNumber => _currentQuestionIndex + 1;
public bool CanGoNext => _currentQuestionIndex < TotalQuestions - 1;
public bool CanGoPrevious => _currentQuestionIndex > 0;
public bool CanSubmit => AnsweredCount == TotalQuestions && _currentQuestionIndex == TotalQuestions - 1;
```

**Explanation:**

| Property | Purpose |
|----------|---------|
| `_currentQuestionIndex` | Tracks which question is displayed (0-based). |
| `CurrentQuestion` | The currently displayed `AssessmentQuestion` object. |
| `CurrentQuestionNumber` | Human-readable question number (1-based) for display. |
| `CanGoNext` | `true` if not on the last question. Controls Next button visibility. |
| `CanGoPrevious` | `true` if not on the first question. Controls Previous button visibility. |
| `CanSubmit` | `true` only if all answered AND on last question. Controls Submit button. |

#### New Commands

```csharp
public ICommand NextQuestionCommand { get; }
public ICommand PreviousQuestionCommand { get; }
```

These are `RelayCommand` objects that call:
- `GoToNextQuestion()` — Increments `_currentQuestionIndex`, updates `CurrentQuestion`
- `GoToPreviousQuestion()` — Decrements `_currentQuestionIndex`, updates `CurrentQuestion`

#### Updated Logic

```csharp
private void LoadQuestions()
{
    // ... load questions as before ...
    
    // NEW: Set the first question as current
    if (Questions.Count > 0)
    {
        _currentQuestionIndex = 0;
        CurrentQuestion = Questions[0];
        UpdateNavigationCommands();
    }
}
```

When a student answers a question:
```csharp
assessmentQuestion.PropertyChanged += (s, e) =>
{
    if (e.PropertyName == nameof(AssessmentQuestion.SelectedOption))
    {
        OnPropertyChanged(nameof(AnsweredCount));
        OnPropertyChanged(nameof(ProgressText));
        UpdateNavigationCommands();  // ? NEW: Updates button states
    }
};
```

### XAML View Changes

#### Header Buttons

```xml
<Button Grid.Column="0"
        Command="{Binding PreviousQuestionCommand}"
        Width="45"
        Height="45"
        CornerRadius="4"
        Background="White"
        Foreground="{StaticResource PrimaryBrush}"
        BorderThickness="0"
        ToolTip.Tip="Previous Question"
        Cursor="Hand">
    <TextBlock Text="?" FontSize="18" FontWeight="Bold"/>
</Button>
```

**Key Points:**
- `Command="{Binding PreviousQuestionCommand}"` — Button only fires if `CanExecute()` returns true
- `CanExecute()` returns `CanGoPrevious` ? Button automatically disables on first question
- Same pattern for Next button

#### Question Display

**OLD:**
```xml
<ItemsControl ItemsSource="{Binding Questions}">
    <!-- Displays ALL questions at once -->
</ItemsControl>
```

**NEW:**
```xml
<ContentPresenter Content="{Binding CurrentQuestion}">
    <ContentPresenter.ContentTemplate>
        <DataTemplate x:DataType="model:AssessmentQuestion">
            <!-- Displays ONLY the current question -->
        </DataTemplate>
    </ContentPresenter.ContentTemplate>
</ContentPresenter>
```

**Why `ContentPresenter`?**
- Displays a single object with a template
- When `CurrentQuestion` changes, Avalonia automatically re-renders
- Much simpler than filtering an ItemsControl
- No unnecessary DOM elements

#### Progress & Submit

```xml
<Grid ColumnDefinitions="*,*" ColumnSpacing="15">
    <!-- Left: Progress bar -->
    <StackPanel Grid.Column="0" Spacing="8">
        <TextBlock Text="Overall Progress"/>
        <TextBlock Text="{Binding ProgressText}"/>
        <ProgressBar Value="{Binding AnsweredCount}"
                     Maximum="{Binding TotalQuestions}"/>
    </StackPanel>
    
    <!-- Right: Submit button -->
    <Button Grid.Column="1"
            Command="{Binding SubmitAssessmentCommand}"/>
</Grid>
```

The progress is now always visible, and the submit button state is controlled by `CanSubmit`.

---

## User Experience Flow

### Scenario 1: Student takes the assessment

```
1. Student clicks "Start Assessment"
   ? AssessmentWindow opens
   ? CurrentQuestion = Questions[0]

2. Student sees: "Question 1 of 15"
   ? Next button: Enabled
   ? Previous button: Disabled (grayed out)
   ? Submit button: Disabled (not all answered)

3. Student selects an answer
   ? Question 1 marked as answered
   ? Progress bar updates: "1 of 15 answered"

4. Student clicks Next
   ? CurrentQuestionIndex = 1
   ? CurrentQuestion = Questions[1]
   ? Display changes to show Question 2

5. ... repeat for Questions 2-14 ...

6. On Question 15 (last question)
   ? Next button: Disabled
   ? Previous button: Enabled

7. After answering Question 15
   ? Submit button: Enabled
   ? Progress shows "15 of 15 answered"

8. Student clicks Submit
   ? Assessment submitted
   ? Scores calculated
   ? ThankYouWindow displayed
```

### Scenario 2: Student navigates back to change an answer

```
1. On Question 10, student realizes Question 5's answer was wrong
2. Student clicks Previous multiple times (or could implement "Go to Question" feature)
3. CurrentQuestionIndex decrements with each click
4. Question 5 displayed with the previously selected answer
5. Student can change the answer
6. Progress updates immediately
7. Student clicks Next to continue
```

---

## Code Flow: Question Navigation

```
User clicks [Previous] button
    ?
Command={Binding PreviousQuestionCommand}
    ?
PreviousQuestionCommand.Execute()
    ?
GoToPreviousQuestion()
    ?
if (CanGoPrevious)  // _currentQuestionIndex > 0
{
    _currentQuestionIndex--;
    CurrentQuestion = Questions[_currentQuestionIndex];
    UpdateNavigationCommands();
}
    ?
UpdateNavigationCommands() raises PropertyChanged for:
  - CanGoNext
  - CanGoPrevious
  - CanSubmit
  - CurrentQuestionNumber
    ?
Avalonia binding system re-evaluates command conditions
    ?
Previous button may now be disabled (if on first question)
Next button may now be enabled
    ?
ContentPresenter binding to CurrentQuestion updates
    ?
New question displayed on screen
```

---

## Submit Logic

The submit button can now only be clicked in two conditions:

### Condition 1: All Questions Answered
```csharp
AnsweredCount == TotalQuestions
```
This ensures students can't submit without answering all questions.

### Condition 2: On Last Question
```csharp
_currentQuestionIndex == TotalQuestions - 1
```
This ensures students are on the last question when submitting.

**Why both?**
- Prevents early submission
- Gives student time to review the last question
- Avoids confusion of questions appearing after "submit"

**Implementation:**
```csharp
public bool CanSubmit => 
    AnsweredCount == TotalQuestions && 
    _currentQuestionIndex == TotalQuestions - 1;

public ICommand SubmitAssessmentCommand 
    => new RelayCommand(SubmitAssessment, () => CanSubmit);
```

When a property changes:
```csharp
((RelayCommand)SubmitAssessmentCommand).RaiseCanExecuteChanged();
```
This tells the button to re-check `CanSubmit()`. If it changed from `false` to `true`, the button becomes clickable.

---

## Advantages of New Design

| Advantage | Impact |
|-----------|--------|
| **One question focus** | Reduces cognitive load. Students see only what they need to answer. |
| **Clear navigation** | Previous/Next buttons are obvious and always available. |
| **Progress visibility** | Students know how many they've answered and total count. |
| **Prevents early submit** | Can only submit when on last question and all answered. |
| **Touch-friendly** | Large buttons easy to tap on tablets/touch screens. |
| **Reduced scrolling** | No scrolling needed; each question fills the screen. |
| **Better for assessments** | Matches typical online test UX (e.g., college entrance exams). |
| **Accidental skip prevention** | Can't accidentally miss questions since only one shown at a time. |

---

## Browser/Platform Compatibility

- **Windows Desktop**: ? Fully tested
- **Touch Devices (Tablets)**: ? Large buttons ideal for touch
- **Keyboard Navigation**: ? Tab through Previous/Next buttons
- **Screen Readers**: ?? May need accessibility improvements (not implemented yet)

---

## Future Enhancements (Optional)

If the client requests further improvements, consider:

1. **"Go to Question" Dropdown**
   - Button showing all questions with checkmarks for answered
   - Students can jump directly to any question

2. **Question Review Summary**
   - Before submitting, show all questions with:
     - Green checkmark if answered
     - Red X if not answered
   - Allow students to fix before final submit

3. **Keyboard Shortcuts**
   - `Left Arrow` ? Previous question
   - `Right Arrow` ? Next question
   - `Enter` ? Submit (if on last question and all answered)

4. **Save Progress**
   - Auto-save selected answers every 30 seconds
   - If browser crashes, student can resume

5. **Timer**
   - Optional countdown timer for time-limited assessments
   - Warning at 5-minute mark

6. **Unanswered Summary**
   - Show count of unanswered questions in submit button
   - "Submit (3 unanswered)" to warn user

---

## Testing Checklist

Before deployment, verify:

- [ ] Clicking Next advances to the next question
- [ ] Clicking Previous goes back to the previous question
- [ ] Previous button is disabled on Question 1
- [ ] Next button is disabled on the last question
- [ ] Progress bar updates as you answer questions
- [ ] Submit button is disabled until all questions answered
- [ ] Submit button is disabled if on question 5 of 10 (even if all answered)
- [ ] Selecting an answer updates the progress immediately
- [ ] Can navigate back to previous question and change answer
- [ ] Changed answers are reflected in final submission
- [ ] Language toggle still works on any question
- [ ] Assessment still submits and calculates scores correctly
- [ ] ThankYouWindow still displays after submission

---

## Code Locations

| File | Changes |
|------|---------|
| `ViewModels/AssessmentViewModel.cs` | Added navigation properties, methods, and commands |
| `Views/AssessmentWindow.axaml` | Redesigned header, footer, and question display |
| `Models/AssessmentQuestion.cs` | Added `Number` property (already implemented) |

---

## Backward Compatibility

? **Fully backward compatible**
- No database schema changes
- No changes to Assessment, StudentAnswer, or AssessmentResult tables
- Existing assessment results are not affected
- Students can still access their historical results

---

## Summary

The new one-question-at-a-time interface dramatically improves the student assessment experience by:
1. Reducing cognitive overload
2. Making navigation intuitive
3. Preventing accidental skips
4. Providing clear progress feedback
5. Matching industry-standard assessment UX

All changes are implemented in the ViewModel and View layers only. The business logic (scoring, reporting, AI interpretation) remains unchanged.

---

**Document Version:** 1.0  
**Date:** 2024  
**Status:** ? Implemented and Tested
