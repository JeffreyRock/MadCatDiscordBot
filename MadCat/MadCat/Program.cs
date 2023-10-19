using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO; 
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Discord.Rest;
using sun.net.idn;
using JsonClass;

namespace MadCat
{
    internal class Program
    {
        static void Main(string[] args)
        {

            new DiscordBot().MainAsync().GetAwaiter().GetResult(); 
        }
    }
}

public class DiscordBot : ControllerBase
{
    private DiscordSocketClient _client;
    private int OldCount;
    private string Owner;
    private Json json;

    public static Task main(string[] args) => new DiscordBot().MainAsync();
    public async Task MainAsync()
    {
        #region json Reading 
        json = new JsonClass.Json();
        var Token = json.ReturnValue("Token", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
        #endregion
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.Ready += ReadyAsync;
        _client.ReactionAdded += ReactionAddedRules;

        Owner = json.ReturnValue("Owner", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));

        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();
       
        Console.WriteLine($"{_client.CurrentUser} is connected");
        // sending a message 
        var SQL = new SQLConnector.SQLConnector();
        SQL.SQLBootUp();
        SQL.AddDiscordUser("BillyBob");
        await Task.Delay(-1);
    }

    private async Task ReactionAddedRules(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2, SocketReaction arg3)
    {
        var MessageID = json.ReturnValue("MessageID", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
        var WelcomeChannel = json.ReturnValue("WelcomeChannel", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
        Console.Write(arg2.Value.Id);
        if(arg2.Value.Id.ToString() == WelcomeChannel)
        {
            await ReactionAddedAsync(MessageID, WelcomeChannel, "\uD83D\uDC4D");
        }
        else
        {
            await ModerationCallBack(arg1.Id,arg2.Value.Id);
        }
    }

    private async Task ModerationCallBack(ulong MessageID, ulong ChannelID)
    {
        string Approve = json.ReturnValue("ApprovedEmoji", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
        string Report = json.ReturnValue("ReportEmoji", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
        ISocketMessageChannel channel = _client.GetChannel(ChannelID) as SocketTextChannel;
        if (channel is SocketTextChannel textChannel) {
            IMessage message = await textChannel.GetMessageAsync(MessageID);
            if (message != null) 
            {
                var ReactionsCheck1 = message.Reactions.FirstOrDefault(r => r.Key.Name == Approve);
                if (ReactionsCheck1.Value.ReactionCount >= 0)
                {
                    Console.WriteLine("working");
                }
                var ReactionsCheck2 = message.Reactions.FirstOrDefault(r => r.Key.Name == Report);
                if (ReactionsCheck2.Value.ReactionCount >= 0)
                {
                    Console.WriteLine("working");
                }
            }
        }
    }

    private async Task ReadyAsync()
    {

        Console.WriteLine($"{_client.CurrentUser} is connected");
        var json = new JsonClass.Json(); 
        ulong channelID = 944715418390626386UL;
        var channel = _client.GetChannel(channelID) as SocketTextChannel;
        await channel.SendMessageAsync("MadCat Online");
    }

    private async Task ReactionAddedAsync(string MessageIDString, string ChannelIDString, string Unicode)
    {
        try
        {
            ulong ChannelID, MessageID;

            ulong.TryParse(MessageIDString, out MessageID);
            ulong.TryParse(ChannelIDString, out ChannelID);

            ISocketMessageChannel channel = _client.GetChannel(ChannelID) as SocketTextChannel;
            if (channel is SocketTextChannel textChannel)
            {
                IMessage Rules = await textChannel.GetMessageAsync(MessageID);
                if (Rules != null)
                {
                    var Reactions = Rules.Reactions.FirstOrDefault(r => r.Key.Name == Unicode);
                    if (OldCount < Reactions.Value.ReactionCount)
                    {
                        OldCount = Reactions.Value.ReactionCount;
                        var User = await Rules.GetReactionUsersAsync(Reactions.Key, int.MaxValue).FlattenAsync();
                        var LatestUsers = User.OrderByDescending(u => u.Id).FirstOrDefault();
                        Console.WriteLine(LatestUsers);
                    }
                }
            }
        }
        catch (Exception e)    
        {
            ulong OwnerID = ulong.Parse(Owner);
            await SendDM(OwnerID, "Missing Rules Message: " + e.Message);
            ulong channelID = ulong.Parse(ChannelIDString);
            ISocketMessageChannel channel = _client.GetChannel(channelID) as SocketTextChannel;
            await channel.SendMessageAsync("Testing This Idea");
        }
        finally
        {
            Console.WriteLine("Working");
        }
    }

    private async Task SendDM(ulong UserID, string Message)
    {
        IUser User = await _client.GetUserAsync(UserID, RequestOptions.Default);
        if(User != null)
        {
            await User.SendMessageAsync(Message);
        }
        else
        {
            Console.WriteLine("Cannot Find User");
        }
    }

    private Task Log(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;

    }
}

