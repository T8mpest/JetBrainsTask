using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace DefaultNamespace;

  public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BuildPromptService _buildPromptService;

        private string _originalCode = string.Empty;
        private string _modifiedCode = string.Empty;
        private string _promptText = string.Empty;
        private string _fileName = "Sample.cs";

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

        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(); }
        }

        public ICommand BuildPromptCommand { get; }

        public MainViewModel(BuildPromptService buildPromptService)
        {
            _buildPromptService = buildPromptService;

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

            BuildPromptCommand = new RelayCommand(_ => BuildPrompt());
        }

        private void BuildPrompt()
        {
            PromptText = _buildPromptService.Build(OriginalCode, ModifiedCode, FileName);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
