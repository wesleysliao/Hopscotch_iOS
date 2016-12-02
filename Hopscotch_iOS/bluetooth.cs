using System;
using System.Timers;

namespace CoreBluetooth
{
	public class MySimpleCBCentralManagerDelegate : CBCentralManagerDelegate
	{
		override public void UpdatedState(CBCentralManager central)
		{
			if (central.State == CBCentralManagerState.PoweredOn)
			{
				//Passing in null scans for all peripherals. Peripherals can be targeted by using CBUIIDs
				CBUUID[] cbuuids = null;
				central.ScanForPeripherals(cbuuids); //Initiates async calls of DiscoveredPeripheral
												 //Timeout after 30 seconds
				var timer = new Timer(30 * 1000);
				timer.Elapsed += (sender, e) => central.StopScan();
			}
			else {
				//Invalid state -- Bluetooth powered down, unavailable, etc.
				System.Console.WriteLine("Bluetooth is not available");
			}
		}

		public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, Foundation.NSDictionary advertisementData, Foundation.NSNumber RSSI)
		{
			Console.WriteLine("Discovered {0}, data {1}, RSSI {2}", peripheral.Name, advertisementData, RSSI);
		}

	}

}
