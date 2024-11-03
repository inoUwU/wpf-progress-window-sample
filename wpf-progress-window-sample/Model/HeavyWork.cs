namespace wpf_progress_window_sample.Model;

/// <summary>
///     重い処理を実行するクラス
/// </summary>
public class HeavyWork
{
    private const int SDataCount = 100;
    private CancellationTokenSource _cancellationTokenSource = null!;

    /// <summary>
    /// 二種類のtaskを実行し進捗率を返します
    /// </summary>
    /// <param name="progress"></param>
    public async Task ExecuteAsync(IProgress<ProgressInfo> progress)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        // HeyWorkAとHeyWorkBを同時に実行
        var subProgress = new Progress<SubProgressInfo>();

        var done = 0;
        subProgress.ProgressChanged += (sender, e) =>
        {
            var progressValue = done * 100 / (SDataCount * 2);
            progress.Report(new ProgressInfo(progressValue, $"処理中... {e.ProgressValue + 1}/{SDataCount}"));
            done++;
        };

        try
        {
            var tasks = new Task[2];
            tasks[0] = HeavyWorkA(SDataCount, token, subProgress);
            tasks[1] = HeavyWorkB(SDataCount, token, subProgress);
            await Task.WhenAll(tasks).WaitAsync(token);
        }
        catch (OperationCanceledException)
        {
            // キャンセルされた場合
            progress.Report(new ProgressInfo(0, "処理をキャンセルしました"));
        }
    }

    private async Task HeavyWorkA(int task, CancellationToken token, IProgress<SubProgressInfo> progress)
    {
        for (var i = 0; i < task; i++)
        {
            // キャンセルされたら例外発生
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();

            await Task.Delay(50, token);

            // 進捗報告
            progress.Report(new SubProgressInfo(i));
        }
    }

    private async Task HeavyWorkB(int task, CancellationToken token, IProgress<SubProgressInfo> progress)
    {
        for (var i = 0; i < SDataCount; i++)
        {
            // キャンセルされたら例外発生
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();

            await Task.Delay(50, token);

            // 進捗報告
            progress.Report(new SubProgressInfo(i));
        }
    }

    /// <summary>
    ///     処理をキャンセルする
    /// </summary>
    public void Cancel()
    {
        if (_cancellationTokenSource.IsCancellationRequested == false) _cancellationTokenSource.Cancel();
    }
}