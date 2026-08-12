using CareerCounsellingApp.DTO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font;
using iText.Kernel.Font;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.Reports
{
    public class AssessmentPdfExportService
    {
        public async Task<string> ExportAssessmentToPdfAsync(AssessmentReportDto report)
        {
            return await Task.Run(() => GeneratePdf(report));
        }

        private string GeneratePdf(AssessmentReportDto report)
        {
            try
            {
                var fileName = $"Assessment_Report_{report.Student.StudentName}_{DateTime.Now:yyyy-MM-dd_HHmmss}.pdf";
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filePath = Path.Combine(documentsPath, fileName);

                using (var writer = new PdfWriter(filePath))
                using (var pdfDocument = new PdfDocument(writer))
                using (var document = new Document(pdfDocument))
                {
                    // Set margins
                    document.SetMargins(20, 20, 20, 20);

                    // Header - Blue Background
                    var headerTable = new Table(new float[] { 1 });
                    var headerCell = new Cell()
                        .SetBackgroundColor(new DeviceRgb(37, 99, 235))
                        .SetPadding(15);

                    var titleParagraph = new Paragraph("Career Counselling Assessment Report")
                        .SetFontSize(24)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontColor(new DeviceRgb(255, 255, 255));

                    var subtitleParagraph = new Paragraph("Professional Student Assessment")
                        .SetFontSize(12)
                        .SetFontColor(new DeviceRgb(220, 235, 255))
                        .SetMarginTop(5);

                    headerCell.Add(titleParagraph);
                    headerCell.Add(subtitleParagraph);
                    headerTable.AddCell(headerCell);
                    document.Add(headerTable);

                    // Spacing
                    document.Add(new Paragraph("\n"));

                    // Student Information Section
                    document.Add(new Paragraph("Student Information")
                        .SetFontSize(18)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontColor(new DeviceRgb(30, 41, 59)));

                    var studentTable = new Table(new float[] { 1, 1 });
                    studentTable.SetWidth(UnitValue.CreatePercentValue(100));

                    AddStudentInfoRow(studentTable, "Student Name", report.Student.StudentName);
                    AddStudentInfoRow(studentTable, "Admission Number", report.Student.AdmissionNo);
                    AddStudentInfoRow(studentTable, "Course", report.Student.Course);
                    AddStudentInfoRow(studentTable, "Gender", report.Student.Gender);
                    AddStudentInfoRow(studentTable, "Age", report.Student.Age.ToString());
                    AddStudentInfoRow(studentTable, "Email", report.Student.Email);
                    AddStudentInfoRow(studentTable, "Mobile", report.Student.MobileNumber);
                    AddStudentInfoRow(studentTable, "Assessment Date", 
                        report.Student.AssessmentDate.ToString("dd-MMM-yyyy"));

                    document.Add(studentTable);
                    document.Add(new Paragraph("\n"));

                    // Overall Assessment Section
                    document.Add(new Paragraph("Overall Assessment")
                        .SetFontSize(18)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontColor(new DeviceRgb(30, 41, 59)));

                    var assessmentTable = new Table(new float[] { 1, 1 });
                    assessmentTable.SetWidth(UnitValue.CreatePercentValue(100));

                    AddAssessmentRow(assessmentTable, "Overall Percentage", 
                        $"{report.OverallPercentage}%", new DeviceRgb(37, 99, 235));
                    AddAssessmentRow(assessmentTable, "Overall Band", report.OverallBand, 
                        new DeviceRgb(16, 185, 129));
                    AddAssessmentRow(assessmentTable, "Obtained Score", 
                        report.OverallScore.ToString(), new DeviceRgb(30, 41, 59));
                    AddAssessmentRow(assessmentTable, "Maximum Score", 
                        report.MaximumScore.ToString(), new DeviceRgb(30, 41, 59));

                    document.Add(assessmentTable);
                    document.Add(new Paragraph("\n"));

                    // Overall Remark
                    if (!string.IsNullOrEmpty(report.OverallRemark))
                    {
                        document.Add(new Paragraph("Remarks")
                            .SetFontSize(14)
                            .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                            .SetFontColor(new DeviceRgb(30, 41, 59)));

                        document.Add(new Paragraph(report.OverallRemark)
                            .SetFontSize(11)
                            .SetFontColor(new DeviceRgb(71, 85, 105)));

                        document.Add(new Paragraph("\n"));
                    }

                    // Parent Category Summary
                    if (report.ParentCategories != null && report.ParentCategories.Count > 0)
                    {
                        document.Add(new Paragraph("Parent Category Summary")
                            .SetFontSize(18)
                            .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                            .SetFontColor(new DeviceRgb(30, 41, 59)));

                        var categoryTable = new Table(new float[] { 2, 1, 1 });
                        categoryTable.SetWidth(UnitValue.CreatePercentValue(100));

                        var headerCellPadding = 8f;

                        var categoryHeaderCell = new Cell()
                            .SetBackgroundColor(new DeviceRgb(226, 232, 240))
                            .SetPadding(headerCellPadding)
                            .Add(new Paragraph("Category")
                                .SetFontSize(11)
                                .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                        categoryTable.AddHeaderCell(categoryHeaderCell);

                        var percentageHeaderCell = new Cell()
                            .SetBackgroundColor(new DeviceRgb(226, 232, 240))
                            .SetPadding(headerCellPadding)
                            .Add(new Paragraph("Percentage")
                                .SetFontSize(11)
                                .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                        categoryTable.AddHeaderCell(percentageHeaderCell);

                        var bandHeaderCell = new Cell()
                            .SetBackgroundColor(new DeviceRgb(226, 232, 240))
                            .SetPadding(headerCellPadding)
                            .Add(new Paragraph("Band")
                                .SetFontSize(11)
                                .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                        categoryTable.AddHeaderCell(bandHeaderCell);

                        foreach (var category in report.ParentCategories)
                        {
                            categoryTable.AddCell(new Cell().SetPadding(6)
                                .Add(new Paragraph(category.ParentCategoryName)
                                    .SetFontSize(10)));

                            categoryTable.AddCell(new Cell().SetPadding(6)
                                .Add(new Paragraph($"{category.Percentage}%")
                                    .SetFontSize(10)
                                    .SetFontColor(new DeviceRgb(37, 99, 235))));

                            categoryTable.AddCell(new Cell().SetPadding(6)
                                .Add(new Paragraph(category.Band)
                                    .SetFontSize(10)));
                        }

                        document.Add(categoryTable);
                        document.Add(new Paragraph("\n"));
                    }

                    // Category Analysis Section
                    if (report.ParentCategories != null && report.ParentCategories.Count > 0)
                    {
                        document.Add(new Paragraph("Category Analysis")
                            .SetFontSize(18)
                            .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                            .SetFontColor(new DeviceRgb(30, 41, 59)));

                        foreach (var parentCategory in report.ParentCategories)
                        {
                            document.Add(new Paragraph(parentCategory.ParentCategoryName)
                                .SetFontSize(14)
                                .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                                .SetFontColor(new DeviceRgb(30, 41, 59))
                                .SetMarginTop(10));

                            if (parentCategory.Categories != null && parentCategory.Categories.Count > 0)
                            {
                                var subCategoryTable = new Table(new float[] { 2, 1, 1 });
                                subCategoryTable.SetWidth(UnitValue.CreatePercentValue(100));

                                var subHeaderCellPadding = 6f;

                                var subCategoryHeaderCell = new Cell()
                                    .SetBackgroundColor(new DeviceRgb(240, 245, 250))
                                    .SetPadding(subHeaderCellPadding)
                                    .Add(new Paragraph("Sub-Category")
                                        .SetFontSize(10)
                                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                                subCategoryTable.AddHeaderCell(subCategoryHeaderCell);

                                var subPercentageHeaderCell = new Cell()
                                    .SetBackgroundColor(new DeviceRgb(240, 245, 250))
                                    .SetPadding(subHeaderCellPadding)
                                    .Add(new Paragraph("Percentage")
                                        .SetFontSize(10)
                                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                                subCategoryTable.AddHeaderCell(subPercentageHeaderCell);

                                var subBandHeaderCell = new Cell()
                                    .SetBackgroundColor(new DeviceRgb(240, 245, 250))
                                    .SetPadding(subHeaderCellPadding)
                                    .Add(new Paragraph("Band")
                                        .SetFontSize(10)
                                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                                subCategoryTable.AddHeaderCell(subBandHeaderCell);

                                foreach (var category in parentCategory.Categories)
                                {
                                    subCategoryTable.AddCell(new Cell().SetPadding(4)
                                        .Add(new Paragraph(category.CategoryName)
                                            .SetFontSize(9)));

                                    subCategoryTable.AddCell(new Cell().SetPadding(4)
                                        .Add(new Paragraph($"{category.Percentage}%")
                                            .SetFontSize(9)
                                            .SetFontColor(new DeviceRgb(37, 99, 235))));

                                    subCategoryTable.AddCell(new Cell().SetPadding(4)
                                        .Add(new Paragraph(category.Band)
                                            .SetFontSize(9)));
                                }

                                document.Add(subCategoryTable);
                            }
                        }

                        document.Add(new Paragraph("\n"));
                    }

                    // Footer with timestamp
                    document.Add(new Paragraph($"Generated on: {DateTime.Now:dd-MMM-yyyy HH:mm:ss}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(9)
                        .SetFontColor(new DeviceRgb(107, 114, 128))
                        .SetMarginTop(20));
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating PDF: {ex.Message}", ex);
            }
        }

        private void AddStudentInfoRow(Table table, string label, string value)
        {
            var labelCell = new Cell()
                .SetBackgroundColor(new DeviceRgb(248, 250, 252))
                .SetPadding(8)
                .Add(new Paragraph(label)
                    .SetFontSize(10)
                    .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                    .SetFontColor(new DeviceRgb(100, 116, 139)));

            var valueCell = new Cell()
                .SetPadding(8)
                .Add(new Paragraph(value)
                    .SetFontSize(10)
                    .SetFontColor(new DeviceRgb(30, 41, 59)));

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

        private void AddAssessmentRow(Table table, string label, string value, DeviceRgb color)
        {
            var labelCell = new Cell()
                .SetBackgroundColor(new DeviceRgb(248, 250, 252))
                .SetPadding(8)
                .Add(new Paragraph(label)
                    .SetFontSize(10)
                    .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                    .SetFontColor(new DeviceRgb(100, 116, 139)));

            var valueCell = new Cell()
                .SetPadding(8)
                .Add(new Paragraph(value)
                    .SetFontSize(12)
                    .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                    .SetFontColor(color));

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

        public void OpenPdfFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
            }
            catch
            {
                // Silently fail if PDF reader not available
            }
        }
    }
}
