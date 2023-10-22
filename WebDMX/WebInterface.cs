using System.Net;
using System.Text;

namespace WebDMX;
// TODO make a registry for pages
/*
 * hard coded web framework because i am too lazy to use an actual framework.
 *
 * it turns out this is not as hard as you may think
 */

public class WebInterface
{
    private HttpListener _httpListener;
    public WebInterface(int port)
    {
        using (_httpListener = new HttpListener())
        {
            _httpListener.Prefixes.Add("http://+:"+port+"/");
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
        if (context.Request.Url.LocalPath == "/color")
        {
            string text;
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                text = reader.ReadToEnd();
            }
            
            Console.WriteLine(text);
            
            Program.Universe1.AddFixture(new ParCan8Ch(), Convert.ToInt32(text.Split("=")[1]));
            
        }
        // web page, template taken from https://www.w3schools.com/html/tryit.asp?filename=tryhtml_form_submit
        string tableBody = "";
        foreach (Tuple<string, int, int> fixture in Program.Universe1.GetPatchList())
        {
            tableBody += "<tr><td>" + fixture.Item1 + "</td><td>" + fixture.Item2 + "</td><td>" + fixture.Item3 + "</tr>";
        }
        
        string response = @"<!DOCTYPE html>
<html>
<body>

<h2>Patch Fixture</h2>

<form action=""/color"">
  <label for=""fname"">Fixture address:</label><br>
  <input type=""text"" id=""address"" name=""address"" value=""1""><br>
  <input type=""submit"" value=""Submit"" formmethod=""post"">
</form> 

<p>If you click the ""Submit"" button, the form data will be sent to a page called ""/color"" in a post request..</p>

<p>A new fixture will be created at the address specified above in the default universe found in Program.cs</p>
<table><tr>
      <th>Type</th>
      <th>Start</th>
      <th>Addresses</th>
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
    
}