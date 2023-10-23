namespace WebDMX;

/*
 * In DMX512, a universe is 512 channels. Instead of storing a List of channels,
 * it is better to associate each fixture with a starting address in the universe.
 * A DMX connection is instantiated with a universe, which can then use the GetData
 * method to obtain values for all the channels.
 * 
 * This prevents a programmer from adding
 * addresses above 512.
 *
 * We also store what connection(s) this universe should be sent to. Why would you want multiple?
 * You may need them for different venues, backup connections in case something dies, or for a 3D viewer.
 */
public class Universe
{
    private List<Connection> _connectionList = new List<Connection>();
    private List<Tuple<Fixture, int>> _fixtures = new List<Tuple<Fixture, int>>();

    
    // TODO CHECKS TO MAKE SURE FIXTURES DON'T OVERLAP!!!!!
    public void AddFixture(Fixture fixture, int startingAddress)
    {
        if (startingAddress < 1 || startingAddress > 512)
        {
            throw new Exception("Starting address must be between 1-512");
        }
        _fixtures.Add(new Tuple<Fixture, int>(fixture, startingAddress));
    }

    public void SendData()
    {
        foreach (Connection connection in _connectionList)
        {
            connection.SendData(GetData());
        }
    }

    /*
     * This is the only part of the program that publicly returns byte values,
     * even though channels are really just byte values. This is because there is no reason to treat
     * the channels like bytes until they are being transmitted outside of the program.
     */
    public byte[] GetData()
    {
        byte[] data = new byte[512];
        Array.Fill(data, (byte) 0);
        foreach (Tuple<Fixture, int> address in this._fixtures)
        {
            int addressOffset = 0; 
            foreach (Channel channel in address.Item1.GetChannels())
            {
                // minus one because arrays start at 0 and our list is 1-512 instead of 0-511
                data[(address.Item2 - 1) + addressOffset] = (byte)channel.GetValue();
                addressOffset++;
            }
        }
        return data;
    }

    // Returns a "patch list", which is a list of each fixture type and the addresses they occupy.
    public List<Tuple<string, int, int>> GetPatchList()
    {
        List<Tuple<string, int, int>> patchList = new List<Tuple<string, int, int>>();
        foreach (Tuple<Fixture,int> fixture in _fixtures)
        {
            patchList.Add(new Tuple<string, int, int>(fixture.Item1.GetType().Name, fixture.Item2, fixture.Item1.GetAddressSpace())); 
        }

        return patchList;
    }

    public void AddConnection(Connection conn)
    {
        this._connectionList.Add(conn);
    }

    public void PrintDebug()
    {
        foreach (Tuple<Fixture,int> fixture in _fixtures)
        {
            Console.WriteLine(fixture.Item2);
            fixture.Item1.GetChannels().ForEach(channel => Console.WriteLine(channel.ChannelName + " " + channel.GetValue()));
        }
    }
}