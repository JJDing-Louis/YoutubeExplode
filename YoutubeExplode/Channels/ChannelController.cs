﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode.Bridge;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Utils;

namespace YoutubeExplode.Channels;

internal class ChannelController : YoutubeControllerBase
{
    public ChannelController(HttpClient http)
        : base(http)
    {
    }

    private async ValueTask<ChannelPageExtractor> GetChannelPageAsync(
        string channelRoute,
        CancellationToken cancellationToken = default) =>
        await Retry.WhileNullAsync(async innerCancellationToken =>
                ChannelPageExtractor.TryCreate(
                    await GetStringAsync(channelRoute, innerCancellationToken)
                ), 5, cancellationToken
        ) ??
        throw new YoutubeExplodeException(
            "Channel page is broken. " +
            "Please try again in a few minutes."
        );

    public async ValueTask<ChannelPageExtractor> GetChannelPageAsync(
        ChannelId channelId,
        CancellationToken cancellationToken = default) =>
        await GetChannelPageAsync("channel/" + channelId, cancellationToken);

    public async ValueTask<ChannelPageExtractor> GetChannelPageAsync(
        UserName userName,
        CancellationToken cancellationToken = default) =>
        await GetChannelPageAsync("user/" + userName, cancellationToken);

    public async ValueTask<ChannelPageExtractor> GetChannelPageAsync(
        ChannelSlug channelSlug,
        CancellationToken cancellationToken = default) =>
        await GetChannelPageAsync("c/" + channelSlug, cancellationToken);

    public async ValueTask<ChannelPageExtractor> GetChannelPageAsync(
        ChannelHandle channelHandle,
        CancellationToken cancellationToken = default) =>
        await GetChannelPageAsync("@" + channelHandle, cancellationToken);
}