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
		Hopscotch_iOS.ViewController viewController;

		public MySimpleCBCentralManagerDelegate(Hopscotch_iOS.ViewController view_controller) : base()
		{
			viewController = view_controller;
		}

		public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
		{
			tileController = peripheral;
			System.Console.WriteLine("Connected to " + tileController.Name);

			viewController.connectedToControllerBox();

			if (tileController.Delegate == null)
			{
				tileControllerDelegate = new SimplePeripheralDelegate(viewController, tileController);
				tileController.Delegate = tileControllerDelegate;
				//Begins asynchronous discovery of services
				tileController.DiscoverServices();
			}
		}

		public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, Foundation.NSError error)
		{
			viewController.disconnectedFromControllerBox();
		}

		override public void UpdatedState(CBCentralManager central)
		{
			if (central.State == CBCentralManagerState.PoweredOn)
			{
				//Passing in null scans for all peripherals. Peripherals can be targeted by using CBUIIDs
				CBUUID[] cbuuids = null;
				central.ScanForPeripherals(cbuuids); //Initiates async calls of DiscoveredPeripheral
													 //Timeout after 30 seconds
				var timer = new Timer(60 * 1000);
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
		CBPeripheral tileController;
		public CBCharacteristic dataReadCharacteristic;
		public CBCharacteristic dataWriteCharacteristic;
		Hopscotch_iOS.ViewController viewController;

		public SimplePeripheralDelegate(Hopscotch_iOS.ViewController view_controller, CBPeripheral tile_controller) : base()
		{
			viewController = view_controller;
			tileController = tile_controller;
		}

		public override void DiscoveredService(CBPeripheral peripheral, Foundation.NSError error)
		{
			System.Console.WriteLine("Discovered a service");
			foreach (var service in peripheral.Services)
			{
				if (service.UUID == CBUUID.FromString("FFE5") || service.UUID == CBUUID.FromString("FFE0"))
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
					peripheral.SetNotifyValue(true, dataReadCharacteristic);
				}
			}

		}

		int mapCharsLeft;
		byte[] allMapData;
		int index;

		public override void UpdatedCharacterteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, Foundation.NSError error)
		{
			if (dataReadCharacteristic.Value != null && characteristic == dataReadCharacteristic)
			{
				System.Console.WriteLine(dataReadCharacteristic.Value.ToString());
				var data = dataReadCharacteristic.Value.ToArray();

				if ((int)data[0] == 20)
				{
					mapCharsLeft = (data[1] * 6) + 2;
					allMapData = new byte[mapCharsLeft + 2];
					allMapData.Initialize();
					allMapData[1] = data[1];
					index = 0;

				}

				if (mapCharsLeft > 0)
				{
					
					data.CopyTo(allMapData, index);
					index += data.Length;
					mapCharsLeft -= data.Length;

					if (mapCharsLeft == 0)
					{
						ParseNetMap(allMapData);
					}
				}
				else
				{
					viewController.tileSteppedOn((int)dataReadCharacteristic.Value.ToArray()[0]);
				}
			}
		}

		void ParseNetMap(byte[] mapData)
		{
			var net_map = new int[mapData[1], 6];
			net_map.Initialize();
			int index = 2;
			for (int line = 0; line < (int)mapData[1]; line++)
			{
				for (int item = 0; item < 6; item++)
				{
					net_map[line, item] = (int)mapData[index];
					index++;
				}
			}

			viewController.ParseTileMap(net_map, (int)mapData[1]);
		}
	}

}
