# QUICK START TESTING GUIDE

## 30-Second Overview

The assessment interface changed from:
- ? All 15 questions in a scrollable list
- ? To: One question at a time with [? PREVIOUS] and [NEXT ?] buttons at the top

---

## How to Test

### 1. Start the Application
```
dotnet run
```

### 2. Login as Student
- Username: (any student account, or create one in Admin panel)
- Password: (student password)

### 3. Click "Start Assessment"

### 4. You Should See:
```
???????????????????????????????????????
? [?]  Question 1 of 15  [?]          ?  ? Navigation buttons here
???????????????????????????????????????
?                                     ?
? Question 1 (large text)             ?  ? Only ONE question shown
?                                     ?
? Options:                            ?
? ? Option A                          ?
? ? Option B                          ?
? ? Option C                          ?
? ? Option D                          ?
?                                     ?
???????????????????????????????????????
? Progress: 0 of 15 answered [SUBMIT] ?  ? Submit disabled until end
???????????????????????????????????????
```

### 5. Test These Scenarios

#### Test A: Basic Navigation
1. Click [? PREVIOUS] button ? Should be disabled (grayed out)
2. Select an answer (click a radio button)
3. Progress updates: "1 of 15 answered" ?
4. Click [NEXT ?] button
5. Question 2 appears ?
6. Now [? PREVIOUS] is enabled ?
7. Click [? PREVIOUS] ? Back to Question 1
8. Your answer is still selected ?

#### Test B: Progress Tracking
1. Answer Question 1
2. Progress shows "1 of 15 answered"
3. Click [NEXT ?] ? Question 2
4. Answer Question 2
5. Progress shows "2 of 15 answered" ?
6. Continue until you've answered several questions
7. Progress bar fills up as you answer more

#### Test C: Language Toggle (if applicable)
1. On any question, click "English" or "??????" toggle
2. Question text changes language ?
3. Options text changes language ?
4. Can toggle back and forth

#### Test D: Submit Button Control
1. Answer Questions 1-14
2. On Question 14: [SUBMIT] button is DISABLED ?
3. Click [NEXT ?] ? Go to Question 15
4. Answer Question 15
5. Now [SUBMIT] button is ENABLED ?
6. Click [SUBMIT]
7. Assessment submitted ?
8. See "Thank You" message ?

#### Test E: Changing Answers
1. Answer all 15 questions (getting to Q15 so you can see Submit button)
2. Click [? PREVIOUS] multiple times
3. Go back to Question 3
4. Change the answer
5. Progress bar still shows "15 of 15 answered" ?
6. Go forward to Question 15
7. Click [SUBMIT]
8. Submit succeeds with changed answer

#### Test F: Edge Cases
1. On Question 1, [? PREVIOUS] button is disabled ?
2. On Question 15, [? NEXT] button is disabled ?
3. On any question before 15, [SUBMIT] button is disabled even if all answered ?
4. Images display correctly if present ?
5. Very long question text wraps properly ?

---

## Expected Results Summary

| Test | Expected | Pass/Fail |
|------|----------|-----------|
| A1: Previous disabled on Q1 | Grayed out | [ ] |
| A2: Next advances question | Shows Q2 | [ ] |
| A3: Previous button works | Goes back to Q1 | [ ] |
| A4: Answer is saved | Selected answer visible | [ ] |
| B1: Progress updates | "1 of 15 answered" | [ ] |
| B2: Progress bar fills | Grows as answers | [ ] |
| C1: Language toggle works | Text changes language | [ ] |
| D1: Submit disabled before end | Button grayed | [ ] |
| D2: Submit enabled on Q15 | Button clickable | [ ] |
| D3: Submit works | Assessment saved | [ ] |
| E1: Can change answers | Answer updates | [ ] |
| E2: Changed answer saved | Correct score | [ ] |
| F1: Next disabled on Q15 | Grayed out | [ ] |
| F2: Images display | Visible and sized | [ ] |

---

## Common Issues & Solutions

### Issue: [NEXT] Button Disabled When Not on Last Question
**Solution:** Make sure you're answering questions. Next button is only enabled if current question answered.

### Issue: [SUBMIT] Button Still Disabled on Q15
**Solution:** Check that ALL questions are answered. Must answer all 15 before submit is enabled.

### Issue: Previous Question Doesn't Show
**Solution:** Check you're not on Question 1. Previous disabled on first question.

### Issue: Question Number Still Shows Wrong Count
**Solution:** This shouldn't happen. If it does, please screenshot and report.

### Issue: Pressing [NEXT] Doesn't Change Questions
**Solution:** Make sure you clicked the button. If button appears disabled, answer current question first.

---

## Quick Test Path (5 minutes)

```
1. Login as student (1 min)
2. Start Assessment (20 sec)
3. Answer Question 1 ? Click [NEXT ?] (30 sec)
4. Answer Question 2 ? Click [NEXT ?] (30 sec)
5. Answer Questions 3-15 quickly (2 min)
   ? On Q15, verify [SUBMIT] enabled
6. Click [SUBMIT] (10 sec)
7. Verify "Thank You" page (10 sec)
```

? If this works, the basic functionality is good!

---

## Thorough Test Path (15 minutes)

1. **Login & Start** (1 min)
2. **Test Navigation** (3 min)
   - Previous disabled on Q1
   - Next works to Q2, Q3, Q4
   - Previous works back to Q3, Q2, Q1
3. **Test Answer Persistence** (2 min)
   - Answer Q1, Q2, Q3
   - Navigate back to Q1
   - Verify answers still selected
4. **Test Progress** (2 min)
   - Check "X of Y answered" updates
   - Check progress bar fills
5. **Test Submit Control** (3 min)
   - On Q5: Verify Submit disabled
   - Navigate to Q15
   - Answer all remaining questions
   - Verify Submit becomes enabled on Q15
6. **Submit & Verify** (2 min)
   - Click Submit
   - Verify assessment saved
   - Verify scores calculated
   - See Thank You page

---

## Regression Testing Checklist

Make sure we didn't break anything:

- [ ] Student dashboard still works
- [ ] Can still logout
- [ ] Admin can still create assessments
- [ ] Questions with images still display correctly
- [ ] English/Malayalam language toggle still works
- [ ] Scoring still calculates correctly
- [ ] Reports still display results
- [ ] AI interpretation still generates (if enabled)
- [ ] Previous assessments still accessible
- [ ] Results history unchanged

---

## What NOT to Expect

? Auto-advancement to next question (doesn't happen, use [NEXT ?] button)  
? Submit button enabled before Question 15 (intended behavior)  
? Showing all questions at once (that was the old design)  
? Scrolling through long lists (one question at a time now)  

---

## Success Criteria

? All answers accepted and saved  
? Navigation buttons work smoothly  
? Progress bar accurate  
? Submit only works when ready  
? No errors in logs  
? Assessment completes successfully  

---

## Report Issues

If you find a problem, please include:

1. **What happened:** What did you do?
2. **What was expected:** What should happen?
3. **What actually happened:** What went wrong?
4. **Screenshot:** If possible
5. **Steps to reproduce:** How do we recreate the issue?

Example:
```
Issue: Submit button disabled on Q15 even after answering all questions
Expected: Submit button enabled when all answered and on Q15
What happened: Submit button stayed grayed out
Steps: 
  1. Answered all 15 questions
  2. Reached Q15
  3. Submit button was grayed out
  4. Tried to click - nothing happened
```

---

## Contact Support

If tests fail or you have questions, refer to:
- `ASSESSMENT_UI_REDESIGN.md` — Full explanation
- `ASSESSMENT_TECHNICAL_REFERENCE.md` — Technical details
- `ASSESSMENT_VISUAL_GUIDE.md` — UI layouts

---

**Ready to Test? Let's go! ?**
