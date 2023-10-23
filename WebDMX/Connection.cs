namespace WebDMX;


/*
 * A connection provides a method of sending 1 universe of data (512 channels) to a connection device.
 * This device could be ArtNet (networking protocol), an Arduino, a USB-DMX adapter, etc. and therefore
 * is abstract to provide for all the different types of connections. Those connections can be
 * connected to physical devices or they could go to a 3D viewer (like MA Lighting's 3D viewer)
 *
 * Ideally, IsEnabled, Enable, and Disable methods should exist, but such convenient features of a
 * program are out of the scope of this project.
 */
public abstract class Connection
{
    abstract public void SendData(byte[] data);
}