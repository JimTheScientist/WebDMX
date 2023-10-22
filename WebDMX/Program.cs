// See https://aka.ms/new-console-template for more information

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