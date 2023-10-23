# WebDMX
simple web interface (probably insecure) for interacting with DMX lighting.

Because of HttpListener, you must either "netsh http add urlacl url=http://+:8008/ user=(your username)"
in an administrator command prompt, or you must run the program as administrator!


After running, navigate to http://localhost:8008/ in your browser.

Requires System.IO.Ports' SerialPort to compile. You do not need any physical hardware to test
the program, but the functionality is there.