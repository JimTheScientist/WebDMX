// See https://aka.ms/new-console-template for more information

using System.Drawing;
using WebDMX;

/*
 * Because of HttpListener, you must either "netsh http add urlacl url=http://+:8008/ user=(your username)"
 * in an administrator command prompt, or you must run the program as administrator!
 *
 * After running, navigate to http://localhost:8008/ in your browser.
 */

class Program
{
    // Temporary universe

    public static Universe Universe1 = new Universe();

    static void Main(string[] args)
    {
        // The below lines are uncommented because if you don't have an arduino it'll just throw errors, but the program works without it.
        //Connection conn = new ArduinoConnection("COM6", 250000);
        //conn.Enable();
        //Universe1.AddConnection(conn);
        try
        {
            new WebInterface(8008);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("YOU PROBABLY DID NOT GIVE THE PROGRAM THE APPROPRIATE PERMISSIONS. READ THE COMMENT IN Program.cs!");
        }

    }
}