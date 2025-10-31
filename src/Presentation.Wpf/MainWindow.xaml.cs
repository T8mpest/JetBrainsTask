using System.Windows;
using DefaultNamespace;

namespace JetBrainsTask.Presentation.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // manual DI
        var diffService = new SimpleLineDiffService();
        var promptBuilder = new DefaultPromptBuilder();
        var buildPromptService = new BuildPromptService(diffService, promptBuilder);

        DataContext = new MainViewModel(buildPromptService);
    }
}