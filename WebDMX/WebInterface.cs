using System.Drawing;
using System.Net;
using System.Text;

namespace WebDMX;
// TODO make a registry for pages
/*
 * hard coded web framework because i am too lazy to use an actual framework.
 *
 * it turns out this is not as hard as you may think
 *
 * This is just in charge of user input, and it works! sorry it looks awful...
 */

public class WebInterface
{
    private HttpListener _httpListener;

    public WebInterface(int port)
    {
        using (_httpListener = new HttpListener())
        {
            _httpListener.Prefixes.Add("http://+:" + port + "/");
            _httpListener.Start();
            _httpListener.BeginGetContext(GetContextCallback, null);
            Console.WriteLine("Web interface created. Hit Enter to close.");
            Console.ReadLine();
        }
    }

    public void GetContextCallback(IAsyncResult asyncResult)
    {
        HttpListenerContext context = this._httpListener.EndGetContext(asyncResult);
        _httpListener.BeginGetContext(GetContextCallback, null);
        Console.WriteLine(context.Request.Url.LocalPath);
        string text;
        using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        {
            text = reader.ReadToEnd();
        }
        Console.WriteLine(text);

        Dictionary<string,string> postData = ConvertPostData(text);
        switch (context.Request.Url.LocalPath)
        {
            case "/add-fixture":
                context.Response.Redirect("/");
                if (postData["name"] == "ParCan8Ch")
                {
                    Program.Universe1.AddFixture(new ParCan8Ch(), Convert.ToInt32(postData["address"]));
                }
                break;
            case "/change-channel":
                context.Response.Redirect("/");
                List<Tuple<Fixture, int, int, long>> patchList = Program.Universe1.GetPatchList();
                foreach (Tuple<Fixture,int,int,long> tuple in patchList)
                {
                    if (postData["fid"] == tuple.Item4.ToString())
                    {
                        foreach (Channel channel in tuple.Item1.GetChannels())
                        {
                            if (postData["channelname"] == channel.ChannelName)
                            {
                                channel.SetValue(Convert.ToInt32(postData["channelValue"]));
                            }
                        }
                    }
                }

                break;
        }
        Program.Universe1.SendData();


        //if (context.Request.Url.LocalPath == "/color")
        //{
        //
        //    Console.WriteLine(text);
        //    
        //    Program.Universe1.AddFixture(new ParCan8Ch(), Convert.ToInt32(text.Split("=")[1]));
        //    
        //}
        //This is where the web page is created and sent back. No matter what this is the webpage. Routes are only used for the post request data.
        // web page, template taken from https://www.w3schools.com/html/tryit.asp?filename=tryhtml_form_submit
        string tableBody = "";
        foreach (Tuple<Fixture, int, int, long> fixture in Program.Universe1.GetPatchList())
        {
            string options = "";
            fixture.Item1.GetChannels().ForEach(channel => options += "<option value=\"" + channel.ChannelName + "\">" + channel.ChannelName + "</option>");
            tableBody += "<tr><td>" + fixture.Item1.GetType().Name + "</td><td>" + fixture.Item2 + "</td><td>" + fixture.Item3 + "</td><td>" + $@"<form action=""/change-channel"">
                <label for=""channelname"">Channel Name:</label><br>
                  <select id=""channelname"" name=""channelname"">
    {options}
  </select>
                <label for=""channelValue"">Channel Value:</label><br>
                <input type=""text"" id=""channelValue"" name=""channelValue""><br>
                <input type = ""hidden"" name = ""fid"" value = ""{fixture.Item4}"" />
                <input type=""submit"" value=""Submit"" formmethod=""post"">
                </form> " + 
                         "</td></tr>";
        }

        string response = @"<!DOCTYPE html>
<html>
<style>
table, th, td {
  border: 1px solid;
  border-collapse: collapse;
  table-layout: auto;
  width: 1225px;
}
</style>
<body>

<h2>Patch Fixture</h2>

<form action=""/add-fixture"">
  <label for=""fname"">Fixture address:</label><br>
  <input type=""text"" id=""address"" name=""address"" value=""1""><br>
  <label for=""fname"">Fixture name:</label><br>

  <select id=""name"" name=""name"">
    <option value=""ParCan8Ch"">ParCan8Ch</option>
  </select>
<br>
  <input type=""submit"" value=""Submit"" formmethod=""post"">
</form> 

<p>If you click the ""Submit"" button, the form data will be sent to a page called ""/color"" in a post request..</p>

<p>A new fixture will be created at the address specified above in the default universe found in Program.cs</p>
<table><tr>
      <th>Type</th>
      <th>Start</th>
      <th>Addresses</th>
      <th>ChangeValue</th>
    </tr>" + tableBody + @"</table>
</body>
</html>";
        context.Response.ContentType = "text/html";
        byte[] buffer = Encoding.UTF8.GetBytes(response);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.StatusCode = 200;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
    }

    public Dictionary<string, string> ConvertPostData(string postData)
    {
        Dictionary<string,string> dict = new Dictionary<string, string>();
        foreach (string s in postData.Split("&"))
        {
            Console.WriteLine(s.Split("=")[0]);
            try
            {
                dict.Add(s.Split("=")[0], s.Split("=")[1]);
            }
            catch (Exception e)
            {
            }
        }
        return dict;
    }
}