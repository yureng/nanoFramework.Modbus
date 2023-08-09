
* [🌶️ nanoFramework.Modbus has been moved to the official nanoFramework driver library.](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/Modbus/README.md)

# nanoFramework.Modbus
Serial port Modbus-RTU protocol implementation. 
Tested ESP32, other targets should be fine.

![Diagram](https://github.com/yureng/nanoFramework.Modbus/blob/main/src/ESP32_Demo/MAX485.png)

## Modbus service

First, you need to define a class that inherits from ModbusDevice to handle client requests:
```
class Device : ModbusDevice
{
     public Device(byte id = 1) : base(id)
     { 
     }
     
     protected override bool TryWriteHoldingRegister(ushort address, ushort value)
     {
          // the code write value to address...
          // If the address is invalid, false is returned
     }
     
     // The below code is similar
     protected override bool TryWriteCoil(ushort address, bool value)
     protected override bool TryReadInputRegister(ushort address, out ushort value)
     protected override bool TryReadHoldingRegister(ushort address, out ushort value)
     protected override bool TryReadDiscreteInput(ushort address, out bool value)
     protected override bool TryReadCoil(ushort address, out bool value)
}
```

Config the serial port (COM2)
```
Configuration.SetPinFunction(16, DeviceFunction.COM2_RX);
Configuration.SetPinFunction(17, DeviceFunction.COM2_TX);
Configuration.SetPinFunction(18, DeviceFunction.COM2_RTS);
```

Start the service
```
var server = new ModbusServer(new Device(1), "COM2");
server.ReadTimeout = server.WriteTimeout = 2000;
server.StartListening();
```

## Modbus client

Config the serial port (COM3)
```
Configuration.SetPinFunction(25, DeviceFunction.COM3_RX);
Configuration.SetPinFunction(26, DeviceFunction.COM3_TX);
Configuration.SetPinFunction(27, DeviceFunction.COM3_RTS);
```

Declare the client
```
var client = new ModbusClient("COM3");
client.ReadTimeout = client.WriteTimeout = 2000;
```

Read the address 0x7 0x23
```
var data1 = client.ReadHoldingRegisters(2, 0x7, 4);
var data2 = client.ReadCoils(2, 0x23, 2);
```

Write the address 0x5 or Diagnostics
```
client.WriteMultipleRegisters(2, 0x5, new ushort[] { 3, 5, 2, 3 });
client.Raw(2, FunctionCode.Diagnostics, new byte[] { 0x01, 0x01, 0x01, 0x01 });
```
