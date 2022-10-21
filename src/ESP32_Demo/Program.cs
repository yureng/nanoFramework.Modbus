using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Hardware.Esp32;

using nF.Modbus.Client;
using nF.Modbus.Server;

namespace nF.Modbus.Demo
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            // SerialPort COM1
            Configuration.SetPinFunction(25, DeviceFunction.COM1_RX);
            Configuration.SetPinFunction(26, DeviceFunction.COM1_TX);
            Configuration.SetPinFunction(27, DeviceFunction.COM1_RTS);

            // SerialPort COM2
            Configuration.SetPinFunction(16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(17, DeviceFunction.COM2_TX);
            Configuration.SetPinFunction(18, DeviceFunction.COM2_RTS);

            // Modbus Server
            var server = new ModbusServer(new Device(1), "COM2");
            server.ReadTimeout = server.WriteTimeout = 2000;
            server.StartListening();

            // Modbus Client
            var client = new ModbusClient("COM1");
            client.ReadTimeout = client.WriteTimeout = 2000;

            client.WriteMultipleRegisters(2, 0x5, new ushort[] { 3, 5, 2, 3 });
            client.WriteRaw(2, 0x6, 4, new byte[] { 0, 2, 4, 6 }, FunctionCode.WriteMultipleRegisters);

            var data1 = client.ReadHoldingRegisters(2, 0x7, 4);
            var data2 = client.ReadCoils(2, 0x23, 2);

            Thread.Sleep(Timeout.Infinite);
        }
    }

    class Device : ModbusDevice
    {
        public Device(byte id = 1) : base(id)
        { 
        }

        /// <summary>
        /// Read Coil
        /// </summary>
        /// <param name="address">Register address</param>
        /// <param name="value">Output register values</param>
        /// <returns>True: address is readable; False: address is invalid</returns>
        protected override bool TryReadCoil(ushort address, out bool value)
        {
            // Your code is here...
            switch (address)
            {
                case 100:
                    value = false;  // Gets the switch status
                    break;
                case 101:
                    value = true;  // Gets the switch status
                    break;
                default:
                    value = false;
                    return false;
            }
            return true;
        }

        protected override bool TryReadDiscreteInput(ushort address, out bool value)
        {
            // Similar to the code above...
            throw new NotImplementedException();
        }

        protected override bool TryReadHoldingRegister(ushort address, out ushort value)
        {
            // Similar to the code above...
            throw new NotImplementedException();
        }

        protected override bool TryReadInputRegister(ushort address, out ushort value)
        {
            // Similar to the code above...
            throw new NotImplementedException();
        }

        protected override bool TryWriteCoil(ushort address, bool value)
        {
            // Similar to the code above...
            throw new NotImplementedException();
        }

        protected override bool TryWriteHoldingRegister(ushort address, ushort value)
        {
            // Similar to the code above...
            throw new NotImplementedException();
        }
    }
}
