namespace WebDMX;
/*
 * In DMX512, a channel's value is intended to be an unsigned byte, a value between 0-255.
 * We do not store the address here because fixtures may be re-patched (The starting addres can change).
 */
public class Channel
{
    // Channel name so we can build debug info later...
    public string ChannelName = "Unnamed Channel";
    public Channel(){}

    public Channel(string channelName)
    {
        this.ChannelName = channelName;
    }
    private int _value = 0;

    public void SetValue(int value)
    {
        if (value > 255 || value < 0)
        {
            throw new Exception("Value must be between 0-255!");
        }

        this._value = value;
    }
    
    public int GetValue()
    {
        return _value;
    }
    
}