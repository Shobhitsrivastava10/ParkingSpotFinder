using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ParkingSpotFinderApp;
using System.Collections.Generic;

namespace ParkingSpotFinderTestProject
{
	public class Tests
	{
		private Mock<ILogger<ParkingSpotFinder>> _mockLogger;
		public ParkingSpotFinder _spotFinder;

		public ParkingSpotInfo[] _totalParkingSpots;
		public Dictionary<string, ParkingSpotInfo> _parkedCar;
		public Dictionary<CarSpotType, int> _spotForEachType;

		[SetUp]
		public void Setup()
		{
			_mockLogger = new Mock<ILogger<ParkingSpotFinder>>();
			_spotForEachType = new Dictionary<CarSpotType, int>();
			_spotForEachType.Add(CarSpotType.Hatchback, 10);
			_spotForEachType.Add(CarSpotType.Sedan, 10);
			_spotForEachType.Add(CarSpotType.MiniTruck, 10);

		}

		[Test]
		public void GetFirstSpotForHatchBack()
		{
			var car = new HatchBack() { CarNumber = "221" };
			_spotFinder = new ParkingSpotFinder(_spotForEachType, _mockLogger.Object);

			var parkingDetail = _spotFinder.GetBestParkingOption(car);

			Assert.IsTrue(parkingDetail.Item1);
			Assert.AreEqual(parkingDetail.Item2.StartPosition,0);
			Assert.GreaterOrEqual(parkingDetail.Item2.ParkingCost, 50);
		}

		[Test]
		public void GetBestSpot()
		{
			var car1 = new HatchBack() { CarNumber = "221" };
			var car2 = new HatchBack() { CarNumber = "122" };
			var car3 = new Sedan() { CarNumber = "123" };
			var car4 = new HatchBack() { CarNumber = "124" };
			var car5 = new HatchBack() { CarNumber = "125" };
			
			_spotFinder = new ParkingSpotFinder(_spotForEachType, _mockLogger.Object);

			var parkingDetailCar1 = _spotFinder.GetBestParkingOption(car1);
			bool parkCar1 = _spotFinder.ParkCar(car1, parkingDetailCar1.Item2);

			var parkingDetailCar2 = _spotFinder.GetBestParkingOption(car2);
			bool parkCar2 = _spotFinder.ParkCar(car2, parkingDetailCar2.Item2);

			var parkingDetailCar3 = _spotFinder.GetBestParkingOption(car3);
			bool parkCar3 = _spotFinder.ParkCar(car3, parkingDetailCar3.Item2);

			var parkingDetailCar4 = _spotFinder.GetBestParkingOption(car4);
			bool parkCar4 = _spotFinder.ParkCar(car4, parkingDetailCar4.Item2);

			var unParkCar2 = _spotFinder.UnParkCar(car2);
			var parkingDetailCar5 = _spotFinder.GetBestParkingOption(car5);

			Assert.IsTrue(parkingDetailCar5.Item1);
			Assert.AreEqual(parkingDetailCar5.Item2.StartPosition, 1);
			Assert.GreaterOrEqual(parkingDetailCar5.Item2.ParkingCost, 50);
		}
	}
}