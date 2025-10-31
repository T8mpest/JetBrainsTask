using System.Windows;
using DefaultNamespace;
using JetBrainsTask.Infrastructure.Llm;

namespace JetBrainsTask.Presentation.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // infra
        var diff = new SimpleLineDiffService();
        var promptBuilder = new DefaultPromptBuilder();
        var llmClient = new OpenAiLlmClient(); 

        // application
        var buildPromptService = new BuildPromptService(diff, promptBuilder);
        var analyzeWithLlm = new AnalyzeCodeWithLlmService(buildPromptService, llmClient);

        // vm
        DataContext = new MainViewModel(buildPromptService, analyzeWithLlm);
    }
}