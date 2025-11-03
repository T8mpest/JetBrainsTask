using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

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
    private bool _alsoFillModified = true; 

    public string OriginalCode { get => _originalCode; set { _originalCode = value; OnPropertyChanged(); } }
    public string ModifiedCode { get => _modifiedCode; set { _modifiedCode = value; OnPropertyChanged(); } }
    public string PromptText  { get => _promptText;  set { _promptText = value; OnPropertyChanged(); } }
    public string LlmResponse { get => _llmResponse; set { _llmResponse = value; OnPropertyChanged(); } }
    public string FileName    { get => _fileName;    set { _fileName    = value; OnPropertyChanged(); } }

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
    }

    public bool AlsoFillModified
    {
        get => _alsoFillModified;
        set { _alsoFillModified = value; OnPropertyChanged(); }
    }

    public ICommand OpenOriginalCommand { get; }
    public ICommand OpenModifiedCommand { get; }
    public ICommand BuildPromptCommand { get; }
    public ICommand SendToLlmCommand { get; }

    public MainViewModel(BuildPromptService buildPromptService, AnalyzeCodeWithLlmService analyzeCodeWithLlmService)
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

        OpenOriginalCommand = new RelayCommand(async _ => await OpenFileAsync(toModified:false), _ => !IsBusy);
        OpenModifiedCommand = new RelayCommand(async _ => await OpenFileAsync(toModified:true),  _ => !IsBusy);
        BuildPromptCommand = new RelayCommand(_ => BuildPrompt(),               _ => !IsBusy);
        SendToLlmCommand   = new RelayCommand(async _ => await SendToLlmAsync(), _ => !IsBusy);
    }

    private async Task OpenFileAsync(bool toModified)
    {
        var dlg = new OpenFileDialog
        {
            Title = "Open source file",
            Filter = "Source files (*.cs;*.csx;*.java;*.kt;*.py;*.js;*.ts;*.txt)|*.cs;*.csx;*.java;*.kt;*.py;*.js;*.ts;*.txt|All files (*.*)|*.*",
            Multiselect = false
        };

        if (dlg.ShowDialog() != true) return;

        try
        {
            string text;
            using (var sr = new StreamReader(dlg.FileName, detectEncodingFromByteOrderMarks: true))
                text = await sr.ReadToEndAsync();
            
            const int maxChars = 200_000;
            if (text.Length > maxChars)
                text = text[..maxChars] + "\n// [truncated for preview]\n";
            
            if (toModified)
            {
                ModifiedCode = text;
            }
            else
            {
                OriginalCode = text;
                if (AlsoFillModified || string.IsNullOrWhiteSpace(ModifiedCode))
                    ModifiedCode = text;
                FileName = System.IO.Path.GetFileName(dlg.FileName);
            }

            
            PromptText = string.Empty;
            LlmResponse = string.Empty;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open file:\n{ex.Message}", "Open file error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BuildPrompt() =>
        PromptText = _buildPromptService.Build(OriginalCode, ModifiedCode, FileName);

    private async Task SendToLlmAsync()
    {
        IsBusy = true;
        try
        {
            var (prompt, response) = await _analyzeCodeWithLlmService.AnalyzeAsync(OriginalCode, ModifiedCode, FileName);
            PromptText = prompt;
            LlmResponse = response;
        }
        finally { IsBusy = false; }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
