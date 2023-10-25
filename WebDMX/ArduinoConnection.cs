namespace WebDMX;

using System.IO.Ports;

/*
 * For my ECEN class, I wrote a program which can use the Arduino to communicate with DMX lighting.
 * This connection type allows you to send data to an arduino, which will then control DMX lighting.
 */

public class ArduinoConnection : Connection
{
    private SerialPort _serialPort;

    public ArduinoConnection(string portName, int baudRate)
    {
        // create new serial port and set name + baud rate
        _serialPort = new SerialPort();
        _serialPort.PortName = portName;
        _serialPort.BaudRate = baudRate;
    }
    public override void SendData(byte[] data)
    {
        Console.WriteLine(data);
        _serialPort.Write(data,0,512);

        //for (int i = 1; i<=512; i++)
        //{
        //    _serialPort.Write(data[i-1]);
        //    Console.WriteLine(Convert.ToString(data[i-1]));
        //}
    }

    /*
     * Unfortunately we can't force the compiler / LSP to make programmers write exceptions when calling something like
     * this like we can in java with the "throws" declaration. *sigh*
     *
     * (that means use a try catch future me)
     */
    public override void Enable()
    {
        _serialPort.Open();
        _serialPort.DiscardOutBuffer();
        _serialPort.DiscardInBuffer();
        Thread.Sleep(2000);
        this._enabled = true;
    }

    public override void Disable()
    {
        _serialPort.Close();
        this._enabled = false;
    }
}