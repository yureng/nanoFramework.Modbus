# nanoFramework.Modbus
Serial port modbus protocol implementation. 
Tested ESP32, other targets should be fine.

## Modbus service

First, you need to define a class that handles the client's request:
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

Modbus RTU Server
```
var server = new ModbusServer(new Device(1), "COM2");
server.ReadTimeout = server.WriteTimeout = 2000;
server.StartListening();
```

## Modbus client

Config the serial port (COM1)
```
Configuration.SetPinFunction(25, DeviceFunction.COM1_RX);
Configuration.SetPinFunction(26, DeviceFunction.COM1_TX);
Configuration.SetPinFunction(27, DeviceFunction.COM1_RTS);
```

Client
```
var client = new ModbusClient("COM1");
client.ReadTimeout = client.WriteTimeout = 2000;
```

Read the address 0x7 0x23
```
var data1 = client.ReadHoldingRegisters(2, 0x7, 4);
var data2 = client.ReadCoils(2, 0x23, 2);
```

Write the address 0x5 0x6
```
client.WriteMultipleRegisters(2, 0x5, new ushort[] { 3, 5, 2, 3 });
client.WriteRaw(2, 0x6, 4, new byte[] { 0, 2, 4, 6 }, FunctionCode.WriteMultipleRegisters);
```
