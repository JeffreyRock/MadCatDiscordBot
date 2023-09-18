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
        var json = new JsonClass.Json();
        _client = new DiscordSocketClient();
        _client.Log += Log;
        var Token = json.ReturnValue("Token", Path.Combine( Directory.GetCurrentDirectory(),"..","..","..","json1.json"));

        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();
        Console.WriteLine($"{_client.CurrentUser} is connected");

        // sending a message 

        var channel = _client.GetChannel(944715418390626386) as SocketTextChannel;
        await channel.SendMessageAsync(MSG);

        await Task.Delay(-1);



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