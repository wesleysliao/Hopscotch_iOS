using System;
using System.Timers;
using CoreFoundation;
using UIKit;

namespace CoreBluetooth
{
	public class MySimpleCBCentralManagerDelegate : CBCentralManagerDelegate
	{
		public CBPeripheral tileController;
		public SimplePeripheralDelegate tileControllerDelegate;

		public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
		{
			tileController = peripheral;
			System.Console.WriteLine("Connected to " +tileController.Name);

			if (tileController.Delegate == null)
			{
				tileControllerDelegate = new SimplePeripheralDelegate();
				tileController.Delegate = tileControllerDelegate;
				//Begins asynchronous discovery of services
				tileController.DiscoverServices();
			}

		}

		override public void UpdatedState(CBCentralManager central)
		{
			if (central.State == CBCentralManagerState.PoweredOn)
			{
				//Passing in null scans for all peripherals. Peripherals can be targeted by using CBUIIDs
				CBUUID[] cbuuids = null;
				central.ScanForPeripherals(cbuuids); //Initiates async calls of DiscoveredPeripheral
												 //Timeout after 30 seconds
				var timer = new Timer(10 * 1000);
				timer.Elapsed += (sender, e) => central.StopScan();
			}
			else {
				//Invalid state -- Bluetooth powered down, unavailable, etc.
				System.Console.WriteLine("Bluetooth is not available");
			}
		}

		public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, Foundation.NSDictionary advertisementData, Foundation.NSNumber RSSI)
		{
			
			Console.WriteLine("Discovered {0}, data {1}", peripheral.Name, advertisementData);

			if (peripheral.Name == "Tv221u-4C701516")
			{
				central.ConnectPeripheral(peripheral);
			}

		}

		public void sendLightTile(int tileID)
		{
			if (tileControllerDelegate != null && tileControllerDelegate.dataWriteCharacteristic != null)
			{
				var idbyte = new byte[1];
				idbyte[0] = (byte)tileID;
				tileController.WriteValue(Foundation.NSData.FromArray(idbyte), tileControllerDelegate.dataWriteCharacteristic, CBCharacteristicWriteType.WithoutResponse);
			}
		}
	}

	public class SimplePeripheralDelegate : CBPeripheralDelegate
	{
		public CBCharacteristic dataReadCharacteristic;
		public CBCharacteristic dataWriteCharacteristic;
		
		public override void DiscoveredService(CBPeripheral peripheral, Foundation.NSError error)
		{
			System.Console.WriteLine("Discovered a service");
			foreach (var service in peripheral.Services)
			{
				if (service.UUID == CBUUID.FromString("FFE5"))
				{
					System.Console.WriteLine(service.UUID.ToString());
					peripheral.DiscoverCharacteristics(service);
				}
			}
		}

		public override void DiscoveredCharacteristic(CBPeripheral peripheral, CBService service, Foundation.NSError error)
		{
			System.Console.WriteLine("Discovered characteristics of " + peripheral);
			foreach (var c in service.Characteristics)
			{
				Console.WriteLine(c.ToString());
				peripheral.ReadValue(c);
				if (c.UUID == CBUUID.FromString("FFE9"))
				{
					dataWriteCharacteristic = c;
				}

				if (c.UUID == CBUUID.FromString("FFE4"))
				{
					dataReadCharacteristic = c;
				}
			}

		}

		public override void UpdatedValue(CBPeripheral peripheral, CBDescriptor descriptor, Foundation.NSError error)
		{
			Console.WriteLine("Value of characteristic " + descriptor.Characteristic + " is " + descriptor.Value);
		}

		public override void UpdatedCharacterteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, Foundation.NSError error)
		{
			Console.WriteLine("Value of characteristic " + characteristic.ToString() + " is " + characteristic.Value);
		}
	}

}
