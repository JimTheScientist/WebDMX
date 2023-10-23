using System.Drawing;

namespace WebDMX;

/*
 * A fixture is exactly what it sounds like, a lighting fixture. See ParCan8Ch
 * for an example implementation.
 */

public abstract class Fixture
{
    
    // readonly because channels needs to be the length of the address space!
    private readonly List<Channel> _channels = new List<Channel>();

    // Address space is the number of addresses a fixture takes up  
    private int _addressSpace;

    protected Fixture(List<Channel> channels)
    {
        this._addressSpace = channels.Count;
        this._channels = channels;
    }

    public int GetAddressSpace()
    {
        return _addressSpace;
    }
    public List<Channel> GetChannels()
    {
        return _channels;
    }
    public Channel GetChannel(int channel)
    {
        return _channels[channel - 1];
    }
}