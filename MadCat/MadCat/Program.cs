using Discord;
using Discord.WebSocket;
using MadCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using JsonClass;
using AI;
using SQLConnector;

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

public class DiscordBot
{
    private DiscordSocketClient _client;

    public static Task main(string[] args) => new DiscordBot().MainAsync();
    public async Task MainAsync()
    {
        #region json Reading 
        var json = new JsonClass.Json();
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.Ready += ReadyAsync;
        var Token = json.ReturnValue("Token", Path.Combine( Directory.GetCurrentDirectory(),"..","..","..","json1.json"));

        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();
        Console.WriteLine($"{_client.CurrentUser} is connected");
        // sending a message 

        var SQL = new SQLConnector.SQLConnector();
        SQL.SQLBootUp();
        SQL.AddDiscordUser("BillyBob");
        await Task.Delay(-1);
    }

    private async Task ReadyAsync()
    {

        Console.WriteLine($"{_client.CurrentUser} is connected");
        var json = new JsonClass.Json();
        ChatGPU chatGPU = new ChatGPU();
        chatGPU.ApiToken = json.ReturnValue("ChatGPT", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
        
        ulong channelID = 944715418390626386UL;
        var channel = _client.GetChannel(channelID) as SocketTextChannel;
        await channel.SendMessageAsync("MadCat Online");

    }

    private Task Log(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;

    }
}

public class Commander
{

}