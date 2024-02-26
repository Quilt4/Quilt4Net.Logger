using Quilt4Net.Entities;

namespace Quilt4Net.Internals;

internal class StateService : IStateService
{
    private readonly Quilt4NetOptions _options;
    private int _sendCount;
    private ELoggerState _lastState;

    public StateService(IConfigurationEngine configurationEngine, ISenderEngine senderEngine, IMessageQueue messageQueue, Quilt4NetOptions options)
    {
        _options = options;

        messageQueue.QueueEvent += (s, e) =>
        {
            try
            {
                options.QueueEvent?.Invoke(e);
            }
            catch
            {
                // ignored
            }
        };

        HandleLoggerState(configurationEngine, senderEngine, messageQueue);

        Task.Run(async () =>
        {
            try
            {
                if (!configurationEngine.Started) await configurationEngine.StartAsync(CancellationToken.None);
                if (!senderEngine.Started) await senderEngine.StartAsync(CancellationToken.None);
            }
            catch (Exception e)
            {
                FireAction(ELoggerState.Crash, messageQueue.QueueCount, e);
            }
        });
    }

    private void HandleLoggerState(IConfigurationEngine configurationEngine, ISenderEngine senderEngine, IMessageQueue messageQueue)
    {
        FireAction(ELoggerState.Initiated, messageQueue.QueueCount);

        if (!configurationEngine.Started || !senderEngine.Started)
        {
            FireAction(ELoggerState.WaitingToStart, messageQueue.QueueCount);
        }
        else if (!configurationEngine.HaveConfiguration)
        {
            FireAction(ELoggerState.WaitingForConfiguration, messageQueue.QueueCount);
        }

        senderEngine.SendEvent += (s, e) =>
        {
            var state = _lastState;
            switch (e.SendAction)
            {
                case ESendActionEh.Success:
                    state = ELoggerState.Online;
                    _sendCount++;
                    break;
                case ESendActionEh.Fail:
                    break;
                case ESendActionEh.Crash:
                    state = ELoggerState.Crash;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown send action {e.SendAction}.");
            }
            FireAction(state, messageQueue.QueueCount, e.Exception);

            try
            {
                switch (e.SendAction)
                {
                    case ESendActionEh.Success:
                        _options.SendEvent?.Invoke(new SendEventArgs(ESendAction.Success, _sendCount, e.Message));
                        break;
                    case ESendActionEh.Fail:
                        _options.SendEvent?.Invoke(new SendEventArgs(ESendAction.Fail, _sendCount, e.Message));
                        break;
                    case ESendActionEh.Crash:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                // ignored
            }
        };
        configurationEngine.ConfigurationEvent += (s, e) =>
        {
            var state = _lastState;
            switch (e.ConfigurationAction)
            {
                case EConfigurationAction.Success:
                    if (state < ELoggerState.Ready) state = ELoggerState.Ready;
                    break;
                case EConfigurationAction.Crash:
                    state = ELoggerState.Crash;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            FireAction(state, messageQueue.QueueCount, e.Exception);
        };
    }

    private void FireAction(ELoggerState state, int queueCount, Exception exception = null)
    {
        if (state == _lastState) return;

        try
        {
            _lastState = state;
            _options.LogStateEvent?.Invoke(new StateChangedEventArgs(state, /*queueCount, _sendCount,*/ exception));
        }
        catch
        {
            // ignored
        }
    }
}