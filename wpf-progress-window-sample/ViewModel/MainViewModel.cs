using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using wpf_progress_window_sample.Model;

namespace wpf_progress_window_sample.ViewModel;

public class MainViewModel : ObservableObject
{
    /// <summary>
    ///     実行中かどうか
    /// </summary>
    private bool _isBusy;

    private bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    /// <summary>
    /// 実行中の進捗情報
    /// </summary>
    private string? _progressText;

    public int ProgressValue
    {
        get => _progressValue;
        private set => SetProperty(ref _progressValue, value);
    }

    /// <summary>
    /// 実行済みの進捗値
    /// </summary>
    private int _progressValue;

    public string? ProgressText
    {
        get => _progressText;
        private set => SetProperty(ref _progressText, value);
    }

    private bool _progressVisible;

    public bool ProgressVisible
    {
        get => _progressVisible;
        set => SetProperty(ref _progressVisible, value);
    }

    /// <summary>
    ///     実行コマンド
    /// </summary>
    public IAsyncRelayCommand ExecuteCommand { get; }

    /// <summary>
    ///     キャンセルコマンド
    /// </summary>
    public IRelayCommand CancelCommand { get; }

    private HeavyWork? _model;

    /// <summary>
    ///     コンストラクタ
    /// </summary>
    public MainViewModel()
    {
        IsBusy = false;

        // プログレスリングを非表示
        ProgressVisible = false;

        // 実行コマンド初期化
        ExecuteCommand = new AsyncRelayCommand(OnExecuteAsync, CanExecute);

        // キャンセルコマンド初期化
        CancelCommand = new RelayCommand(
            () =>
            {
                _model?.Cancel();
                // コマンドの実行可否 更新
                UpdateCommandStatus();
            },
            () => IsBusy);
    }

    /// <summary>
    /// 実行コマンドの処理
    /// </summary>
    private async Task OnExecuteAsync()
    {
        IsBusy = true;

        // プログレスリングを表示
        ProgressVisible = true;

        // コマンドの実行可否 更新
        UpdateCommandStatus();

        _model = new HeavyWork();

        var p = new Progress<ProgressInfo>();

        p.ProgressChanged += (sender, e) =>
        {
            ProgressValue = e.ProgressValue;
            ProgressText = e.ProgressText;
        };

        // 時間のかかる処理 開始
        await _model.ExecuteAsync(p);

        IsBusy = false;
        // コマンドの実行可否 更新
        UpdateCommandStatus();
    }

    /// <summary>
    /// 実行コマンドの実行可否
    /// </summary>
    /// <returns></returns>
    private bool CanExecute()
    {
        // 処理中でなければ実行可
        return !IsBusy;
    }

    /// <summary>
    /// コマンドの実行可否 更新
    /// </summary>
    private void UpdateCommandStatus()
    {
        ExecuteCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
    }
}