using FFXIVAPI.Configs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace FFXIVAPI
{
    public class FfApi
    {

        public FfApi()
        {

        }

        /// <summary>
        /// Launches FFXIV with the supplied parameters.
        /// </summary>
        /// <param name="realsid">Real SessionID</param>
        /// <param name="language">language(0=japanese,1=english,2=french,3=german)</param>
        /// <param name="dx11">Runs the game in dx11 mode if true</param>
        /// <param name="expansionlevel">current level of expansions loaded(0=ARR/default,1=Heavensward)</param>
        public void LaunchGame(string realsid, int language, bool dx11, int expansionlevel)
        {
            try
            {
                /*var ffxivgame = new Process(); TODO: Add Process implementation
                if (dx11)
                {
                    ffxivgame.StartInfo.FileName = Settings.Get().GamePath() + "/game/ffxiv_dx11.exe";
                }
                else
                {
                    ffxivgame.StartInfo.FileName = Settings.Get().GamePath() + "/game/ffxiv.exe";
                }

                var startInfoArguments = $"DEV.TestSID={realsid} DEV.MaxEntitledExpansionID={expansionlevel} language={language}";
                ffxivgame.StartInfo.Arguments = startInfoArguments;
                ffxivgame.Start();*/
            }
            catch (Exception exc)
            {
                var op = new MessageDialog("Could not launch executable. Is your game path correct?\n\n" + exc, "Launch failed").ShowAsync();
            }
        }

        /// <summary>
        /// Gets a real SessionID for the supplied credentials.
        /// </summary>
        /// <param name="username">Sqare Enix ID</param>
        /// <param name="password">Password</param>
        /// <param name="otp">OTP</param>
        /// <returns></returns>
        public async Task<string> GetRealSid(string username, string password, string otp)
        {
            var hashstr = "";
            try
            {
                hashstr = "ffxivboot.exe/" + GenerateHash(Settings.Get().GamePath() + "/boot/ffxivboot.exe") + ",ffxivlauncher.exe/" + GenerateHash(Settings.Get().GamePath() + "/boot/ffxivlauncher.exe") + ",ffxivupdater.exe/" + GenerateHash(Settings.Get().GamePath() + "/boot/ffxivupdater.exe"); //make the string of hashed files to prove game version
            }
            catch (Exception exc)
            {
                var op = new MessageDialog("Could not generate hashes. Is your game path correct?\n\n" + exc, "Launch failed").ShowAsync();
            }

            var sidClient = new HttpClient();
            sidClient.DefaultRequestHeaders.Add("X-Hash-Check", "enabled");
            sidClient.DefaultRequestHeaders.Add("user-agent", "SQEXAuthor/2.0.0(Windows 6.2; ja-jp; 9e75ab3012)");
            sidClient.DefaultRequestHeaders.Add("Referer", "https://ffxiv-login.square-enix.com/oauth/ffxivarr/login/top?lng=en&rgn=3");
            sidClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            InitiateSslTrust();

            var uri = new Uri("https://patch-gamever.ffxiv.com/http/win32/ffxivneo_release_game/" + GetLocalGamever() + "/" + GetSid(username, password, otp));
            var post = await sidClient.PostAsync(uri, new HttpStringContent(hashstr));

            return post.Headers["X-Patch-Unique-Id"];
        }


        private string GetStored() //this is needed to be able to access the login site correctly
        {
            var loginInfo = new HttpClient();
            loginInfo.DefaultRequestHeaders.Add("user-agent", "SQEXAuthor/2.0.0(Windows 6.2; ja-jp; 9e75ab3012)");
            var reply = loginInfo.GetStringAsync(new Uri("https://ffxiv-login.square-enix.com/oauth/ffxivarr/login/top?lng=en&rgn=3&isft=0&issteam=0"));

            var storedre = new Regex(@"\t<\s*input .* name=""_STORED_"" value=""(?<stored>.*)"">");

            return storedre.Matches(reply.GetResults())[0].Groups["stored"].Value;
        }

        public async Task<string> GetSid(string username, string password, string otp)
        {
            using (var loginData = new HttpClient())
            {
                loginData.DefaultRequestHeaders.Add("user-agent", "SQEXAuthor/2.0.0(Windows 6.2; ja-jp; 9e75ab3012)");
                loginData.DefaultRequestHeaders.Add("Referer", "https://ffxiv-login.square-enix.com/oauth/ffxivarr/login/top?lng=en&rgn=3&isft=0&issteam=0");
                loginData.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

                var uri = new Uri("https://ffxiv-login.square-enix.com/oauth/ffxivarr/login/login.send");
                var content = new Dictionary<string, string>
                {
                    { "_STORED_", GetStored() },
                    { "sqexid", username },
                    { "password", password },
                    { "otppw", otp }
                };
                var resp =  //get the session id with user credentials
                await loginData.PostAsync(uri, new HttpFormUrlEncodedContent(content.ToList()));
                byte[] response = resp.Content.ReadAsBufferAsync().GetResults().ToArray();
                var reply = Encoding.UTF8.GetString(response);

                var sidre = new Regex(@"sid,(?<sid>.*),terms");
                return sidre.Matches(reply)[0].Groups["sid"].Value;
            }
        }

        private string GetLocalGamever()
        {
            try
            {
                var fs = $@"{Settings.Get().GamePath()}/game/ffxivgame.ver";
                using (var sr = new StreamReader(new FileStream(fs, FileMode.Open)))
                {
                    var line = sr.ReadToEnd();
                    return line;
                }
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        private string GenerateHash(string file)
        {
            var filebytes = File.ReadAllBytes(file);

            var hash = SHA1.Create().ComputeHash(filebytes);
            var hashstring = string.Join("", hash.Select(b => b.ToString("x2")).ToArray());

            var length = new FileInfo(file).Length;

            return length + "/" + hashstring;
        }

        public bool GetGateStatus()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var reply = client.GetStringAsync(new Uri("http://frontier.ffxiv.com/worldStatus/gate_status.json"));
                    return Convert.ToBoolean(int.Parse(reply.GetResults()[10].ToString()));
                }
            }
            catch (Exception exc)
            {
                var op = new MessageDialog("Failed getting gate status.\n\n" + exc, "Launch failed").ShowAsync();
                return false;
            }

        }

        private void InitiateSslTrust()
        {
            //Change SSL checks so that all checks pass, squares game ver server does strange things
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
    }
}
