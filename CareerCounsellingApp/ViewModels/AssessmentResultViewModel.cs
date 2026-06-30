using CareerCounsellingApp.Data;
using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Services.AI;
using CareerCounsellingApp.Services.Assessment;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels
{
    public class AssessmentResultViewModel:INotifyPropertyChanged
    {
        private readonly AIInterpretationWorkflowService _workflow;
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
             var context = new AppDbContext();
            _reportService = new AssessmentReportService(context);
            var settings = new GeminiSettings
            {
                ApiKey = "AQ.Ab8RN6ITNPgIZCajGspKqZ1rw_XEgJxmrReL4rlqHUYkr6zVgw",
                Model = "gemini-2.5-flash"
            };
            var geminiService = new GeminiAIService(new GeminiPromptBuilder(),settings);
            _workflow = new AIInterpretationWorkflowService(context,_reportService,geminiService);
            HasAIInterpretation = _workflow.HasInterpretation(assessmentId);
            Report = _reportService.GetReport(assessmentId);
            TestGeminiCommand=new AsyncRelayCommand(async () => await TestGeminiAsync(assessmentId));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private async Task TestGeminiAsync(int assessmentId)
        {
            await _workflow.GenerateInterpretationAsync(assessmentId);
        }
    }
}
