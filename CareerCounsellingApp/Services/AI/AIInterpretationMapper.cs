using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.AI;

public class AIInterpretationMapper
{
    public static AIInterpretation ToEntity(
        AIInterpretationDto dto,
        int assessmentResultId,
        string modelName)
    {
        return new AIInterpretation
        {
            AssessmentResultId = assessmentResultId,

            ExecutiveSummary = dto.ExecutiveSummary,

            StrengthsJson = JsonSerializer.Serialize(dto.Strengths),

            DevelopmentAreasJson = JsonSerializer.Serialize(dto.DevelopmentAreas),

            DiscussionPointsJson = JsonSerializer.Serialize(dto.DiscussionPoints),

            GeneratedOn = DateTime.Now,

            ModelName = modelName
        };
    }
}
