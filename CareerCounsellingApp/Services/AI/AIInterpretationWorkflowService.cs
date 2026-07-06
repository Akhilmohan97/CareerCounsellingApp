using CareerCounsellingApp.Data;
using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Services.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.AI
{
    public class AIInterpretationWorkflowService
    {
        private readonly AppDbContext _db;
        private readonly AssessmentReportService _reportService;
        private readonly IAIInterpretationService _aiService;

        public AIInterpretationWorkflowService(
            AppDbContext db,
            AssessmentReportService reportService,
            IAIInterpretationService aiService)
        {
            _db = db;
            _reportService = reportService;
            _aiService = aiService;
        }
        public async Task GenerateInterpretationAsync(int assessmentResultId)
        {
            var existing = _db.AIInterpretations
                           .FirstOrDefault(x => x.AssessmentResultId == assessmentResultId);

            if (existing != null)
                return;
            AssessmentReportDto report =_reportService.GetReport(assessmentResultId);
            AIInterpretationDto interpretation =await _aiService.GenerateAsync(report);
            var entity =AIInterpretationMapper.ToEntity(interpretation,assessmentResultId,"gemini-2.5-flash");
            _db.AIInterpretations.Add(entity);

            await _db.SaveChangesAsync();
        }
        public bool HasInterpretation(int assessmentResultId)
        {
            return _db.AIInterpretations
          .Any(x => x.AssessmentResultId == assessmentResultId);
        }
    }
}
