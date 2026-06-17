using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Helpers;

public enum MessageType { Success, Error, Warning, Info }

public interface IMessageNotifier : INotifyPropertyChanged
{
    void ShowMessage(string message, MessageType type = MessageType.Info, int durationMs = 3000);
    bool IsVisible { get; }
    string Message { get; }
    string MessageTypeIcon { get; }
    string MessageColor { get; }
}

public class MessageNotifier : IMessageNotifier
{
    private bool _isVisible;
    private string _message = "";
    private MessageType _messageType;
    private CancellationTokenSource? _cancellationTokenSource;

    public bool IsVisible
    {
        get => _isVisible;
        private set
        {
            _isVisible = value;
            OnPropertyChanged(nameof(IsVisible));
        }
    }

    public string Message
    {
        get => _message;
        private set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public string MessageTypeIcon
    {
        get => _messageType switch
        {
            MessageType.Success => "?",
            MessageType.Error => "?",
            MessageType.Warning => "?",
            MessageType.Info => "?",
            _ => ""
        };
    }

    public string MessageColor
    {
        get => _messageType switch
        {
            MessageType.Success => "#10B981",
            MessageType.Error => "#EF4444",
            MessageType.Warning => "#F59E0B",
            MessageType.Info => "#2563EB",
            _ => "#6B7280"
        };
    }

    public void ShowMessage(string message, MessageType type = MessageType.Info, int durationMs = 3000)
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        Message = message;
        _messageType = type;
        IsVisible = true;

        OnPropertyChanged(nameof(MessageTypeIcon));
        OnPropertyChanged(nameof(MessageColor));

        _ = HideMessageAfterDelayAsync(durationMs, _cancellationTokenSource.Token);
    }

    private async Task HideMessageAfterDelayAsync(int delayMs, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(delayMs, cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                IsVisible = false;
            }
        }
        catch (TaskCanceledException)
        {
            // Expected when canceled
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

