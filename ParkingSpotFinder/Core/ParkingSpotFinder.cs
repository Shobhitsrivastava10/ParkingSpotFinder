using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSpotFinderApp
{
	public class ParkingSpotFinder
	{
		private readonly ParkingSpotInfo[] _totalParkingSpots;
		private readonly Dictionary<string, ParkingSpotInfo> _parkedCar;
		private readonly ILogger _logger;

		public ParkingSpotFinder(Dictionary<CarSpotType, int> spotForEachType, ILogger<ParkingSpotFinder> logger)
		{
			_logger = logger;
			int count = 0;
			foreach (var item in spotForEachType)
			{
				count += item.Value;
			}
			_totalParkingSpots = new ParkingSpotInfo[count];

			_parkedCar = new Dictionary<string, ParkingSpotInfo>();

			int startPosition = 0;
			int endPosition = 0;
			foreach (var item in spotForEachType)
			{
				endPosition += item.Value;
				for (int i = startPosition; i < endPosition; i++)
				{
					_totalParkingSpots[i] = new ParkingSpotInfo { IsAvailable = true, SpotType = item.Key, StartPosition = i, ParkingCost = GetParkingCost(item.Key) };
				}
				startPosition += item.Value;
			}

		}

		/// <summary>
		/// Call to park the car. It requires car number and its parking spot
		/// </summary>
		/// <param name="car"></param>
		/// <param name="parkingSpot"></param>
		/// <returns></returns>
		public bool ParkCar(ICar car, ParkingSpotInfo parkingSpot = null)
		{
			try
			{
				if (car == null)
				{
					_logger.LogError($"No car for parking");
					return false;
				}

				//This is additional check if someone directly calls the park method.
				if (parkingSpot == null)
				{
					var availability = GetBestParkingOption(car);
					if (availability != null && availability.Item1)
					{
						parkingSpot = availability.Item2;
					}
				}

				if (parkingSpot == null)
				{
					_logger.LogError($"No parking spot found for car number {car.CarNumber}");
					return false;
				}

				if (_parkedCar.ContainsKey(car.CarNumber))
				{
					_logger.LogError($"Car with number - {car.CarNumber} is already parked");
					return false;
				}

				parkingSpot.IsAvailable = false;
				_totalParkingSpots[parkingSpot.StartPosition] = parkingSpot;

				//added car in parked car dictionary
				_parkedCar.Add(car.CarNumber, parkingSpot);

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"Failed to park car - {car.CarNumber} with error - {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Method to unpark the car using car object
		/// </summary>
		/// <param name="car"></param>
		/// <returns></returns>
		public bool UnParkCar(ICar car)
		{
			try
			{
				if (car == null)
				{
					_logger.LogError($"No car for Unparking");
					return false;
				}


				if (_parkedCar.ContainsKey(car.CarNumber) && _parkedCar.TryGetValue(car.CarNumber, out ParkingSpotInfo unparkedCar))
				{
					_parkedCar.Remove(car.CarNumber);

					unparkedCar.IsAvailable = true;
					_totalParkingSpots[unparkedCar.StartPosition] = unparkedCar;

					return true;
				}
				else
				{
					_logger.LogError($"Car with number - {car.CarNumber} is not parked");
				}
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"Failed to unpark car - {car.CarNumber} with error - {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Method to unpark using car number
		/// </summary>
		/// <param name="carNumber"></param>
		/// <returns></returns>
		public bool UnParkCar(string carNumber)
		{
			try
			{
				if (string.IsNullOrEmpty(carNumber))
				{
					_logger.LogError($"CarNumber is missing for Unparking");
					return false;
				}


				if (_parkedCar.ContainsKey(carNumber) && _parkedCar.TryGetValue(carNumber, out ParkingSpotInfo unparkedCar))
				{
					_parkedCar.Remove(carNumber);

					unparkedCar.IsAvailable = true;
					_totalParkingSpots[unparkedCar.StartPosition] = unparkedCar;

					return true;
				}
				else
				{
					_logger.LogError($"Car with number - {carNumber} is not parked");
				}
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"Failed to unpark car - {carNumber} with error - {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Get best/nearest parking option for a car
		/// </summary>
		/// <param name="car"></param>
		/// <returns></returns>
		public Tuple<bool, ParkingSpotInfo> GetBestParkingOption(ICar car)
		{
			try
			{
				if (car == null)
				{
					_logger.LogError($"Car details are missing to find best possible spot");
					return new Tuple<bool, ParkingSpotInfo>(false, null);
				}
				if (_totalParkingSpots == null || _totalParkingSpots.Length <= 0)
				{
					_logger.LogError($"No parking spots is present on the floor");
					return new Tuple<bool, ParkingSpotInfo>(false, null);
				}

				ParkingSpotInfo parkingSpot = _totalParkingSpots.FirstOrDefault(x => x.IsAvailable && x.SpotType == car.SpotType);

				bool spotFound = false;

				if (parkingSpot == null || !parkingSpot.IsAvailable)
				{
					foreach (CarSpotType i in Enum.GetValues(typeof(CarSpotType)))
					{
						if (!spotFound && i > car.SpotType)
						{
							parkingSpot = _totalParkingSpots.FirstOrDefault(x => x.IsAvailable && x.SpotType == i);

							spotFound = parkingSpot != null && parkingSpot.IsAvailable;
						}
					}
				}
				else
				{
					spotFound = true;
				}


				return new Tuple<bool, ParkingSpotInfo>(spotFound, parkingSpot);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"Failed to get best parking option for car - {car.CarNumber} with error - {ex.Message}");
				return new Tuple<bool, ParkingSpotInfo>(false, null);
			}
		}

		/// <summary>
		/// Return parking cost based on car type
		/// </summary>
		/// <param name="spotType"></param>
		/// <returns></returns>
		private decimal GetParkingCost(CarSpotType spotType)
		{
			switch (spotType)
			{
				case CarSpotType.Hatchback:
					return 50;
				case CarSpotType.Sedan:
					return 75;
				case CarSpotType.MiniTruck:
					return 100;
				default:
					return 75;


			}
		}
	}
}
