namespace wpf_progress_window_sample.Model;

/// <summary>
///     進捗情報
/// </summary>
public class SubProgressInfo
{
    /// <summary>
    ///     コンストラクタ
    /// </summary>
    /// <param name="value">進捗値</param>
    public SubProgressInfo(int value)
    {
        ProgressValue = value;
    }

    /// <summary>
    ///     進捗値
    /// </summary>
    public int ProgressValue { get; set; }
}