using System.Drawing;
using System.Threading.Channels;

namespace WebDMX;

/*
 * This fixture is similar to this one here
 * https://www.bhphotovideo.com/lit_files/483154.pdf
 *
 * The channel specifications (on page 4) are slightly different from what I have.
 * 
 * A physical fixture. The channels are as follows:
 * 1: Master Dimmer
 * 2-4: I do not want to implement now (macros, strobe)
 * 5: Red
 * 6: Green
 * 7: Blue
 * 8: White
 */

public class ParCan8Ch : Fixture
{
    public ParCan8Ch() : base(InstantiateChannels())
    {
    }
    
    private static List<Channel> InstantiateChannels()
    {
        List<Channel> channels = new List<Channel>();
        channels.Add(new Channel("Master"));
        channels.Add(new Channel());
        channels.Add(new Channel());
        channels.Add(new Channel());
        channels.Add(new Channel("Red"));
        channels.Add(new Channel("Green"));
        channels.Add(new Channel("Blue"));
        channels.Add(new Channel("White"));
        return channels;
    }

    public void SetBrightness(int value)
    {
        GetChannel(1).SetValue(value);
    }
    
    public void SetWhite(int value)
    {
        GetChannel(8).SetValue(value);
    }

    public void SetColor(Color color)
    {
        // Set red
        GetChannel(5).SetValue(color.R);
        // Set green
        GetChannel(6).SetValue(color.G);
        // Set red
        GetChannel(7).SetValue(color.B);
    }
}