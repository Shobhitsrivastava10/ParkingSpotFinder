﻿namespace ParkingSpotFinderApp
{
	public class HatchBack : ICar
	{
		public string CarNumber { get; set; }
		public CarType Type
		{
			get { return CarType.Hatchback; }
		}

		public CarSpotType SpotType
		{
			get { return CarSpotType.Hatchback; }
		}

	}
}
