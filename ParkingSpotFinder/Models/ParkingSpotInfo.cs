﻿namespace ParkingSpotFinderApp
{
	public class ParkingSpotInfo
	{
		public int StartPosition { get; set; }
		public CarSpotType SpotType { get; set; }

		public decimal ParkingCost { get; set; }
		public bool IsAvailable { get; set; }
	}
}
