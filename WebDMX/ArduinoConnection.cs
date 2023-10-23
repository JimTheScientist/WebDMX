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
        _serialPort.Open();
        _serialPort.DiscardOutBuffer(); // TODO make this only happen on startup, along with Open and DisardInBuffer, and then create open and close methods in connection class
        _serialPort.DiscardInBuffer();
        Thread.Sleep(2000);
        _serialPort.Write(data,0,512);
        _serialPort.Close();

            //for (int i = 1; i<=512; i++)
        //{
        //    _serialPort.Write(data[i-1]);
        //    Console.WriteLine(Convert.ToString(data[i-1]));
        //}
    }
}