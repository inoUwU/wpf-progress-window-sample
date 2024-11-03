namespace wpf_progress_window_sample.Model;

/// <summary>
///     進捗情報
/// </summary>
public class ProgressInfo
{
    /// <summary>
    ///     コンストラクタ
    /// </summary>
    /// <param name="value">進捗値</param>
    /// <param name="text">進捗テキスト</param>
    public ProgressInfo(int value, string? text)
    {
        ProgressValue = value;
        ProgressText = text;
    }

    /// <summary>
    ///     進捗値
    /// </summary>
    public int ProgressValue { get; set; }

    /// <summary>
    ///     進捗テキスト
    /// </summary>
    public string? ProgressText { get; set; }
}