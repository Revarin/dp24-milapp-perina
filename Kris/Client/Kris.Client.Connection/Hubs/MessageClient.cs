using FluentResults;
using Kris.Client.Common.Events;
using Kris.Client.Common.Options;
using Kris.Client.Connection.Hubs.Events;
using Kris.Client.Data.Cache;
using Kris.Common;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Kris.Client.Connection.Hubs;

public sealed class MessageClient : IMessageHub, IMessageReceiver
{
    private readonly ConnectionOptions _connectionOptions;
    private readonly IIdentityStore _identityStore;

    public event EventHandler<MessageReceivedEventArgs> MessageReceived;
    public event EventHandler<ResultEventArgs> ErrorReceived;

    public bool IsConnected { get; private set; }

    private HubConnection _hubConnection;

    public MessageClient(IOptions<ConnectionOptions> options, IIdentityStore identityStore)
    {
        _connectionOptions = options.Value;
        _identityStore = identityStore;
    }

    public async Task Connect()
    {
        var jwt = _identityStore.GetJwtToken();

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_connectionOptions.ApiUrl}/MessageHub", options =>
            {
                options.Headers.Add(Constants.ApiKeyHeader, _connectionOptions.ApiKey);
                options.AccessTokenProvider = () => Task.FromResult(jwt.Token);
            })
            .Build();

        _hubConnection.On<MessageModel>(nameof(ReceiveMessage), ReceiveMessage);
        _hubConnection.On<Response>(nameof(ReceiveError), ReceiveError);

        await _hubConnection.StartAsync().ContinueWith(_ => IsConnected = true);
    }

    public async Task Disconnect()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync().ContinueWith(_ => IsConnected = false);
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }

    public async Task SendMessage(SendMessageRequest request)
    {
        if (_hubConnection.State != HubConnectionState.Connected)
        {
            await _hubConnection.StartAsync();
        }

        await _hubConnection.SendAsync("SendMessage", request);
    }

    public async Task ReceiveMessage(MessageModel message)
    {
        await Application.Current.MainPage.Dispatcher.DispatchAsync(() =>
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
        });
    }

    public async Task ReceiveError(Response response)
    {
        throw new Exception("MESSAGE SEND ERROR");
        await Application.Current.MainPage.Dispatcher.DispatchAsync(() =>
        {
            ErrorReceived?.Invoke(this, new ResultEventArgs(Result.Fail("ERROR")));
        });
    }
}
