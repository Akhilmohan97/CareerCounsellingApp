using CareerCounsellingApp.DTO;
using System.Text;

namespace CareerCounsellingApp.Services.AI;

public class GeminiPromptBuilder
{
    public string Build(AssessmentReportDto report)
    {
        var prompt = new StringBuilder();

        prompt.AppendLine("""
                                You are an experienced professional Career Counsellor with expertise in interpreting psychometric and aptitude assessment reports.

                                Your responsibility is ONLY to interpret the assessment results.

                                IMPORTANT RULES

                                • Do NOT recommend specific careers or professions.
                                • Do NOT assign career paths.
                                • Do NOT diagnose personality or mental health conditions.
                                • Do NOT exaggerate strengths.
                                • Be objective, encouraging and professional.
                                • Use simple language that can be understood by students, parents and counsellors.
                                • Base every statement ONLY on the assessment results provided.
                                • If an area has a lower score, describe it as an opportunity for development rather than a weakness.
                                • Return ONLY valid JSON.
                                • Do NOT return Markdown.
                                • Do NOT wrap the response inside ```json blocks.
                                • Do NOT include any explanation before or after the JSON.

                                Return JSON in exactly this format:

                                {
                                    "executiveSummary": "",
                                    "strengths": [],
                                    "developmentAreas": [],
                                    "discussionPoints": []
                                }

                                """);

        prompt.AppendLine("STUDENT CONTEXT");
        prompt.AppendLine("----------------------------");
        prompt.AppendLine($"Age: {report.Student.Age}");
        prompt.AppendLine($"Course: {report.Student.Course}");
        prompt.AppendLine();

        prompt.AppendLine("OVERALL ASSESSMENT");
        prompt.AppendLine("----------------------------");
        prompt.AppendLine($"Overall Percentage: {report.OverallPercentage}%");
        prompt.AppendLine($"Overall Band: {report.OverallBand}");
        prompt.AppendLine($"Obtained Score: {report.OverallScore}");
        prompt.AppendLine($"Maximum Score: {report.MaximumScore}");
        prompt.AppendLine();

        prompt.AppendLine("PARENT CATEGORY RESULTS");
        prompt.AppendLine("----------------------------");

        foreach (var parent in report.ParentCategories)
        {
            prompt.AppendLine(
                $"{parent.ParentCategoryName}: {parent.Percentage}% ({parent.Band})");
        }

        prompt.AppendLine();

        prompt.AppendLine("CATEGORY RESULTS");
        prompt.AppendLine("----------------------------");

        foreach (var parent in report.ParentCategories)
        {
            prompt.AppendLine(parent.ParentCategoryName);

            foreach (var category in parent.Categories)
            {
                prompt.AppendLine(
                    $" - {category.CategoryName}: {category.Percentage}% ({category.Band})");
            }

            prompt.AppendLine();
        }

        prompt.AppendLine("""
                                FINAL INSTRUCTIONS

                                1. Write an executive summary in approximately 80–120 words.

                                2. Identify the student's strongest competencies based on the highest scoring assessment categories.

                                3. Identify only genuine development areas based on comparatively lower scoring categories.

                                4. Suggest practical counselling discussion points that help the counsellor explore the student's interests, learning style and personal growth.

                                5. Keep every point concise.

                                6. Do NOT repeat percentages or scores inside the interpretation.

                                7. Do NOT recommend careers, courses or educational institutions.

                                8. Maintain a positive and encouraging tone.

                                9. Return ONLY valid JSON matching the required schema.

                                """);

        return prompt.ToString();
    }
}