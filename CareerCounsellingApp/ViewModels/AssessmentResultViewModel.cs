using CareerCounsellingApp.Data;
using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Services.AI;
using CareerCounsellingApp.Services.Assessment;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels
{
    public class AssessmentResultViewModel:INotifyPropertyChanged
    {
        private readonly AIInterpretationWorkflowService _workflow;
        private readonly AppDbContext _context;
        private readonly int _assessmentId;
        private bool _hasAIInterpretation;

        public bool HasAIInterpretation
        {
            get => _hasAIInterpretation;
            set
            {
                _hasAIInterpretation = value;
                OnPropertyChanged(nameof(HasAIInterpretation));
                OnPropertyChanged(nameof(ShowGenerateButton));
            }
        }
        private bool _isGeneratingAI;

        public bool IsGeneratingAI
        {
            get => _isGeneratingAI;
            set
            {
                _isGeneratingAI = value;
                OnPropertyChanged(nameof(IsGeneratingAI));
            }
        }
        private AIInterpretationDto? _aiInterpretation;

        public AIInterpretationDto? AIInterpretation
        {
            get => _aiInterpretation;
            set
            {
                _aiInterpretation = value;
                OnPropertyChanged(nameof(AIInterpretation));
            }
        }
        
        private string _generationMessage = "";

        public string GenerationMessage
        {
            get => _generationMessage;
            set
            {
                _generationMessage = value;
                OnPropertyChanged(nameof(GenerationMessage));
            }
        }
        
        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public bool ShowGenerateButton => !HasAIInterpretation;
        private readonly AssessmentReportService _reportService;
        public ICommand TestGeminiCommand { get; }
        public AssessmentReportDto Report { get; }

        public AssessmentResultViewModel(int assessmentId)
        {
            _assessmentId = assessmentId;
            _context = new AppDbContext();
            _reportService = new AssessmentReportService(_context);
            string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;
            var settings = new GeminiSettings
            {
                ApiKey = apiKey,
                Model = "gemini-2.5-flash"
            };
            var geminiService = new GeminiAIService(new GeminiPromptBuilder(),settings);
            _workflow = new AIInterpretationWorkflowService(_context,_reportService,geminiService);
            
            Report = _reportService.GetReport(assessmentId);
            
            // Try to load existing AI interpretation
            LoadAIInterpretation();
            
            TestGeminiCommand=new AsyncRelayCommand(async () => await TestGeminiAsync(assessmentId));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        private void LoadAIInterpretation()
        {
            try
            {
                var aiInterpretation = _context.AIInterpretations
                    .FirstOrDefault(x => x.AssessmentResultId == _assessmentId);

                if (aiInterpretation != null)
                {
                    AIInterpretation = new AIInterpretationDto
                    {
                        ExecutiveSummary = aiInterpretation.ExecutiveSummary,
                        Strengths = JsonSerializer.Deserialize<List<string>>(aiInterpretation.StrengthsJson) ?? new(),
                        DevelopmentAreas = JsonSerializer.Deserialize<List<string>>(aiInterpretation.DevelopmentAreasJson) ?? new(),
                        DiscussionPoints = JsonSerializer.Deserialize<List<string>>(aiInterpretation.DiscussionPointsJson) ?? new()
                    };
                    HasAIInterpretation = true;
                }
                else
                {
                    HasAIInterpretation = false;
                }
            }
            catch (Exception ex)
            {
                GenerationMessage = $"Error loading interpretation: {ex.Message}";
            }
        }
        
        private async Task TestGeminiAsync(int assessmentId)
        {
            try
            {
                IsGeneratingAI = true;
                GenerationMessage = "Generating AI interpretation...";
                
                await _workflow.GenerateInterpretationAsync(assessmentId);
                
                // Load the newly generated interpretation
                LoadAIInterpretation();
                
                GenerationMessage = "✓ AI interpretation generated successfully!";
            }
            catch (Exception ex)
            {
                GenerationMessage = $"✗ Error: {ex.Message}";
            }
            finally
            {
                IsGeneratingAI = false;
            }
        }
    }
}
