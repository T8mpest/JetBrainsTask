using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace DefaultNamespace;

   public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BuildPromptService _buildPromptService;
        private readonly AnalyzeCodeWithLlmService _analyzeCodeWithLlmService;

        private string _originalCode = string.Empty;
        private string _modifiedCode = string.Empty;
        private string _promptText = string.Empty;
        private string _llmResponse = string.Empty;
        private string _fileName = "Sample.cs";
        private bool _isBusy;

        public string OriginalCode
        {
            get => _originalCode;
            set { _originalCode = value; OnPropertyChanged(); }
        }

        public string ModifiedCode
        {
            get => _modifiedCode;
            set { _modifiedCode = value; OnPropertyChanged(); }
        }

        public string PromptText
        {
            get => _promptText;
            set { _promptText = value; OnPropertyChanged(); }
        }

        public string LlmResponse
        {
            get => _llmResponse;
            set { _llmResponse = value; OnPropertyChanged(); }
        }

        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand BuildPromptCommand { get; }
        public ICommand SendToLlmCommand { get; }

        public MainViewModel(
            BuildPromptService buildPromptService,
            AnalyzeCodeWithLlmService analyzeCodeWithLlmService)
        {
            _buildPromptService = buildPromptService;
            _analyzeCodeWithLlmService = analyzeCodeWithLlmService;

            // demo data
            OriginalCode =
@"public class Calculator
{
    public int Add(int a, int b) => a + b;
}";
            ModifiedCode =
@"public class Calculator
{
    public int Add(int a, int b)
    {
        // TODO: logging
        return a + b;
    }

    public int Sub(int a, int b) => a - b;
}";

            BuildPromptCommand = new RelayCommand(_ => BuildPrompt(), _ => !IsBusy);
            SendToLlmCommand = new RelayCommand(async _ => await SendToLlmAsync(), _ => !IsBusy);
        }

        private void BuildPrompt()
        {
            PromptText = _buildPromptService.Build(OriginalCode, ModifiedCode, FileName);
        }

        private async Task SendToLlmAsync()
        {
            IsBusy = true;
            try
            {
                var (prompt, response) =
                    await _analyzeCodeWithLlmService.AnalyzeAsync(OriginalCode, ModifiedCode, FileName);
                PromptText = prompt;      
                LlmResponse = response;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }