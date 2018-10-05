using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Threading.Tasks;
using DiscordBot.Tools;
using HtmlAgilityPack;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;


namespace DiscordBot.Commands
{
    public class LolCommands
    {
        private Dictionary<string, IEnumerable<string>> _counter;

        /// <summary>
        /// Scrapping lolcounterpicks to get a dictionnary of counter picks
        /// </summary>
        /// <param name="ctx">Command context (containing champion name)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [Command("counter")]
        public async Task GetCounter(CommandContext ctx)
        {
            var searchedChampion = ctx.RawArgumentString.Trim()?.ToLower();

            if (string.IsNullOrWhiteSpace(searchedChampion))
                throw new ArgumentException("Invalid user input");

            if (_counter == null)
            {
                const string url = "http://www.lolcounterpicks.com/champions/table";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var values = doc.DocumentNode.SelectNodes("//tr").Where(item => item.HasClass("tablerow"));
                _counter = new Dictionary<string, IEnumerable<string>>();

                foreach (var tab in values)
                {
                    try
                    {
                        var champions = tab.SelectNodes("./td/a/img");

                        if (champions == null)
                            continue;

                        var championValues = champions.Where(champion => champion.Attributes["alt"] != null)
                            .Select(champion => champion.Attributes["alt"].Value).ToList();

                        _counter.Add(championValues[0].ToLower(), championValues.Skip(1));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            if (!_counter.ContainsKey(searchedChampion))
            {
                await ctx.RespondAsync($"{searchedChampion} not found.");
                return;
            }

            await ctx.RespondAsync($"{string.Join("; ", _counter[searchedChampion])}");
        }

        /// <summary>
        /// Scrapping champion.gg to get build
        /// </summary>
        /// <param name="ctx">command context</param>
        /// <param name="champion">champion's name to get his buiild</param>
        /// <param name="lane">champion's lane (not mandatory)</param>
        /// <returns></returns>
        [Command("build")]
        public async Task GetBuild(CommandContext ctx, string champion, string lane = "")
        {
            List<string> values;

            try
            {
                var url = $"https://champion.gg/champion/{champion}/{lane}";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                values = doc.DocumentNode.SelectNodes("//div[contains(@class, 'build-wrapper')]/a/img")
                    .Select(item => "https:" + item.Attributes["src"].Value).Take(6).ToList();
            }
            catch (Exception)
            {
                throw new Exception("Invalid champion / lane");
            }

            var finalPicture = new Bitmap(85 * 6, 85);
            var i = 0;
            Graphics g = Graphics.FromImage(finalPicture);

            foreach (var value in values)
            {
                WebRequest req = WebRequest.Create(value);
                WebResponse response = req.GetResponse();
                Stream stream = response.GetResponseStream();
                var tmpPicture = new Bitmap(stream);
                g.DrawImage(tmpPicture, new Point(85 * i, 0));
                tmpPicture.Dispose();
                i++;
            }

            g.Flush();

            using (var stream = new MemoryStream())
            {
                finalPicture.Save(stream, ImageFormat.Jpeg);
                stream.Seek(0, SeekOrigin.Begin);
                await ctx.RespondWithFileAsync(stream, "file.jpg");
            }

            g.Dispose();
            finalPicture.Dispose();
        }

        

    }
}